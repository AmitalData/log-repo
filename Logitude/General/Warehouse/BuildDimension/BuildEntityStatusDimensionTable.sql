


If(OBJECT_ID('tempdb..#DIM_ShipmentStatusesTemp') Is Not Null)
Begin
    Drop Table #DIM_ShipmentStatusesTemp
End


CREATE TABLE #DIM_ShipmentStatusesTemp (
	Id_Number int not null identity(1,1) primary key,
    Id varchar(15) not null,
    Name varchar(40) not null,
    Code varchar(4) not null,
   	SourceTenant  int,
    ParentTenant  int,
);


insert into #DIM_ShipmentStatusesTemp values ('-1' , 'Not Specified' ,'NOSP' , 0, 0)

   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @Code as varchar(4)
   declare @SourceTenant int
   declare @ParentTenant int
   
   
	DECLARE EntityStatusCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name  ,Code, Tenant, Tenant
	From dw_ShipmentStatuses
	OPEN EntityStatusCursor FETCH NEXT FROM EntityStatusCursor INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ShipmentStatusesTemp values(@Id,@Name,@Code , 	@SourceTenant , @ParentTenant)

	FETCH NEXT FROM EntityStatusCursor  INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant
		End
	CLOSE EntityStatusCursor
	DEALLOCATE EntityStatusCursor


	SELECT * 
INTO NewDIM_ShipmentStatuses
FROM #DIM_ShipmentStatusesTemp

If(OBJECT_ID('tempdb..#DIM_ShipmentStatusesTemp') Is Not Null)
Begin
    Drop Table #DIM_ShipmentStatusesTemp
End



IF OBJECT_ID ('DIM_ShipmentStatuses', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_ShipmentStatuses', 'OldDIM_ShipmentStatuses'

end

EXEC sp_rename 'NewDIM_ShipmentStatuses', 'DIM_ShipmentStatuses'


IF OBJECT_ID ('OldDIM_ShipmentStatuses', 'U')  IS NOT NULL
begin
 IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_ShipmentStatuses_Status')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_ShipmentStatuses_Status;
  end

    drop table OldDIM_ShipmentStatuses
end









ALTER TABLE DIM_ShipmentStatuses ADD CONSTRAINT PK_DIM_ShipmentStatuses_Id_Number PRIMARY KEY CLUSTERED (Id_Number);    

  IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_ShipmentStatuses_Status  FOREIGN KEY (Status) REFERENCES DIM_ShipmentStatuses(Id_Number);

 end

 CREATE NONCLUSTERED INDEX [IX_DIM_ShipmentStatuses_Id]
ON [dbo].[DIM_ShipmentStatuses]([Id])


