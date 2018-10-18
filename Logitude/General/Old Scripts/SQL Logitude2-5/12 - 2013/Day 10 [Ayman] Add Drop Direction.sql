


if not exists (select * from Directions where Id = 'R')
insert into Directions(Id,Name,SearchFields) values('R','Drop','R,Drop')

select * from Directions