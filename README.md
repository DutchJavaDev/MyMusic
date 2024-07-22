# Discontinued google took down a third party API that I use :)
# A personal clone of Spotify .NET
To see if it can be created cheaper using a vps/pi/local-pc to handle downloading YouTube videos and converting those to mp3 files, those mp3 files can be streamed or downloaded to a mp3 player running in a web browser or on a phone. Using mongodb for storage and/or local on the device.

this is a proof of concept I am working on

[![Publish Docker image](https://github.com/DutchJavaDev/MyMusic/actions/workflows/docker-image.yml/badge.svg)](https://github.com/DutchJavaDev/MyMusic/actions/workflows/docker-image.yml)

- [ ] MVP
  - [x]   Working api
    - [x] Download search request
    - [x] Convert request to mp3
    - [x] Upload mp3 to s3 (minio) -> mongodb
    - [x] Update status   
  - [x]   Working s3 storage (minio) -> mongodb
    - [x] Write-only account
    - [x] Read-only account   
  - [ ]   Working minimal player
    - [x] Search music sources etc youtube etc
    - [x] Stream song from s3 storage (minio) -> api -> mongodb
    - [ ] Implement player UI
    - [ ] Working Playlist manager
- [x] Dockerize
  - [x] Compose
  - [ ] Extra step Kubernetes?  
- [ ] VPS Run
- [ ] Rasberry PI Run
- [ ] Local pc Run

