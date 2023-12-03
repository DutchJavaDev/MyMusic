package main

import (
	"encoding/json"
	"fmt"
	"io"
)

func GetRequestModelFromReader(r io.ReadCloser) CreateStorageUserModel {
	var model CreateStorageUserModel
	err := json.Unmarshal([]byte(r.Close().Error()), &model)

	if err != nil {
		fmt.Println(err)
	}

	return model
}
