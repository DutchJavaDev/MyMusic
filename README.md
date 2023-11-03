A personal clone of Spotify to see if it can be created cheaper using your own server to handle converting videos to mp3 files, mp3 files can be streamed or downloaded to a mp3 player running in a web browser or on a phone.

[Architecture](https://drive.google.com/file/d/1piXXR4OJywxrwHlFv3x89VZuT_wm_QbU/view?usp=sharing)

LRC File :)

# MyMusic Frontend (.NET MAUI)
Search, download and play music from here.
[UI (Figma)](https://www.figma.com/file/VkXHrNW3PIS2ou2nRswQA4/DarkUI%2FMusicPlayer-(Community)?type=design&mode=design&t=1GwHGTyWHWvkbKNF-1)

### Nav
Option to create a playlist
Show all playlist
### Pages/Popups

#### Configuration
Page to setup the server URL and input the password for the server, YouTube Data API
API key. Stores the password after success full login to local storage.

#### Home
Search for songs here, download a song after searching for it

#### Songs
Show all songs

#### Cloud download
Show progress of all songs being downloading

### Local download
Show a list of all songs locally on the device

### Components
#### Search
Search videos to convert by using the YouTube Data API->backend
Input field that will show a modal with the results.

### Services
#### Search
Search videos to be converted to mp3
#### Server API
Wrapper to make request to the server
### Playlist service
Manage user created playlists


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

#### Download
Used to download a video, then convert and store it in storage
A three step process

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