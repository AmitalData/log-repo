  


If(OBJECT_ID('tempdb..#DIM_DirectionsTemp') Is Not Null)
Begin
    Drop Table #DIM_DirectionsTemp
End

CREATE TABLE #DIM_DirectionsTemp (
	Code varchar(1) not null primary key,
	Name varchar(40) not null
);
 
 
insert into #DIM_DirectionsTemp values ( '1' ,'Not Specified' )

   declare @Id as varchar(1)
   declare @Name as varchar(40)

	DECLARE DirectionsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_Directions
	OPEN DirectionsCursor FETCH NEXT FROM DirectionsCursor INTO @Id , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_DirectionsTemp values(@Id,@Name)

	FETCH NEXT FROM DirectionsCursor INTO @Id , @Name
		End
	CLOSE DirectionsCursor
	DEALLOCATE DirectionsCursor




		SELECT * 
INTO NewDIM_Directions
FROM #DIM_DirectionsTemp

If(OBJECT_ID('tempdb..#DIM_DirectionsTemp') Is Not Null)
Begin
    Drop Table #DIM_DirectionsTemp
End



IF OBJECT_ID ('DIM_Directions', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Directions', 'OldDIM_Directions'

end

EXEC sp_rename 'NewDIM_Directions', 'DIM_Directions'


IF OBJECT_ID ('OldDIM_Directions', 'U')  IS NOT NULL
begin

 IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_Dim_Directions_Direction')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_Dim_Directions_Direction;
  end

    drop table OldDIM_Directions
end





ALTER TABLE DIM_Directions ADD CONSTRAINT PK_DIM_Directions_Code PRIMARY KEY CLUSTERED (Code);    

 IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_Dim_Directions_Direction  FOREIGN KEY (Direction) REFERENCES Dim_Directions(Code);
 end



 CREATE NONCLUSTERED INDEX [IX_Dim_Directions_Code]
ON [dbo].[DIM_Directions]([Code])

