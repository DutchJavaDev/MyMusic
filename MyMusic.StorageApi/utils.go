package main

import (
	"context"
	"fmt"
	"os"

	"github.com/gin-gonic/gin"
	"github.com/sethvargo/go-password/password"
)

const PASSWORD_LENGHT = 12
const MAX_DIGITS = 4
const MAX_SYMBOLS = 0
const NO_UPPERCASE = false
const REPEAT = false

func GetRequestModelFromReader(c *gin.Context) CreateStorageUserModel {
	var model CreateStorageUserModel
	c.BindJSON(&model)
	return model
}

func GeneratePassword() string {
	res, err := password.Generate(PASSWORD_LENGHT, MAX_DIGITS, MAX_SYMBOLS, NO_UPPERCASE, REPEAT)

	if err != nil {
		Error(err)
	}

	return res
}

func Error(e error) {
	db, err := CreateDatabaseClient()

	if err != nil {
		fmt.Fprintf(os.Stderr, "Unable to connect to database: %v\n", err)
		fmt.Println("System exit.")
		os.Exit(1)
	}
	defer db.Close(context.Background())

	query := fmt.Sprintf("insert into mymusic.exception (message,app,stacktrace) values('%s','%s','%s')", "Oooops", "minio-auth", e.Error())
	fmt.Println(query)
	rows, err := db.Query(context.Background(), query)

	if err != nil {
		fmt.Println(err.Error())
	}

	rows.Close()

	fmt.Println(e)
}

func InsertMinioUser(name string, password string, policy string) {
	db, err := CreateDatabaseClient()

	if err != nil {
		fmt.Fprintf(os.Stderr, "Unable to connect to database: %v\n", err)
		os.Exit(1)
	}
	defer db.Close(context.Background())

	// Insert user

	// Did this without learning go, this can be done beter i know
	query := fmt.Sprintf("insert into mymusic.minio_users (name,password,policy) values('%s','%s','%s')", name, password, policy)
	fmt.Println(query)
	rows, err := db.Query(context.Background(), query)

	if err != nil {
		Error(err)
	}

	fmt.Println(rows)
}

// / Does not set permisson to list objects in webview
func CreateMinioUserWithPolicy(model CreateStorageUserModel, password string) (bool, error) {
	mdmClnt, err := CreateMinioClient()

	if err != nil {
		return false, err
	}

	// Add user
	if err = mdmClnt.AddUser(context.Background(), model.User, password); err != nil {
		// Failed
		return false, err
	}

	// Set policy
	if err = mdmClnt.SetPolicy(context.Background(), model.Policy, model.User, false); err != nil {
		// Failed
		return false, err
	}

	return true, err
}
