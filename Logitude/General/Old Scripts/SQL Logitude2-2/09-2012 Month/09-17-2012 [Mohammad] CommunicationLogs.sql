alter table communicationlogs add [From] varchar(1000) null
alter table communicationlogs alter column [To] varchar(1000) null

alter table communicationlogs add LastStatusDate DateTime not null default GetDate()

insert into communicationlogTypes (code,name) values('T','Transmission')

