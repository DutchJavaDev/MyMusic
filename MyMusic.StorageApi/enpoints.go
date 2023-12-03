package main

import (
	"context"
	"fmt"
	"net/http"
	"os"

	"github.com/gin-gonic/gin"
)

func ShowEnviromentVariables(c *gin.Context) {
	env := os.Environ()
	c.HTML(http.StatusOK, "env.html", gin.H{
		"env": env,
	})
}

func CreateStorageUser(c *gin.Context) {

	model := GetRequestModelFromReader(c.Request.Body)

	mdmClnt, err := CreateMinioClient()

	if err != nil {
		fmt.Println(err)
		return
	}

	// Add user
	if err = mdmClnt.AddUser(context.Background(), model.User, "Generate-super-secrete-password"); err != nil {
		// Failed
		fmt.Println(err)
		return
	}

	// Set policy
	if err = mdmClnt.SetPolicy(context.Background(), model.Policy, model.User, false); err != nil {
		// Failed
		fmt.Println(err)
	}

	// Update database
	db, err := CreateDatabaseClient()

	if err != nil {
		fmt.Fprintf(os.Stderr, "Unable to connect to database: %v\n", err)
		os.Exit(1)
	}
	defer db.Close(context.Background())
}

// func minioTest(c *gin.Context) {
// 	// Initialize minio client object.
// 	minioClient, err := CreateMinioClient()

// 	if err != nil {
// 		log.Fatalln(err)
// 	}

// 	log.Printf("%#v\n", minioClient) // minioClient is now set up

// 	object, err := minioClient.GetObject(context.Background(), "images", "background.png", minio.GetObjectOptions{})
// 	if err != nil {
// 		fmt.Println(err)
// 		return
// 	}
// 	defer object.Close()
// 	buf := new(bytes.Buffer)
// 	buf.ReadFrom(object)

// 	if err != nil {
// 		fmt.Println(err)
// 		return
// 	}

// 	sEnc := b64.StdEncoding.EncodeToString(buf.Bytes())

// 	imageSource := "data:image/png;base64," + sEnc

// 	c.HTML(http.StatusOK, "index.html", gin.H{
// 		"src": imageSource,
// 	})
// }
