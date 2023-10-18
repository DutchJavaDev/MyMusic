# About
A personal clone of Spotify to see if it can be created cheaper using my own server to handle downloading YouTube videos and converting those to mp3 files, those mp3 files can be streamed or downloaded to a mp3 player running in a web browser or on a phone.

[Architecture](https://drive.google.com/file/d/1piXXR4OJywxrwHlFv3x89VZuT_wm_QbU/view?usp=sharing)

# Docker Compose
Combine web frontend with backend, database and object storage.
For mobile only deploy backend, database and object storage

# MyMusic Frontend (Latest Flutter version)
Main use is to play music by streaming or locally by downloading, create playlist to group a collection of songs also manage the playlist by able to update and delete the playlist.

### Pages

#### Configuration
Page to setup the server URL and input the password for the server, YouTube Data API
API key. Stores the password after success full login to local storage.

#### Main
List of all my current songs, search component

#### Playlist
Page to manage playlists

#### MyMusic
Listen to music :)
### Components
#### Search
Search videos to convert by using the YouTube Data API->backend
Input field that will show a modal with the results.

### Services
#### Download
Downloads server files to local only for mobile, stream only for web!

# MyMusic Backend (.NET 6 Web API)
The server will have a API that can be called by the frontends, the API will handle downloading videos from YouTube and converting those to mp3 files that will be stored in Object Storage.
Data will be stored in a database thins like music, playlist etc. 

# Download YouTube playlist into actual playlist by converting all videos to mp3 and creating playlist?

### Authentication
Server will generate an initial password stored in the database, onetime login.
password will be stored locally on the device. (Configurable)

### Password Middleware
Checks if the password is present in the header if not then cancel the request. (401)

### Endpoints

#### Mp3Pipeline
Used to download a video, then convert and store it in storage
#### Playlist
Used to manage playlist
#### Music
Used for streaming music
#### Download
Used for downloading music to device
#### Stream
Used for streaming music to device
### Streaming & Downloading
https://stackoverflow.com/questions/76523932/downloading-a-file-from-minio-s3-storage-in-asp-net

### Storage
MINIO to store files, access with service wrapper

### Database 
Postgres database to store
server password
server password hash
music information
playlist information
downloads information
