namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryForStatisticsToQuote_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "CountryForStatisticsId", c => c.String(maxLength: 15, unicode: false));            
            CreateIndex("dbo.Quotes", "CountryForStatisticsId");            
            AddForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries", "Id");

            Sql(@"
declare @Tenant as int
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @QuoteId as varchar(15)
declare @DirectionId as varchar(1)
declare @TransportmodeId as varchar(1)
declare @ToPartnerAddressId as varchar(15)
declare @ToAddressCountryId as varchar(15)
declare @IncludeDelivery as bit
declare @CountryForStatisticsId as varchar(15)

BEGIN 
		DECLARE QuotesCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant,FromPortId,ToPortId,DirectionId,TransportModeId, ToPartnerAddressId, IncludePickUp, ToAddressCountryId
		FROM Quotes
		OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId,@Tenant,@FromPortId,@ToPortId,@DirectionId,@TransportmodeId, @ToPartnerAddressId, @IncludeDelivery, @ToAddressCountryId
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if(@DirectionId = 'I')
			begin
				set @CountryForStatisticsId = (select CountryId from Ports where Id = @FromPortId)		
			end

			else if(@DirectionId = 'E')
			begin
				if (@IncludeDelivery = 1)
				begin
					set @CountryForStatisticsId = @ToAddressCountryId
				end

				if (@CountryForStatisticsId is null)
				begin
					set @CountryForStatisticsId = (select CountryId from Ports where Id = @ToPortId)
			    end
			end

			else if(@DirectionId = 'D')
			begin
				if(@TransportmodeId = 'I')
				begin
					set @CountryForStatisticsId = (select CountryId from Addresses where Id = @ToPartnerAddressId)
				end

				else
				begin
					set @CountryForStatisticsId = (select CountryId from Ports where Id = @ToPortId)
				end
			end

			else if(@DirectionId = 'R')
			begin
				set @CountryForStatisticsId = (select CountryId from Ports where Id = @ToPortId)
			end

			update Quotes set CountryForStatisticsId = @CountryForStatisticsId where Id = @QuoteId and Tenant = @Tenant

			FETCH NEXT FROM QuotesCursor INTO  @QuoteId,@Tenant,@FromPortId,@ToPortId,@DirectionId,@TransportmodeId, @ToPartnerAddressId, @IncludeDelivery, @ToAddressCountryId
			END
		CLOSE QuotesCursor
		DEALLOCATE QuotesCursor
END");
        }
        
        public override void Down()
        {            
            DropForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries");            
            DropIndex("dbo.Quotes", new[] { "CountryForStatisticsId" });           
            DropColumn("dbo.Quotes", "CountryForStatisticsId");           
        }
    }
}
