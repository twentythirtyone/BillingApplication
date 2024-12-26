ENV_FILE = .env

ENV_VARS = \
    POSTGRES_DB=billingapp \
    POSTGRES_USER=billing \
    POSTGRES_PASSWORD=password \
    POSTGRES_HOST=billing-app-db \
    POSTGRES_PORT=5432 \
	PGBOUNCER_PORT=6432

env:
	@$(eval SHELL:=/bin/bash)
	@printf "%s\n" $(ENV_VARS) > $(ENV_FILE)
	@echo "$(ENV_FILE) file created"

run:
	@chmod +x database/scripts/entrypoint.sh
	@docker compose up --build -d

runl:
	@chmod +x database/scripts/entrypoint.sh
	@docker compose up --build

off:
	@docker compose down

db:
	@docker compose up --build -d db pgbouncer