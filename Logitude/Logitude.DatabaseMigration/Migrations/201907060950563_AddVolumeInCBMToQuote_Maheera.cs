namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddVolumeInCBMToQuote_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "VolumeInCBM", c => c.Double());
            Sql("update Quotes set VolumeInCBM = round(Volume, 3) where Volume is not null and VolumeUnitCode = 'CBM'");
            Sql("update Quotes set VolumeInCBM = round(Volume / 61024, 3) where Volume is not null and VolumeUnitCode = 'CBI'");
            Sql("update Quotes set VolumeInCBM = round(Volume / 35.315, 3) where Volume is not null and VolumeUnitCode = 'CBF'");

        }

        public override void Down()
        {
            DropColumn("dbo.Quotes", "VolumeInCBM");
        }
    }
}
