namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedEditFieldSessionTimeoutTableTenantLoginPolicy : DbMigration
    {
        public override void Up()
        {
            Sql("DECLARE @TableName AS NVARCHAR(255), @ColumnName AS NVARCHAR(255), @ConstraintName AS NVARCHAR(255), @DropConstraintSQL AS NVARCHAR(255)SET @TableName = 'TenantLoginPolicies' SET @ColumnName = 'SessionTimeout' SET @ConstraintName = (SELECT TOP 1 o.name FROM sysobjects o JOIN syscolumns c ON o.id = c.cdefault JOIN sysobjects t ON c.id = t.id WHERE o.xtype = 'd' AND c.name = @ColumnName AND t.name = @TableName) SET @DropConstraintSQL = 'ALTER TABLE ' + @TableName + ' DROP ' + @ConstraintName EXEC(@DropConstraintSQL)");
            AlterColumn("dbo.TenantLoginPolicies", "SessionTimeout", c => c.Decimal(nullable: false, precision: 18, scale: 2));

        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenantLoginPolicies", "SessionTimeout", c => c.Int(nullable: false));
        }
    }
}
