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

        }

        public override void Down()
        {
            DropColumn("dbo.Quotes", "VolumeInCBM");
        }
    }
}
