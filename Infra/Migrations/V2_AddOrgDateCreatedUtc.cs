using FluentMigrator;

namespace Infra.Migrations
{
    [Migration(202505100001)]
    public class V2_AddOrgDateCreatedUtc_MySql : Migration
    {
        public override void Up()
        {
            Alter.Table("Orgs")
                 .AddColumn("OrgDateCreated")
                 .AsDateTime()
                 .NotNullable()
                 .WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        public override void Down()
        {
            Delete.Column("OrgDateCreated").FromTable("Orgs");
        }
    }
}
