
If(OBJECT_ID('tempdb..#DIM_LevelsTemp') Is Not Null)
Begin
    Drop Table #DIM_LevelsTemp
End

CREATE TABLE #DIM_LevelsTemp (
	Code varchar(1) not null primary key,
	Name varchar(40) not null
);
 

insert into #DIM_LevelsTemp values ('1' , 'Not Specified' )


   declare @Code as varchar(1)
   declare @Name as varchar(40)

	DECLARE ShipmentLevelsCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name
	From dw_Levels
	OPEN ShipmentLevelsCursor FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_LevelsTemp values(@Code,@Name)

	FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name
		End
	CLOSE ShipmentLevelsCursor
	DEALLOCATE ShipmentLevelsCursor
	

IF OBJECT_ID ('NewDIM_Levels', 'U')  IS NOT NULL Begin  Drop Table NewDIM_Levels End
	SELECT * 
INTO NewDIM_Levels
FROM #DIM_LevelsTemp

If(OBJECT_ID('tempdb..#DIM_LevelsTemp') Is Not Null)
Begin
    Drop Table #DIM_LevelsTemp
End

ALTER TABLE NewDIM_Levels ADD CONSTRAINT PK_NewDIM_Levels_Code PRIMARY KEY CLUSTERED (Code);   
CREATE NONCLUSTERED INDEX [IX_DIM_Levels_Code] ON [dbo].[NewDIM_Levels]([Code])






--IF OBJECT_ID ('DIM_Levels', 'U')  IS NOT NULL
--begin
--EXEC sp_rename 'DIM_Levels', 'OldDIM_Levels'

--end

--EXEC sp_rename 'NewDIM_Levels', 'DIM_Levels'


--IF OBJECT_ID ('OldDIM_Levels', 'U')  IS NOT NULL
--begin
 
-- IF EXISTS (SELECT * 
--  FROM sys.foreign_keys 
--   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Levels_Level')
--   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
--)
-- begin

--  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Levels_Level;
--  end


--    drop table OldDIM_Levels
--end







--ALTER TABLE DIM_Levels ADD CONSTRAINT PK_DIM_Levels_Code PRIMARY KEY CLUSTERED (Code);    

-- IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
--begin




-- --declare @count  as varchar(15)
-- --set @count = (select    count(*)  from Fact_Shipments  where Level not in (select Code from DIM_Levels))  if(@count >0)  begin update  Fact_Shipments set Level = '1' where  Level not in (select Code from DIM_Levels)  end

--ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Levels_Level  FOREIGN KEY (Level) REFERENCES DIM_Levels(Code);
-- end


--CREATE NONCLUSTERED INDEX [IX_DIM_Levels_Code]
--ON [dbo].[DIM_Levels]([Code])
