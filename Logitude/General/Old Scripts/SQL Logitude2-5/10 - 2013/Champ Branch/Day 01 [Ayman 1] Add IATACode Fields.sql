
begin transaction
begin

alter table IATACodes add MeasurementCode varchar(4) null

alter table IATACodes add DueTypeCode varchar(2) null

alter table IATACodes add constraint [IATACode_DueType] foreign key ([DueTypeCode]) references [DueTypes]([Code]);

END
commit transaction