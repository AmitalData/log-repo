DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.foreign_keys
WHERE parent_object_id = object_id(N'dbo.Shipments')
AND name like '%ForwardingPartnerId%';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Shipments] DROP CONSTRAINT [' + @var1 + ']')