-- excute on main DB

alter table EventTypes add [IsSharedLogisticsEnabled] bit not null default 0 
go