# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MyMusic.Api/MyMusic.Api.csproj", "MyMusic.Api/"]
COPY ["MyMusic.Common/MyMusic.Common.csproj", "MyMusic.Common/"]
COPY ["MyMusic.Data/MyMusic.Data.csproj", "MyMusic.Data/"]
RUN dotnet restore "./MyMusic.Api/MyMusic.Api.csproj"
COPY . .
WORKDIR "/src/MyMusic.Api"
RUN dotnet build "./MyMusic.Api.csproj" -c Release -o /app/build

# Stage 2: Publish stage
FROM build AS publish
RUN dotnet publish "./MyMusic.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install ffmpeg under the mediatoolkit folder
RUN mkdir -p /app/MediaToolkit

# Install required dependencies including xz-utils
RUN apt-get update
RUN apt-get install -y wget xz-utils
RUN apt-get clean
RUN rm -rf /var/lib/apt/lists/*

# Download ffmpeg and extract it to the root folder
RUN apt-get update
RUN apt-get install -y wget
RUN wget https://johnvansickle.com/ffmpeg/releases/ffmpeg-release-amd64-static.tar.xz
RUN tar xf ffmpeg-release-amd64-static.tar.xz --strip-components=1
RUN rm ffmpeg-release-amd64-static.tar.xz
RUN mv ffmpeg ffmpeg.exe

# Copy published files from the previous stage
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080
EXPOSE 8081

# Run the app
ENTRYPOINT ["dotnet", "MyMusic.Api.dll"]