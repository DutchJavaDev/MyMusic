package main

import (
	"bytes"
	"context"
	b64 "encoding/base64"
	"fmt"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/minio/minio-go/v7"
	"github.com/minio/minio-go/v7/pkg/credentials"
)

func main() {

	endpoint := ""
	accessKeyID := ""
	secretAccessKey := ""
	useSSL := false

	// Initialize minio client object.
	minioClient, err := minio.New(endpoint, &minio.Options{
		Creds:  credentials.NewStaticV4(accessKeyID, secretAccessKey, ""),
		Secure: useSSL,
	})
	if err != nil {
		log.Fatalln(err)
	}

	log.Printf("%#v\n", minioClient) // minioClient is now set up

	r := gin.Default()
	r.LoadHTMLGlob("htmlTemplates/*")

	r.GET("/env", ShowEnviromentVariables)

	r.GET("/ping", func(c *gin.Context) {
		object, err := minioClient.GetObject(context.Background(), "images", "background.png", minio.GetObjectOptions{})
		if err != nil {
			fmt.Println(err)
			return
		}
		defer object.Close()
		buf := new(bytes.Buffer)
		buf.ReadFrom(object)

		if err != nil {
			fmt.Println(err)
			return
		}

		sEnc := b64.StdEncoding.EncodeToString(buf.Bytes())

		imageSource := "data:image/png;base64," + sEnc

		c.HTML(http.StatusOK, "index.html", gin.H{
			"src": imageSource,
		})
	})
	r.Run()
}
