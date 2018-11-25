
--If(OBJECT_ID('tempdb..#DIM_TransportModesTemp') Is Not Null)
--Begin
--    Drop Table #DIM_TransportModesTemp
--End

--CREATE TABLE #DIM_TransportModesTemp (
--	Code varchar(1) not null primary key,
--	Name varchar(13) not null
--);



insert into #DIM_TransportModesTemp  (Code,Name) values ( '1' ,'Not Specified' )



   declare @Id as varchar(1)
   declare @Name as varchar(10)

	DECLARE TransportModesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_TransportModes
	OPEN TransportModesCursor FETCH NEXT FROM TransportModesCursor INTO @Id , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_TransportModesTemp  (Code,Name) values(@Id,@Name)

	FETCH NEXT FROM TransportModesCursor INTO @Id , @Name
		End
	CLOSE TransportModesCursor
	DEALLOCATE TransportModesCursor



IF OBJECT_ID ('NewDIM_TransportModes', 'U')  IS NOT NULL Begin  Drop Table NewDIM_TransportModes End
	SELECT * 
INTO NewDIM_TransportModes
FROM #DIM_TransportModesTemp


If(OBJECT_ID('tempdb..#DIM_TransportModesTemp') Is Not Null)
Begin

    Drop Table #DIM_TransportModesTemp
End


ALTER TABLE NewDIM_TransportModes ADD CONSTRAINT PK_NewDIM_TransportModes_Code PRIMARY KEY CLUSTERED (Code);   
CREATE NONCLUSTERED INDEX [IX_DIM_TransportModes_Code] ON [dbo].[NewDIM_TransportModes]([Code])


--IF OBJECT_ID ('DIM_TransportModes', 'U')  IS NOT NULL
--begin
--EXEC sp_rename 'DIM_TransportModes', 'OldDIM_TransportModes'

--end

--EXEC sp_rename 'NewDIM_TransportModes', 'DIM_TransportModes'


--IF OBJECT_ID ('OldDIM_TransportModes', 'U')  IS NOT NULL
--begin
 
-- IF EXISTS (SELECT * 
--  FROM sys.foreign_keys 
--   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_TransportModes_TransportMode')
--   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
--)
-- begin

--  ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_DIM_TransportModes_TransportMode;
--  end


--    drop table OldDIM_TransportModes
--end




--ALTER TABLE DIM_TransportModes ADD CONSTRAINT PK_DIM_TransportModes_Code PRIMARY KEY CLUSTERED (Code);    

-- IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
--begin



--  -- declare @count  as varchar(15)
--  --set @count = (select  count(*) from Fact_Shipments  where TransportMode not in (select Code from DIM_TransportModes)) if(@count >0)   begin update  Fact_Shipments set TransportMode = '1' where  TransportMode not in (select Code from DIM_TransportModes)  end

--ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_TransportModes_TransportMode  FOREIGN KEY (TransportMode) REFERENCES DIM_TransportModes(Code);
-- end


--  CREATE NONCLUSTERED INDEX [IX_DIM_TransportModes_Code] ON [dbo].[DIM_TransportModes]([Code])

