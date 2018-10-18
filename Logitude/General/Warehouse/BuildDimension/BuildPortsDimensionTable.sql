

If(OBJECT_ID('tempdb..#Dim_PortsTemp') Is Not Null)
Begin
    Drop Table #Dim_PortsTemp
End



--Create temporal #Dim_PortsTemp

CREATE TABLE #Dim_PortsTemp (
	Id_Number int not null identity(1,1) primary key,
    Id varchar(15) not null,
    Name varchar(40) not null,
	Code varchar(3) not null,
	LocalName nvarchar(40),
	UNLocCode varchar(30),
	Country varchar(120)  not null,
	StateName  varchar(40),
   	SourceTenant  int,
    ParentTenant  int,
);
insert into #Dim_PortsTemp values ('-1' , 'Not Specified' ,'NoS' ,'Not Specified','Not Specified','Not Specified','Not Specified',0,0)

   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @CombinedCode as varchar(30)
   declare @Country varchar(120) 
   declare @State varchar(40) 
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE PortsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Ports.Id, dw_Ports.EnglishName, dw_Ports.Code , dw_Ports.LocalName , dw_Ports.CombinedCode ,dw_Countries.EnglishName, dw_States.EnglishName, dw_Ports.Tenant,dw_Ports.Tenant
	From dw_Ports
	INNER JOIN dw_States ON dw_Ports.StateId = dw_States.Id
	INNER JOIN dw_Countries ON dw_Ports.CountryId = dw_Countries.Id

	OPEN PortsCursor FETCH NEXT FROM PortsCursor INTO @Id , @Name, @Code , @LocalName  , @CombinedCode, @Country , @State , @SourceTenant , @ParentTenant 
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #Dim_PortsTemp values(@Id,@Name,@Code,@LocalName ,@CombinedCode, @Country, @State , @SourceTenant , @ParentTenant)

	FETCH NEXT FROM PortsCursor  INTO @Id , @Name, @Code , @LocalName  , @CombinedCode, @Country , @State , @SourceTenant , @ParentTenant 
		End
	CLOSE PortsCursor
	DEALLOCATE PortsCursor
	

	SELECT * 
INTO NewDim_Ports
FROM #Dim_PortsTemp

If(OBJECT_ID('tempdb..#Dim_PortsTemp') Is Not Null)
Begin
    Drop Table #Dim_PortsTemp
End




IF OBJECT_ID ('DIM_Ports', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Ports', 'OldDIM_Ports'

end

EXEC sp_rename 'NewDIM_Ports', 'DIM_Ports'


IF OBJECT_ID ('OldDIM_Ports', 'U')  IS NOT NULL
begin
 IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_Dim_Ports_Origin')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

    ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_Dim_Ports_Origin;
    ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_Dim_Ports_FinalDestination;
  end


    drop table OldDIM_Ports
end



ALTER TABLE Dim_Ports ADD CONSTRAINT PK_Dim_Ports_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

  IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_Dim_Ports_Origin  FOREIGN KEY (Origin) REFERENCES Dim_Ports(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_Dim_Ports_FinalDestination  FOREIGN KEY (FinalDestination) REFERENCES Dim_Ports(Id_Number);
 

 end


CREATE NONCLUSTERED INDEX [IX_Dim_Ports_Id]
ON [dbo].[Dim_Ports]([Id])