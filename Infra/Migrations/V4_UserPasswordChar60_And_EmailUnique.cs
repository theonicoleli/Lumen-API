using FluentMigrator;

namespace Infra.Migrations
{
    [Migration(202505100003)]
    public class V4_UserPasswordChar60_And_EmailUnique : Migration
    {
        public override void Up()
        {
            Alter.Table("Users")
                 .AlterColumn("UserPassword")
                 .AsFixedLengthString(60)
                 .NotNullable();

            if (!Schema.Table("Users").Index("UQ_Users_UserEmail").Exists())
            {
                Create.Index("UQ_Users_UserEmail")
                      .OnTable("Users")
                      .OnColumn("UserEmail").Ascending()
                      .WithOptions().Unique();
            }
        }

        public override void Down()
        {
            Alter.Table("Users")
                 .AlterColumn("UserPassword")
                 .AsString(200)
                 .NotNullable();

            Delete.Index("UQ_Users_UserEmail").OnTable("Users");
        }
    }
}
