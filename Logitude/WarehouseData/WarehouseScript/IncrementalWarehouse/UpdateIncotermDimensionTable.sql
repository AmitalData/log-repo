
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Incoterm' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Incoterms )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

    declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE IncotermsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Incoterms.Id, dw_Incoterms.Name , dw_Incoterms.LocalName ,dw_Incoterms.Code, dw_Incoterms.Tenant,dw_DWHSettings.ParentTenant, dw_Incoterms.AutomaticLastUpdateDate, dw_Incoterms.InActive
	From dw_Incoterms
	inner JOIN dw_DWHSettings ON dw_Incoterms.Tenant = dw_DWHSettings.Tenant
	where dw_Incoterms.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN IncotermsCursor FETCH NEXT FROM IncotermsCursor INTO @Id , @Name, @LocalName, @Code, 	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from DIM_Incoterms where Id = @Id)
	if(@Key is  null) begin  insert into DIM_Incoterms (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive]) values(@Id,@Name,@LocalName,@Code ,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive) end
	else begin update   DIM_Incoterms set Name =@Name,  [Local Name] =@LocalName ,  Code = @Code ,[Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate, [InActive] = @InActive  Where Id = @Id; end
    

	FETCH NEXT FROM IncotermsCursor  INTO @Id , @Name, @LocalName, @Code,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE IncotermsCursor
	DEALLOCATE IncotermsCursor
	
	update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Incoterm'

End
