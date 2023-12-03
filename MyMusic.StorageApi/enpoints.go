package main

import (
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
