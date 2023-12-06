package main

import (
	"github.com/gin-gonic/gin"
)

func CreateStorageUser(c *gin.Context) {

	model := GetRequestModelFromReader(c)

	password := GeneratePassword()

	// Create user in minio
	CreateMinioUserWithPolicy(model, password)

	// Update database
	InsertMinioUser(model.User, password, model.Policy)
}
