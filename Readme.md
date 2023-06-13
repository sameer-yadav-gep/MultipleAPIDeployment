## [POC] Running multiple containerized Web APIs within a single K8s pod

---

_This guide was created for .NET Core Web APIs running on locally installed Docker Desktop._



- ### Step 1 : If you are using Docker Desktop, add DNS settings in Docker Engine configuration

```
"dns": [
    "1.1.1.1",
    "8.8.8.8"]
```
> This will take care of the Nuget service index error that might occur during execution of **dotnet restore** command while building the Dockerfile.

- ### Step 2: Create a Dockerfile for your applications. 

The default Dockerfile created automatically by Visual Studio can be used with a few modifications. Since the Base image for aspnetcore container listens on port 80 by default, it creates a socket conflict if we are going to deploy 2 or more apps which use this default port.

This can be resolved by explicitly specifying the ASPNETCORE_URLS environment variable and exposing that port, as shown here: 

```
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_URLS=http://+:5001
WORKDIR /app
EXPOSE 5001
...
...
```


_Note that each API must specify and expose a different URL and port._


- ### Step 3: Build docker images of your Web API projects

Give a unique and valid \<image-name>:\<tag> for each of the docker images.
``` 
docker build -t weatherapitest:dev .
```


- ### Step 4: Create a `deployment.yaml` file for the combined deployment

```
apiVersion: apps/v1
kind: Deployment
metadata:
  name: multiple-api-deployment
spec:
  replicas: 1
  selector:
    matchLabels:
      app: multiple-api-pod
  template:
    metadata:
      labels:
        app: multiple-api-pod
    spec:
      containers:
      - name: student-api-poc
        image: studentapitest:dev
        ports:
        - containerPort: 5000
      - name: weather-api-poc
        image: weatherapitest:dev
        ports:
        - containerPort: 5001

```

- ### Step 5: Deploy on the locally running kubernetes cluster

```
kubectl apply -f deployment.yaml
```

This will create a pod with the 2 containers defined in the above yaml file. The apis can be accessed from within the container using the curl command :  ` curl http://localhost:5001/WeatherForecast `.

Port fowarding can be used to access the deployed apps outside of the containers on localhost.
Syntax : ` kubectl port-forward <pod-name> <external-port>:<container-port> `