
select * from DocumentTypeCustomFields1 where fieldCode = 'MoveType' and DocumentTypeId in (select Id from DocumentTypes where Code = 'BCO')
GO

select * from DocumentTypeCustomFields1 where fieldCode = 'MoveType' and DocumentTypeId in (select Id from DocumentTypes where Code = 'COO')
GO

