### running locally
core git repo in ./DarkRift -> this is at git@github.com:DarkRiftNetworking/DarkRift.git
check git repo to go see changes etc... but technically all we need from here are the end core .dll files...
*we need to have these files built after a git pull*
```
cd DarkRift
dotnet build .
```
don't worry about the `2 Warning(s) 5 Error(s)` as the build still compiles
- run docker-compose and server should startup locally
`docker-compose up`

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