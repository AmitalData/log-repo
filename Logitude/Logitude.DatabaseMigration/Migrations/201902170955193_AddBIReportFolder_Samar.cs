namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBIReportFolder_Samar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BIReportFolders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        Description = c.String(maxLength: 500, unicode: false),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);

            Sql(@"declare @Tenant as int
                declare @Id as varchar(15)
                declare @UserEmail as varchar(100)
                declare @UserId as varchar(15)

                BEGIN 
	                DECLARE TenantsCursor CURSOR READ_ONLY
	                FOR
	                SELECT Id
	                FROM Tenants
	                OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
	                WHILE @@FETCH_STATUS = 0
		                BEGIN

		                set @UserEmail = 'system@tenant' + CONVERT(varchar, @Tenant) +'.com'
		                set @UserId = (select id from users where id in (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant))
		
		                if(@UserId is not null)
		                begin
			                if not exists (select Id from BIReportFolders where Tenant = @Tenant and [Name] = 'General')
			                begin 
		           
				                EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'BIReportFolder'
				                insert into BIReportFolders(Id, Tenant, CreateDate, CreatedByUserId, UpdateDate, UpdatedByUserId, SearchFields, [Name], [Description], [Index])
				                values(@Id, @Tenant, GETDATE(), @UserId, GETDATE(), @UserId, 'General', 'General',NULL, 0)   
				
			                end 
		                end  
		
                           FETCH NEXT FROM TenantsCursor INTO @Tenant      
                        END
	                CLOSE TenantsCursor
	                DEALLOCATE TenantsCursor
                END");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BIReportFolders", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReportFolders", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.BIReportFolders", new[] { "UpdatedByUserId" });
            DropIndex("dbo.BIReportFolders", new[] { "CreatedByUserId" });
            DropTable("dbo.BIReportFolders");
        }
    }
}
