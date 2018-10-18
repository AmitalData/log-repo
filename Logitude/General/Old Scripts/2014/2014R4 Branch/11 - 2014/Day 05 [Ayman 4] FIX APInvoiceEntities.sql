


DELETE FROM APInvoiceEntities WHERE Id NOT IN (SELECT MIN(Id) FROM APInvoiceEntities GROUP BY Tenant, APInvoiceId, EntityId)
go

declare @Tenant as int
declare @Id as varchar(15)
declare @EntityId as varchar(15)
declare @MainEntityReference as varchar(15)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, EntityId
		FROM APInvoiceEntities
		where EntityReference is null
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO  @Id, @Tenant, @EntityId
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if (@EntityId is not null)
			begin
				set @MainEntityReference = (select ShipmentNumber from Shipments where Tenant = @Tenant AND Id = @EntityId)
				
				update APInvoiceEntities
				set EntityReference = @MainEntityReference
				where Id = @Id AND Tenant = @Tenant
			end

			FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @EntityId
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END

alter table APInvoiceEntities add constraint uc_APInvoiceEntities UNIQUE (EntityId,APInvoiceId,ObjectTableId)
GO