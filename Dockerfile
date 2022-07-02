FROM mcr.microsoft.com/dotnet/aspnet:6.0.0 AS dream

COPY ./DR-Lib ./Lib
COPY ./Server.config ./
COPY /MultiplayerPlugin/bin/Debug/MultiplayerPlugin.dll ./Plugins/

EXPOSE 4296/udp
EXPOSE 4296/tcp

ENTRYPOINT ["dotnet", "./Lib/DarkRift.Server.Console.dll"]
