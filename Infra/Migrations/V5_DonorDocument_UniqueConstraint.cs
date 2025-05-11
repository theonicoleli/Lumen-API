using FluentMigrator;

namespace Infra.Migrations
{
    [Migration(202505110001)]
    public class V5_DonorDocument_UniqueConstraint : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                UPDATE Donors 
                   SET DonorDocument = CONCAT('TMP', DonorId) 
                 WHERE DonorDocument = ''");

            if (!Schema.Table("Donors").Index("UQ_Donors_DonorDocument").Exists())
            {
                Create.Index("UQ_Donors_DonorDocument")
                      .OnTable("Donors")
                      .OnColumn("DonorDocument").Ascending()
                      .WithOptions().Unique();
            }
        }

        public override void Down()
        {
            if (Schema.Table("Donors").Index("UQ_Donors_DonorDocument").Exists())
            {
                Delete.Index("UQ_Donors_DonorDocument").OnTable("Donors");
            }
        }
    }
}
