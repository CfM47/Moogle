FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

RUN apt-get update && apt-get install -y make

COPY . .

EXPOSE 5285

CMD ["make", "dev"]