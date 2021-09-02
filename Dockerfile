FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS dream

COPY ./DarkRiftServer .
COPY /MultiplayerPlugin/bin/Debug/MultiplayerPlugin.dll ./Plugins/

ENTRYPOINT ["dotnet", "./Lib/DarkRift.Server.Console.dll"]

EXPOSE 4296