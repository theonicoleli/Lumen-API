using FluentMigrator;

namespace Infra.Migrations
{
    [Migration(202503280001)]
    public class V1_CreateTables : Migration
    {
        public override void Up()
        {
            Create.Table("Admins")
                .WithColumn("AdminId").AsInt32().PrimaryKey().Identity()
                .WithColumn("AdminRole").AsString(100)
                .WithColumn("AdminEmail").AsString(200)
                .WithColumn("AdminPassword").AsString(200)
                .WithColumn("AdminContent").AsString(500);

            Create.Table("Donors")
                .WithColumn("DonorId").AsInt32().PrimaryKey().Identity()
                .WithColumn("DonorDocument").AsString(200)
                .WithColumn("DonorLocation").AsString(200);

            Create.Table("Users")
                .WithColumn("UserId").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserEmail").AsString(200)
                .WithColumn("UserPassword").AsString(200)
                .WithColumn("UserStatus").AsString(50)
                .WithColumn("UserImageUrl").AsString(500)
                .WithColumn("DonorId").AsInt32().Nullable()
                    .ForeignKey("FK_Users_Donors", "Donors", "DonorId")
                .WithColumn("BirthDate").AsDateTime().Nullable()
                .WithColumn("Phone").AsString(50).Nullable();

            Create.Table("Orgs")
                .WithColumn("OrgId").AsInt32().PrimaryKey().Identity()
                .WithColumn("OrgDescription").AsString(500)
                .WithColumn("OrgWebsiteUrl").AsString(200)
                .WithColumn("OrgLocation").AsString(200)
                .WithColumn("OrgFoundationDate").AsDateTime()
                .WithColumn("AdminName").AsString(200)
                .WithColumn("AdminPhone").AsString(50);

            Create.Table("Donations")
                .WithColumn("DonationId").AsInt32().PrimaryKey().Identity()
                .WithColumn("DonationMethod").AsString(100)
                .WithColumn("DonationDate").AsDateTime()
                .WithColumn("DonationAmount").AsDecimal(18, 2)
                .WithColumn("DonationStatus").AsString(100)
                .WithColumn("DonationIsAnonymous").AsBoolean()
                .WithColumn("DonationDonorMessage").AsString(500)
                .WithColumn("DonorId").AsInt32().ForeignKey("FK_Donations_Donors", "Donors", "DonorId")
                .WithColumn("OrgId").AsInt32().ForeignKey("FK_Donations_Orgs", "Orgs", "OrgId");

            Create.Table("Reports")
                .WithColumn("ReportId").AsInt32().PrimaryKey().Identity()
                .WithColumn("ReportDate").AsDateTime()
                .WithColumn("ReportContent").AsString(1000);
        }

        public override void Down()
        {
            Delete.Table("Reports");
            Delete.Table("Donations");
            Delete.Table("Orgs");
            Delete.Table("Users");
            Delete.Table("Donors");
            Delete.Table("Admins");
        }
    }
}
