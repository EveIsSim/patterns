.PHONY: up_db run_migration up_app all stop


up_db:
	docker-compose up -d

run_migration:
	dotnet run --project ./src/EveIsSim.AmbientTransaction.Migrator/EveIsSim.AmbientTransaction.Migrator.csproj


up_app:
	dotnet run --project ./src/EveIsSim.AmbientTransaction.App/EveIsSim.AmbientTransaction.App.csproj

all: up_db run_migration up_app

stop:
	docker-compose down

test:
	dotnet test
