
If(OBJECT_ID('tempdb..#DIM_TransportModesTemp') Is Not Null)
Begin
    Drop Table #DIM_TransportModesTemp
End

CREATE TABLE #DIM_TransportModesTemp (
	Code varchar(1) not null primary key,
	Name varchar(10) not null
);



insert into #DIM_TransportModesTemp values ( '1' ,'NotSpecifi' )



   declare @Id as varchar(1)
   declare @Name as varchar(10)

	DECLARE TransportModesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_TransportModes
	OPEN TransportModesCursor FETCH NEXT FROM TransportModesCursor INTO @Id , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_TransportModesTemp values(@Id,@Name)

	FETCH NEXT FROM TransportModesCursor INTO @Id , @Name
		End
	CLOSE TransportModesCursor
	DEALLOCATE TransportModesCursor




	SELECT * 
INTO NewDIM_TransportModes
FROM #DIM_TransportModesTemp


IF OBJECT_ID ('DIM_TransportModes', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_TransportModes', 'OldDIM_TransportModes'

end

EXEC sp_rename 'NewDIM_TransportModes', 'DIM_TransportModes'


IF OBJECT_ID ('OldDIM_TransportModes', 'U')  IS NOT NULL
begin
 
 IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_TransportModes_TransportMode')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

  ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_DIM_TransportModes_TransportMode;
  end


    drop table OldDIM_TransportModes
end




ALTER TABLE DIM_TransportModes ADD CONSTRAINT PK_DIM_TransportModes_Code PRIMARY KEY CLUSTERED (Code);    

 IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_TransportModes_TransportMode  FOREIGN KEY (TransportMode) REFERENCES DIM_TransportModes(Code);
 end


If(OBJECT_ID('tempdb..#DIM_TransportModesTemp') Is Not Null)
Begin
    Drop Table #DIM_TransportModesTemp
End


