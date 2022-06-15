FROM mcr.microsoft.com/dotnet/aspnet:6.0.0 AS dream

COPY ./DarkRift/Build/Debug/net6.0/Lib ./Lib
COPY ./DarkRift/Build/Debug/net6.0/Server.config ./
COPY /MultiplayerPlugin/bin/Debug/MultiplayerPlugin.dll ./Plugins/

EXPOSE 4296/udp
EXPOSE 4296/tcp

ENTRYPOINT ["dotnet", "./Lib/DarkRift.Server.Console.dll"]
