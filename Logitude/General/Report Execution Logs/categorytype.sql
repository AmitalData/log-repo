INSERT INTO dbo.CategoryTypes(
Code,
Name,
SearchFields)
VALUES(
'RXL', -- Code - varchar(4)
'ReportExecutionLog', -- Name - varchar(40)
N'ReportExecutionLogs,ReportExecutionLog' -- SearchFields - nvarchar(1000)
)