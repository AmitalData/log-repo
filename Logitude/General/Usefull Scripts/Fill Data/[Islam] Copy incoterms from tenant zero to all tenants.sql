 

--declare @Tenant as int
--declare @Email as varchar(70)
--declare @Count as int
 
--BEGIN 
--		DECLARE tenantsCursor CURSOR READ_ONLY
--		FOR
		
--		select id
--		from tenants
--		where id <> 0
   
--  OPEN tenantsCursor FETCH NEXT FROM tenantsCursor INTO @Tenant
--			WHILE @@FETCH_STATUS = 0
--			BEGIN
			 
			 
-- declare @Code as varchar(3)
-- declare @Name as varchar(40)
-- declare @AddedManually as bit
-- declare @InActive as bit
-- declare @Notes as varchar(250)
-- declare @LocalName as varchar(40)
-- declare @SearchFields as varchar(1000)
-- declare @AutomaticLastUpdateDate as datetime
-- declare @Freight as varchar(1)
-- declare @OtherCharges as varchar(1)
  
--		DECLARE incotermsCursor CURSOR READ_ONLY
--		FOR
--		SELECT Code,Name ,AddedManually,InActive,Notes,LocalName,SearchFields,Freight,OtherCharges
--		FROM  incoterms
--		where Tenant = 0 and Code not in (select code from incoterms where tenant = @Tenant)
		 
--    	OPEN incotermsCursor FETCH NEXT FROM incotermsCursor INTO  @Code,@Name ,@AddedManually,@InActive,@Notes,@LocalName,@SearchFields,@Freight,@OtherCharges	
--		WHILE @@FETCH_STATUS = 0
--			BEGIN
 
--				declare @NewId varchar(15)
--				execute usp_GetNextTableIdValue @NewId OUTPUT,'Incoterm' 
--				insert into Incoterms(Id,Tenant,Code,Name,LocalName,InActive,AddedManually,Notes,Freight,OtherCharges,SearchFields)
--							 Values(@NewId,@Tenant,@Code,@Name,@LocalName,@InActive,@AddedManually,@Notes,@Freight,@OtherCharges,@SearchFields)
--print @Code + ' - ' + CONVERT(varchar(15),@Tenant)
--			--END
 
--		   FETCH NEXT FROM incotermsCursor INTO @Code,@Name ,@AddedManually,@InActive,@Notes,@LocalName,@SearchFields,@Freight,@OtherCharges				
--			END
--		CLOSE incotermsCursor
--		DEALLOCATE incotermsCursor


--		FETCH NEXT FROM tenantsCursor INTO @Tenant
--		END
--		CLOSE tenantsCursor
--		DEALLOCATE tenantsCursor
--END





