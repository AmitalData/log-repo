alter table shipments add LastFSRStatusRequestDate datetime null
go

alter table shipments add IsFSRSent bit not null default 0
go
