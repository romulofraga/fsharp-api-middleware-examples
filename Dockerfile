FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV LANG=en_US.UTF-8
ENV ASPNETCORE_URLS=http://*:80


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["fsharpApi.fsproj", "./"]
RUN dotnet restore "fsharpApi.fsproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "fsharpApi.fsproj" -c Release -o /app/build

FROM build AS publish
ARG VERSION
RUN dotnet publish "fsharpApi.fsproj" -c Release -o /app/publish /p:Version=$VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fsharpApi.dll"]
