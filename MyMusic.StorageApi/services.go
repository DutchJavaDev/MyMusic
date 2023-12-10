package main

import (
	"context"
	"fmt"
	"os"

	"github.com/jackc/pgx/v5"
	"github.com/minio/madmin-go/v3"
)

func CreateMinioClient() (*madmin.AdminClient, error) {
	endpoint := os.Getenv("storageapi_endpoint")
	accessKeyID := os.Getenv("storageapi_user")
	secretAccessKey := os.Getenv("storageapi_password")
	fmt.Println(endpoint, accessKeyID, secretAccessKey)
	useSSL := false
	return madmin.New(endpoint, accessKeyID, secretAccessKey, useSSL)
}

func CreateDatabaseClient() (*pgx.Conn, error) {
	databaseUrl := os.Getenv("storageapi_database_url")
	return pgx.Connect(context.Background(), databaseUrl)
}
