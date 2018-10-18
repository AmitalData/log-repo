sp_rename 'objectfields.ControlField', 'ControlField1', 'COLUMN'
GO
alter table objectfields add  [ControlField2] varchar(100)   NULL