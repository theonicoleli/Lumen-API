using FluentMigrator;

namespace Infra.Migrations
{
    [Migration(20250601160000)]
    public class V6_RebuildSchema : Migration
    {
        private readonly string[] _oldTablesToDelete = new string[]
        {
            "Reports",
            "Donations",
            "Users",
            "Admins",
            "Donors",
            "Orgs"
        };

        public override void Up()
        {
            foreach (var tableName in _oldTablesToDelete)
                if (Schema.Table(tableName).Exists())
                    Delete.Table(tableName);

            Create.Table("Users")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserEmail").AsString(200).NotNullable().Unique()
                .WithColumn("UserPassword").AsString(100).NotNullable()
                .WithColumn("Role").AsString(50).NotNullable()
                .WithColumn("UserDateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.Table("Donors")
                .WithColumn("UserId").AsInt32().PrimaryKey()
                    .ForeignKey("FK_Donors_Users", "Users", "Id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Document").AsString(50).Nullable()
                .WithColumn("Phone").AsString(50).Nullable()
                .WithColumn("BirthDate").AsDate().NotNullable()
                .WithColumn("ImageUrl").AsString(2048).Nullable();

            Create.Table("Orgs")
                .WithColumn("UserId").AsInt32().PrimaryKey()
                    .ForeignKey("FK_Orgs_Users", "Users", "Id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("OrgName").AsString(255).NotNullable()
                .WithColumn("Phone").AsString(50).Nullable()
                .WithColumn("Document").AsString(50).Nullable()
                .WithColumn("Address").AsString(500).Nullable()
                .WithColumn("Description").AsString(1000).Nullable()
                .WithColumn("AdminName").AsString(255).Nullable()
                .WithColumn("ImageUrl").AsString(2048).Nullable()
                .WithColumn("OrgWebsiteUrl").AsString(2048).Nullable()
                .WithColumn("OrgFoundationDate").AsDate().NotNullable()
                .WithColumn("AdminPhone").AsString(50).Nullable();

            Create.Table("Projects")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Address").AsString(500).Nullable()
                .WithColumn("Description").AsString(1000).Nullable()
                .WithColumn("Image_Url").AsString(2048).Nullable()
                .WithColumn("OrgId").AsInt32().NotNullable()
                    .ForeignKey("FK_Projects_Orgs", "Orgs", "UserId").OnDelete(System.Data.Rule.Cascade);

            Create.Table("Donations")
                .WithColumn("DonationId").AsInt32().PrimaryKey().Identity()
                .WithColumn("DonationMethod").AsString(100).NotNullable()
                .WithColumn("DonationDate").AsDateTime().NotNullable()
                .WithColumn("DonationAmount").AsDecimal(18, 2).NotNullable()
                .WithColumn("Status").AsString(50).NotNullable()
                .WithColumn("DonationDonorMessage").AsString(1000).Nullable()
                .WithColumn("DonationIsAnonymous").AsBoolean().NotNullable()
                .WithColumn("DonorId").AsInt32().NotNullable()
                    .ForeignKey("FK_Donations_Donors", "Donors", "UserId").OnDelete(System.Data.Rule.None)
                .WithColumn("OrgId").AsInt32().NotNullable()
                    .ForeignKey("FK_Donations_Orgs", "Orgs", "UserId").OnDelete(System.Data.Rule.None);

            Create.Table("Reports")
                .WithColumn("ReportId").AsInt32().PrimaryKey().Identity()
                .WithColumn("ReportDate").AsDateTime().NotNullable()
                .WithColumn("ReportContent").AsString(2000).NotNullable()
                .WithColumn("OrgId").AsInt32().NotNullable()
                    .ForeignKey("FK_Reports_Orgs", "Orgs", "UserId").OnDelete(System.Data.Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("Reports");
            Delete.Table("Donations");
            Delete.Table("Projects");
            Delete.Table("Orgs");
            Delete.Table("Donors");
            Delete.Table("Users");
        }
    }
}