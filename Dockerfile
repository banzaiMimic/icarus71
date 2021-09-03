FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS dream

COPY ./DarkRiftServer .
COPY /MultiplayerPlugin/bin/Debug/MultiplayerPlugin.dll ./Plugins/

EXPOSE 4296/udp
EXPOSE 4296/tcp

ENTRYPOINT ["dotnet", "./Lib/DarkRift.Server.Console.dll"]
