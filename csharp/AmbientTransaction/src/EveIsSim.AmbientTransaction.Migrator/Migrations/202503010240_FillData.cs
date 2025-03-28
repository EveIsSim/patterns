using FluentMigrator;

namespace EveIsSim.AmbientTransaction.Migrator.Migrations;

[Migration(202503010240)]
public class FillData : Migration
{
    public override void Up()
    {

        Execute.Sql(@"
                    INSERT INTO accounts (owner_name, balance)
                    VALUES 
                    ('Alice', 100.00),
                    ('Bob', 0.00);
                ");
    }


    public override void Down()
    {
    }
}
