package main

import (
	"github.com/gin-gonic/gin"
)

func CreateStorageUser(c *gin.Context) {

	model := GetRequestModelFromReader(c)

	password := GeneratePassword()

	// Create user in minio
	created, error := CreateMinioUserWithPolicy(model, password)

	if error != nil {
		Error(error)
	}

	if created {
		// Update database
		InsertMinioUser(model.User, password, model.Policy)
	}
}
