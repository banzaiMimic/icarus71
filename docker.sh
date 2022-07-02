docker rm sheep
docker build -t dev:1.0 .
docker run --name=sheep --restart=always -dt -p 4296:4296/tcp -p 4296:4296/udp dev:1.0
