# NuGet restore
FROM mcr.microsoft.com/dotnet/sdk:5.0.402 AS build
WORKDIR /BGPViewerTool
COPY *.sln .
COPY BGPViewerCore/*.csproj BGPViewerCore/
COPY BGPViewerCore.Models/*.csproj BGPViewerCore.Models/
COPY BGPViewerCore.UnitTests/*.csproj BGPViewerCore.UnitTests/
COPY BGPViewerOpenApi/*.csproj BGPViewerOpenApi/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /BGPViewerTool/BGPViewerOpenApi
RUN dotnet build -o /BGPViewerTool/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0.11 AS runtime
WORKDIR /BGPViewerTool
COPY --from=publish /BGPViewerTool/publish .
# ENTRYPOINT ["dotnet", "BGPViewerOpenApi.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet BGPViewerOpenApi.dll

RUN apt-get update && \   
    apt-get install -y gnupg  libgconf-2-4 wget && \
    wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - && \
    sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' && \
    apt-get update && \
    apt-get install -y google-chrome-stable --no-install-recommends
