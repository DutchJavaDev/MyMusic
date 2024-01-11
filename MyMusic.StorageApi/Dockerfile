# FROM golang:latest

# WORKDIR /usr/src/app

# # pre-copy/cache go.mod for pre-downloading dependencies and only redownloading them in subsequent builds if they change
# COPY go.mod go.sum ./
# RUN go mod download && go mod verify

# COPY . .
# RUN go build -v -o /usr/local/bin/app ./...

# EXPOSE 8080
# CMD ["app"]


#############

FROM golang:latest as base
WORKDIR /builder

ENV GO111MODULE=on CGO_ENABLED=0
COPY go.mod go.sum /builder/
RUN go mod download && go mod verify

# runner image
FROM gcr.io/distroless/static:latest
WORKDIR /usr/src/app
COPY --from=base /builder/ /usr/src/app

EXPOSE 8080
CMD ["app"]