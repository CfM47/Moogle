# Moogle!

Moogle is a search engine project developed as part of the Programming course of the first year of Computer Science at the Faculty of Mathematics and Computing, University of Havana.

## How to run the project without Docker

1. Install .NET Core 6.0. You can check the [official documentation](https://learn.microsoft.com/en-us/dotnet/core/install/) for this.
2. Clone the project and move to the root folder:

```bash
git clone https://github.com/CfM47/Moogle.git
cd Moogle
```

3. If you are on Linux or Windows, you can run the respective commands:
```bash
make dev
```
```bash
dotnet watch run --project MoogleServer
```

## Run the project using Docker

To run the project using Docker, follow these steps:

1. Make sure you have Docker installed on your system. You can follow the instructions in the [official Docker documentation](https://docs.docker.com/get-docker/).

2. Clone the project and move to the root folder:

```bash
git clone https://github.com/CfM47/Moogle.git
cd Moogle
```

3. Build the Docker image:

```bash
docker build -t moogle .
```

4. Run the Docker container:

```bash
docker run --rm -d -p 5285:5285 --name moogleserver_container moogleserver:latest
```

This will start the server on port 5000. You can access the application in your browser at `http://localhost:5285`.

## Customizing the Content

Moogle is a search engine wich mean that it needs something to search. To perform queries, be sure to copy plain text documents into the Content folder. This folder will be used by the application to search and retrieve information.

---

>Programming Project. Faculty of Mathematics and Computing - University of Havana. Courses 2021, 2022.