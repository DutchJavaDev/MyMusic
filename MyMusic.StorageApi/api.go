package main

import (
	"github.com/gin-gonic/gin"
)

func main() {

	r := gin.Default()
	r.LoadHTMLGlob("htmlTemplates/*")

	//r.GET("/mit", minioTest)

	r.GET("/sev", ShowEnviromentVariables)

	r.POST("/csu", CreateStorageUser)

	r.Run()
}
