.PHONY: build
build:
	dotnet build

.PHONY: dev
dev:
	dotnet watch run --urls http://0.0.0.0:5285 --project MoogleServer
