using FluentMigrator;

namespace Infra.Migrations
{
    [Migration(202505100002)]
    public class V3_ChangeBirthAndFoundationToDate : Migration
    {
        public override void Up()
        {
            Alter.Table("Users")
                 .AlterColumn("BirthDate")
                 .AsDate()
                 .Nullable();

            Alter.Table("Orgs")
                 .AlterColumn("OrgFoundationDate")
                 .AsDate()
                 .NotNullable();
        }

        public override void Down()
        {
            Alter.Table("Users")
                 .AlterColumn("BirthDate")
                 .AsDateTime()
                 .Nullable();

            Alter.Table("Orgs")
                 .AlterColumn("OrgFoundationDate")
                 .AsDateTime()
                 .NotNullable();
        }
    }
}
