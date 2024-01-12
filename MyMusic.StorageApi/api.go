package main

import (
	"github.com/gin-gonic/gin"
)

func main() {

	r := gin.Default()

	// r.LoadHTMLGlob("htmlTemplates/*")

	r.POST("/csu", CreateStorageUser)

	r.Run()
}
