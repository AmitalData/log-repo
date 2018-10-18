

BEGIN;

INSERT INTO  ColorIndexs VALUES (1,'#8B008B');
INSERT INTO  ColorIndexs VALUES (2,'#FF0000');
INSERT INTO  ColorIndexs VALUES (3,'#9B0000');
INSERT INTO  ColorIndexs VALUES (4,'#00008B');
INSERT INTO  ColorIndexs VALUES (5,'#0000FF');
INSERT INTO  ColorIndexs VALUES (6,'#EE82EE');

INSERT INTO  ColorIndexs VALUES (7,'#BB58D3');
INSERT INTO  ColorIndexs VALUES (8,'#7FFF00');
INSERT INTO  ColorIndexs VALUES (9,'#008000');
INSERT INTO  ColorIndexs VALUES (10,'#3CB371');

INSERT INTO  ColorIndexs VALUES (11,'#228B22');
INSERT INTO  ColorIndexs VALUES (12,'#B22222');
INSERT INTO  ColorIndexs VALUES (13,'#4B0082');
INSERT INTO  ColorIndexs VALUES (14,'#00FF00');
INSERT INTO  ColorIndexs VALUES (15,'#FF8C00');
INSERT INTO  ColorIndexs VALUES (16,'#BC8F8F');
INSERT INTO  ColorIndexs VALUES (17,'#FF1493');
INSERT INTO  ColorIndexs VALUES (18,'#6495ED');
INSERT INTO  ColorIndexs VALUES (19,'#008B8B');
INSERT INTO  ColorIndexs VALUES (20,'#BB58D3');




declare @index as int
declare @Tenant as int
declare @Id as varchar(15)
declare @ColorIndex as int
       DECLARE ContactsCursor CURSOR READ_ONLY
       FOR
       SELECT Id,IndexColor,Tenant
       From Contacts
       OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO @Id,@ColorIndex,@Tenant
       WHILE @@FETCH_STATUS = 0
       BEGIN
      
       if(@ColorIndex = 0 or @ColorIndex is null)
	    begin
      
       set @index= ABS(CHECKSUM(NewId())) % 20 + 1
      update contacts set IndexColor = @index where Id = @Id and Tenant= @Tenant
 
       print @index --print @Id
   end
       FETCH NEXT FROM ContactsCursor INTO  @Id,@ColorIndex,@Tenant
       END 
       CLOSE ContactsCursor
       DEALLOCATE ContactsCursor
END


