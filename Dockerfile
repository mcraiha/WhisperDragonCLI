FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.15-amd64 AS build
WORKDIR /source

COPY src/. .
RUN dotnet publish -c release -r linux-musl-x64 --self-contained -o /app -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true

FROM alpine:3.15
RUN apk upgrade --no-cache && apk add --no-cache libgcc libstdc++ icu-libs ncurses
WORKDIR /app
COPY --from=build /app ./