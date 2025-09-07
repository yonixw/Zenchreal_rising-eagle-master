FROM mono:latest as builder

COPY linux-sqlite-sources/sqlite-netFx-source-1.0.119.0.zip /usr/src/

RUN sed -i s/deb.debian.org/archive.debian.org/g /etc/apt/sources.list && \
    sed -i s/security.debian.org/archive.debian.org/g /etc/apt/sources.list


RUN apt_packages=" \
        build-essential \
        unzip \
        wget \
    "; \
    apt-get update && apt-get install -y --no-install-recommends $apt_packages 
    
RUN mkdir /usr/src/sqlite-interop \
    && unzip /usr/src/sqlite-netFx-source-1.0.119.0.zip -d /usr/src/sqlite-interop/ 
    
RUN apt install libz-dev
    
RUN cd /usr/src/sqlite-interop/Setup/ \
    && chmod +x compile-interop-assembly-debug.sh \
    && ./compile-interop-assembly-debug.sh \
    && cp ../bin/2013/Debug/bin/libSQLite.Interop.so /usr/lib/ 
    
# Replace the one from the nuget package... Not sure why, same version! 
#   But a must! o.w. System.EntryPointNotFoundException
COPY linux-sqlite-sources/sqlite-netStandard20-binary-1.0.119.0.zip /usr/src/
RUN unzip /usr/src/sqlite-netStandard20-binary-1.0.119.0.zip -d /usr/lib/dlls/ 

FROM mono:latest

WORKDIR /src/app

VOLUME [ "/data" ]

# ENV LD_DEBUG=libs

EXPOSE 27900/udp 28910/tcp 29900/tcp 29901/tcp 29910/udp

CMD ["mono", "/src/app/PRMasterServer.exe", "+db", "/data/LoginDatabase.db3"]

RUN useradd -ms /bin/bash master_server && \
    mkdir -p /data && \
    chown -R master_server:master_server /data
USER master_server

COPY bin/Debug .

COPY --from=builder /usr/lib/libSQLite.Interop.so .
COPY --from=builder /usr/lib/dlls/ .

