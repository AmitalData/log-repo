
If(OBJECT_ID('tempdb..#DIM_TypesTemp') Is Not Null)
Begin
    Drop Table #DIM_TypesTemp
End

CREATE TABLE #DIM_TypesTemp (
	Code varchar(4) not null primary key,
	Name varchar(40) not null
);
 

insert into #DIM_TypesTemp values ('1' , 'Not Specified' )

   declare @Id as varchar(4)
   declare @Name as varchar(40)

	DECLARE ShipmentTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_Types
	OPEN ShipmentTypesCursor FETCH NEXT FROM ShipmentTypesCursor INTO @Id , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_TypesTemp values(@Id,@Name)

	FETCH NEXT FROM ShipmentTypesCursor INTO @Id , @Name
		End
	CLOSE ShipmentTypesCursor
	DEALLOCATE ShipmentTypesCursor
		

	SELECT * 
INTO NewDIM_Types
FROM #DIM_TypesTemp

If(OBJECT_ID('tempdb..#DIM_TypesTemp') Is Not Null)
Begin
    Drop Table #DIM_TypesTemp
End



IF OBJECT_ID ('DIM_Types', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Types', 'OldDIM_Types'

end

EXEC sp_rename 'NewDIM_Types', 'DIM_Types'


IF OBJECT_ID ('OldDIM_Types', 'U')  IS NOT NULL
begin
 IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Types_Type')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Types_Type
  end

    drop table OldDIM_Types
end




ALTER TABLE DIM_Types ADD CONSTRAINT PK_DIM_Types_Code PRIMARY KEY CLUSTERED (Code);    

 IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Types_Type  FOREIGN KEY (Type) REFERENCES DIM_Types(Code);
 end


 CREATE NONCLUSTERED INDEX [IX_DIM_Types_Code]
ON [dbo].[DIM_Types]([Code])
