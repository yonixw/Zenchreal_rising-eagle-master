using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class MockServerService
{
    public static async Task Main()
    {
        Console.WriteLine("Starting PCAP2CURL Mock Server...");
        
        // Create the mock server
        string baseUrl = "http://*:80/";
        var server = new MockServer(baseUrl);
        
        // Load and add mock responses for AuthService
        Console.WriteLine("Loading AuthService mock responses...");

        // LoginProfile response
        string loginProfileResponse = @"<SOAP-ENV:Envelope xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ZSI=""http://www.zolera.com/schemas/ZSI/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body xmlns:ns1=""http://gamespy.net/AuthService/""><ns1:LoginProfileResponse><ns1:LoginProfileResult><ns1:responseCode>0</ns1:responseCode><ns1:profileId>22222</ns1:profileId><ns1:userId>11111</ns1:userId></ns1:LoginProfileResult></ns1:LoginProfileResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        server.AddMockResponse("/AuthService/AuthService.asmx", "http://gamespy.net/AuthService/LoginProfile", loginProfileResponse);

        // LoginRemoteAuth response
        string loginRemoteAuthResponse = @"<SOAP-ENV:Envelope xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ZSI=""http://www.zolera.com/schemas/ZSI/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body xmlns:ns1=""http://gamespy.net/AuthService/""><ns1:LoginRemoteAuthResponse><ns1:LoginRemoteAuthResult><ns1:responseCode>0</ns1:responseCode><ns1:profileId>22222</ns1:profileId><ns1:userId>11111</ns1:userId></ns1:LoginRemoteAuthResult></ns1:LoginRemoteAuthResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        server.AddMockResponse("/AuthService/AuthService.asmx", "http://gamespy.net/AuthService/LoginRemoteAuth", loginRemoteAuthResponse);

        // LoginUniqueNick response
        string loginUniqueNickResponse = @"<SOAP-ENV:Envelope xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ZSI=""http://www.zolera.com/schemas/ZSI/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body xmlns:ns1=""http://gamespy.net/AuthService/""><ns1:LoginUniqueNickResponse><ns1:LoginUniqueNickResult><ns1:responseCode>0</ns1:responseCode><ns1:certificate><ns1:length>303</ns1:length><ns1:version>1</ns1:version><ns1:partnercode>11067</ns1:partnercode><ns1:namespaceid>2</ns1:namespaceid><ns1:userid>11111</ns1:userid><ns1:profileid>22222</ns1:profileid><ns1:expiretime>0</ns1:expiretime><ns1:profilenick>Jackalus</ns1:profilenick><ns1:uniquenick>Jackalus</ns1:uniquenick><ns1:cdkeyhash>1234</ns1:cdkeyhash><ns1:peerkeymodulus>393533373534363545334641433439303046433931324537423330454637313731423035343644463444313835444230344632314337393135334345303931383539444632454244444645353034374438304332454638364132313639423035413933334145324541423239363246374233324346453343423043323545374533413236424236353334433943463139363430463131343337335244304345414137414138384344363441434534363545423033373030373536374631454335314430304331443246314646434645434235333030433933443644364135304331453342443634393546433137363031373934453536353543343736383139</ns1:peerkeymodulus><ns1:peerkeyexponent>30313030303031</ns1:peerkeyexponent><ns1:serverdata>3930384541323142393130394334353539314131413031314246383441313839343044323245303332363031413142324444323335453237384139454631333134303445364230374637453242453842463441363538453243423244444532374530393335344237313237433841303544313042423432393838333746393635313843434234313234393742453031413241383936394639463436443233454244453734433942453632363846304536454438323039414437393732374243383530323734463637323541363743414239314143383730323245353837313034304246383536453534314137364242353743303746344239424534433633313636</ns1:serverdata><ns1:signature>181A4E679AC27D83543CECB8E1398243113EF6322D630923C6CD26860F265FC031C2C61D4F9D86046C07BBBF9CF86894903BD867E3CB59A0D9EFDADCB34A7FB3CC8BC7650B48E8913D327C38BB31E0EEB06E1FC1ACA2CFC52569BE8C48840627783D7FFC4A506B1D23A1C4AEAF12724DEB12B5036E0189E48A0FCB2832E1FB00</ns1:signature><ns1:timestamp>U3VuZGF5LCBPY3RvYmVyIDE4LCAyMDA5IDE6MTk6NTMgQU0=</ns1:timestamp></ns1:certificate><ns1:peerkeyprivate>8818DA2AC0E0956E0C67CA8D785CFAF3A11A9404D1ED9A6E580EA8569E087B75316B85D77B2208916BE2E0D37C7D7FD18EFD6B2E77C11CDA6E1B689BF460A40BBAF861D800497822004880024B4E7F98A020B1896F536D7219E67AB24B17D60A7BDD7D42E3501BB2FA50BB071EF7A80F29870FFD7C409C0B7BB7A8F70489D04D</ns1:peerkeyprivate></ns1:LoginUniqueNickResult></ns1:LoginUniqueNickResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        server.AddMockResponse("/AuthService/AuthService.asmx", "http://gamespy.net/AuthService/LoginUniqueNick", loginUniqueNickResponse);
        
        // Load and add mock responses for SakeStorageServer
        Console.WriteLine("Loading SakeStorageServer mock responses...");

        // SearchForRecords response for PersonalInfo
        string searchPersonalInfoResponse = @"<SOAP-ENV:Envelope xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ZSI=""http://www.zolera.com/schemas/ZSI/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body xmlns:ns1=""http://gamespy.net/sake""><ns1:SearchForRecordsResponse><ns1:SearchForRecordsResult>Success</ns1:SearchForRecordsResult><ns1:values><ns1:ArrayOfRecordValue><ns1:RecordValue><ns1:intValue><ns1:value>100000001</ns1:value></ns1:intValue></ns1:RecordValue><ns1:RecordValue><ns1:intValue><ns1:value>0</ns1:value></ns1:intValue></ns1:RecordValue><ns1:RecordValue><ns1:intValue><ns1:value>1</ns1:value></ns1:intValue></ns1:RecordValue><ns1:RecordValue><ns1:asciiStringValue><ns1:value>&lt;not available here yet&gt;</ns1:value></ns1:asciiStringValue></ns1:RecordValue><ns1:RecordValue><ns1:intValue><ns1:value>0</ns1:value></ns1:intValue></ns1:RecordValue><ns1:RecordValue><ns1:dateAndTimeValue><ns1:value>2018-01-01T00:00:00Z</ns1:value></ns1:dateAndTimeValue></ns1:RecordValue><ns1:RecordValue><ns1:dateAndTimeValue><ns1:value>DATE_NOW</ns1:value></ns1:dateAndTimeValue></ns1:RecordValue><ns1:RecordValue><ns1:byteValue><ns1:value>0</ns1:value></ns1:byteValue></ns1:RecordValue><ns1:RecordValue><ns1:asciiStringValue><ns1:value>mynick</ns1:value></ns1:asciiStringValue></ns1:RecordValue></ns1:ArrayOfRecordValue></ns1:values></ns1:SearchForRecordsResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        server.AddMockResponse("/SakeStorageServer/StorageServer.asmx", "http://gamespy.net/sake/SearchForRecords", searchPersonalInfoResponse);

        // Add a fallback response for any other requests to these endpoints
        string fallbackAuthResponse = @"<SOAP-ENV:Envelope xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ZSI=""http://www.zolera.com/schemas/ZSI/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body><fallback>This is a fallback response for AuthService</fallback></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        server.AddFallbackResponse("/AuthService/AuthService.asmx", fallbackAuthResponse);

        string fallbackSakeResponse = @"<SOAP-ENV:Envelope xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ZSI=""http://www.zolera.com/schemas/ZSI/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body><fallback>This is a fallback response for SakeStorageServer</fallback></SOAP-ENV:Body></SOAP-ENV:Envelope>";
        server.AddFallbackResponse("/SakeStorageServer/StorageServer.asmx", fallbackSakeResponse);
        
        // Start the server
        Console.WriteLine($"Mock server started at {baseUrl}");
        Console.WriteLine("Press Ctrl+C to stop the server");
        
        try
        {
            await server.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting server: {ex.Message}");
        }
    }
    

}