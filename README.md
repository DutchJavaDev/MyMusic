# A personal clone of Spotify .NET
To see if it can be created cheaper using a vps/pi/local-pc to handle downloading YouTube videos and converting those to mp3 files, those mp3 files can be streamed or downloaded to a mp3 player running in a web browser or on a phone. Using minio as s3 type storage for the files.

- [ ] MVP
  - [x]   Working api
    - [x] Download search request
    - [x] Convert request to mp3
    - [x] Upload mp3 to s3 (minio) -> mongodb
    - [x] Update status   
  - [x]   Working s3 storage (minio) -> mongodb
    - [x] Write-only account
    - [x] Read-only account   
  - [ ]   Working player
    - [x] Search music sources etc youtube etc
    - [ ] Stream song from s3 storage (minio) -> api -> mongodb
- [x] Dockerize
  - [x] Compose
  - [ ] Extra step Kubernetes?  
- [ ] VPS Run
- [ ] Rasberry PI Run
- [ ] Local pc Run

