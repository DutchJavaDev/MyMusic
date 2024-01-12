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

FROM golang:latest as builder
WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY . .

## Enable static executable
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o /app/api .

# runner image
FROM scratch

WORKDIR /usr/src/
COPY --from=builder /app .

EXPOSE 8080
CMD ["./api"]