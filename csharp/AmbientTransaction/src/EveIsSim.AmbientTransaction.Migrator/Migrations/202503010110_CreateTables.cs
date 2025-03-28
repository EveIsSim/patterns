using FluentMigrator;

namespace EveIsSim.AmbientTransaction.Migrator.Models;

[Migration(202503010110)]
public class CreateTables : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE IF NOT EXISTS accounts (
                id              SERIAL              PRIMARY KEY,
                owner_name      VARCHAR(100)        NOT NULL,
                balance         DECIMAL(18,2)       NOT NULL
            );
        ");

        Execute.Sql(@"
            CREATE TABLE IF NOT EXISTS transaction_logs (
                id                      SERIAL PRIMARY KEY,
                source_account_id       INT                     NOT NULL,
                destination_account_id  INT                     NOT NULL,
                amount                  DECIMAL(18,2)           NOT NULL,
                timestamp               TIMESTAMP               NOT NULL
            );
        ");

    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS accounts");
        Execute.Sql("DROP TABLE IF EXISTS transaction_logs");
    }
}
