.PHONY: help build up down restart logs clean test

help: ## Show this help message
	@echo 'Usage: make [target]'
	@echo ''
	@echo 'Available targets:'
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  %-15s %s\n", $$1, $$2}' $(MAKEFILE_LIST)

build: ## Build Docker images
	docker-compose build

up: ## Start all services
	docker-compose up -d

down: ## Stop all services
	docker-compose down

restart: ## Restart all services
	docker-compose restart

logs: ## View logs
	docker-compose logs -f

logs-api: ## View API logs only
	docker-compose logs -f api

logs-db: ## View PostgreSQL logs only
	docker-compose logs -f postgres

clean: ## Remove all containers, volumes, and images
	docker-compose down -v --rmi all

ps: ## List running containers
	docker-compose ps

shell-api: ## Open shell in API container
	docker-compose exec api /bin/bash

shell-db: ## Open PostgreSQL shell
	docker-compose exec postgres psql -U postgres -d cqrs_decorator_db

rebuild: ## Rebuild and restart all services
	docker-compose down
	docker-compose build --no-cache
	docker-compose up -d

prod-up: ## Start in production mode
	docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

prod-down: ## Stop production services
	docker-compose -f docker-compose.yml -f docker-compose.prod.yml down

test: ## Run tests in container
	docker-compose exec api dotnet test

migrate: ## Run database migrations
	docker-compose exec api dotnet ef database update --project /src/CQRS-Decorator.Infrastructure --startup-project /src/CQRS-Decorator.API
