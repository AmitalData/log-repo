


begin
declare @StorageEncryptionKey as nvarchar(40)

declare @Id as int

	DECLARE TenantCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @StorageEncryptionKey = (select StorageEncryptionKey from Tenants where  Id = @Id)

	if(@StorageEncryptionKey is  null)
	begin
	
		
DECLARE @s char(32);

SET @s = (
SELECT
	c1 AS [text()]
FROM
	(
	SELECT TOP (32) c1
	FROM
	  (
    VALUES
      ('A'), ('B'), ('C'), ('D'), ('E'), ('F'), ('G'), ('H'), ('I'), ('J'),
      ('K'), ('L'), ('M'), ('N'), ('O'), ('P'), ('Q'), ('R'), ('S'), ('T'),
      ('U'), ('V'), ('W'), ('X'), ('Y'), ('Z'), ('0'), ('1'), ('2'), ('3'),
      ('4'), ('5'), ('6'), ('7'), ('8'), ('9')	
	  ) AS T1(c1)
	ORDER BY ABS(CHECKSUM(NEWID()))
	) AS T2
FOR XML PATH('')
);

SELECT @s AS [@s];
update Tenants set StorageEncryptionKey = @s where Id = @Id

   end

	FETCH NEXT FROM TenantCursor INTO @Id
	END
	CLOSE TenantCursor
	DEALLOCATE TenantCursor
END

