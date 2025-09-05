using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class MockServer
{
    private HttpListener listener;
    private Dictionary<(string, string), string> mockResponses;
    private Dictionary<string, string> fallbackResponses;

    public MockServer(string url)
    {
        listener = new HttpListener();
        listener.Prefixes.Add(url);
        mockResponses = new Dictionary<(string, string), string>();
        fallbackResponses = new Dictionary<string, string>();
    }

    public void AddMockResponse(string requestUrl, string soapAction, string responseContent)
    {
        mockResponses[(requestUrl, soapAction)] = responseContent;
    }

    public void AddFallbackResponse(string requestUrl, string responseContent)
    {
        fallbackResponses[requestUrl] = responseContent;
    }

    public async Task StartAsync()
    {
        listener.Start();
        Console.WriteLine($"Listening for requests on {listener.Prefixes.First()}");

        while (listener.IsListening)
        {
            try
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;



                string soapAction = request.Headers["SOAPAction"]?.Trim('"') ?? string.Empty;
                Console.WriteLine($"Received request: {request.HttpMethod} {request.Url.AbsolutePath} SOAPAction: {soapAction}");

                // Set common headers
                response.ContentType = "text/xml; charset=UTF-8";
                response.Headers.Add("Server", "nginx/1.10.3 (Ubuntu)");
                response.Headers.Add("Date", DateTime.UtcNow.ToString("r"));

                string UserId = "100000001";

                if (request.HttpMethod == "POST")
                {
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string requestBody = await reader.ReadToEndAsync();
                        //Console.WriteLine($"Request body: {requestBody}");

                        var match = new Regex(@"ownerid&#x20;=&#x20;(\d+)").Match(requestBody);
                        if (match.Success)
                        {
                            UserId = match.Groups[1].Value;
                        }
                    }
                }

                if (mockResponses.TryGetValue((request.Url.AbsolutePath, soapAction), out string responseContent))
                {
                    Console.WriteLine($"Found exact match for {request.Url.AbsolutePath} with SOAPAction: {soapAction}");
                    response.StatusCode = 200;

                    if (soapAction == "http://gamespy.net/sake/SearchForRecords")
                    {
                        Console.WriteLine("Replacing with DATE=" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"));
                        responseContent = responseContent.Replace("DATE_NOW", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssK"));
                        Console.WriteLine("Replacing with userId=" + UserId);
                        responseContent = responseContent.Replace("100000001", UserId);
                    }

                    var bytes = Encoding.UTF8.GetBytes(responseContent);
                    response.ContentLength64 = bytes.Length;

                    await response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
                }
                else if (fallbackResponses.TryGetValue(request.Url.AbsolutePath, out string fallbackContent))
                {
                    Console.WriteLine($"Found fallback for {request.Url.AbsolutePath}");
                    response.StatusCode = 200;
                    response.ContentLength64 = fallbackContent.Length;
                    await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(fallbackContent), 0, fallbackContent.Length);
                }
                else
                {
                    Console.WriteLine($"No response found for {request.Url.AbsolutePath} with SOAPAction: {soapAction}");
                    response.StatusCode = 404; // Not Found
                    byte[] errorContent = Encoding.UTF8.GetBytes("<error>Not Found</error>");
                    response.ContentLength64 = errorContent.Length;
                    await response.OutputStream.WriteAsync(errorContent, 0, errorContent.Length);
                }


                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling request: {ex.Message}");
            }
        }
    }

    public void Stop()
    {
        listener.Stop();
        Console.WriteLine("Server stopped");
    }


}