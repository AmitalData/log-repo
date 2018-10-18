




begin transaction
begin


delete from PostLikes

update Posts set NumberOfLikes = 0

ALTER TABLE [Posts] ALTER COLUMN NumberOfLikes int not null

 

END
commit transaction



