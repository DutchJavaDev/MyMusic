package main

import (
	"fmt"

	"github.com/gin-gonic/gin"
)

func CreateStorageUser(c *gin.Context) {

	model := GetRequestModelFromReader(c)

	password := GeneratePassword()

	// Create user in minio
	created, error := CreateMinioUserWithPolicy(model, password)

	if error != nil {
		fmt.Println(error)
	}

	if created {
		// Update database
		InsertMinioUser(model.User, password, model.Policy)
	}
}
