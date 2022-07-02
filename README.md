### running locally
core git repo in ./DarkRift -> this is at git@github.com:DarkRiftNetworking/DarkRift.git
check git repo to go see changes etc... but technically all we need from here are the end core .dll files...
*we need to have these files built after a git pull*
```
// rebuild MultiplayerPlugin for deploys
cd MultiplayerPlugin
dotnet build .
// git push these changes up and on redeploy server will pull the updated MultiplayerPlugin.dll
```
- run `docker-compose up` and server should startup locally

### unity client
load UnityClient project via UnityHub and run

### info.docker
handles basically grabbing all the dll files we need and running dotnet to spin server up
(check Dockerfile)

### info.dev
we'l probably have to push the built files to server so that we dont need to
manually build on server deploy

### misc docker
keep docker container running
```
ENTRYPOINT ["tail", "-f", "/dev/null"]
```

### server deployment
login to do.sheep-network
```
cd /home/dev/icarus71
git pull
./docker.sh
```