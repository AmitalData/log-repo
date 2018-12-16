namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameAWBMessagingStockTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AWBMessagingStocks", newName: "MessagingStocks");
            RenameTable(name: "dbo.AWBStockUsageHistories", newName: "MessagingStockUsageHistories");

            Sql("update DBIdCounters set TableName = 'MessagingStock' where TableName = 'AWBMessagingStock'");
            Sql("update DBIdCounters set TableName = 'MessagingStockUsageHistory' where TableName = 'AWBStockUsageHistory'");

            Sql("update ObjectTables set Name = 'MessagingStock' where Name = 'AWBMessagingStock'");
            Sql("update ObjectTables set Name = 'MessagingStockUsageHistory' where Name = 'AWBStockUsageHistory'");

            Sql("update TextCodes set Code = REPLACE(Code, 'AWBMessagingStock', 'MessagingStock') where ObjectTableId in (select Id from ObjectTables where Name in ('MessagingStock'))");
            Sql("update TextCodes set Code = REPLACE(Code, 'AWBStockUsageHistory', 'MessagingStockUsageHistory') where ObjectTableId in (select Id from ObjectTables where Name in ('MessagingStockUsageHistory'))");

            Sql("update Features set Code = REPLACE(Code, 'AWBMessagingStock', 'MessagingStock') where ObjectTableId in (select Id from ObjectTables where Name in ('MessagingStock'))");
            Sql("update Queries set Code = REPLACE(Code, 'All AWB Messaging Stocks', 'All Messaging Stocks') where ObjectTableId in (select Id from ObjectTables where Name in ('MessagingStock'))");

            Sql("update MenuButtonGroups set Name = REPLACE(Name, 'AWBMessagingStockEditButtonsGroup', 'MessagingStockEditButtonsGroup'), MenuButtonGroupType = REPLACE(MenuButtonGroupType, 'AWBMessagingStockEdit', 'MessagingStockEdit')  where ObjectTableId in (select Id from ObjectTables where Name in ('MessagingStock'))");
        }


        public override void Down()
        {
            RenameTable(name: "dbo.MessagingStockUsageHistories", newName: "AWBStockUsageHistories");
            RenameTable(name: "dbo.MessagingStocks", newName: "AWBMessagingStocks");
        }
    }
}
