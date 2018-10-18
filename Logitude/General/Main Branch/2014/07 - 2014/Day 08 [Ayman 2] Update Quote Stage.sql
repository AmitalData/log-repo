

begin transaction
begin

	if COLUMNPROPERTY(OBJECT_ID(N'Quotes'), 'EntityStatusId', 'ColumnId') is not null	
	begin
	
		if COLUMNPROPERTY(OBJECT_ID(N'Quotes'), 'StageId', 'ColumnId') is not null
		begin

			update Quotes set StageId = (select Id from QuoteStages where Tenant = Quotes.Tenant and Code = (select Code from EntityStatus where Id = Quotes.EntityStatusId and Tenant = Quotes.Tenant))

			update EventTypes set EntityStatusId = null where ObjectTableId = (select Id from ObjectTables where Name = 'Quote')			

			if not exists (select * from Quotes where StageId is null)
			begin
				if exists (select * from sys.objects o where o.object_id = object_id(N'FK_EntityStatusQuote') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
				alter table Quotes drop FK_EntityStatusQuote
				
				if exists (select * from sys.indexes where name = 'IX_FK_EntityStatusQuote')
				drop index IX_FK_EntityStatusQuote on Quotes

				if COLUMNPROPERTY(OBJECT_ID(N'Quotes'), 'EntityStatusId', 'ColumnId') is not null
				alter table Quotes drop column EntityStatusId
			end
		end
	end

END
commit transaction

