

update Opportunities set StageId = (select Id from Stages where Tenant = Opportunities.Tenant and Code = 'QUA') where StageId is null
GO

ALTER TABLE Opportunities alter column StageId varchar(15) not null
GO
