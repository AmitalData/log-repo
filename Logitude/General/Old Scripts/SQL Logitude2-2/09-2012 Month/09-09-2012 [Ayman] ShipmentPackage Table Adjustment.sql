


update TextCodes set DefaultText = 'Pieces' where Code = 'ShipmentPackage.F.Quantity'

--alter table shipmentpackages drop column AWBPrintRate

--alter table shipmentpackages add AWBRateCharge float null

--alter table shipmentpackages add CommodityNumber varchar(7) null


--delete from ObjectFields where FieldName = 'AWBPrintRate' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentPackage')

--delete from TextCodes where Code like 'ShipmentPackage.%AWB%'


--alter table shipmentpackages add RateClassCode varchar(3) null

--ALTER TABLE [dbo].[shipmentpackages]
--ADD CONSTRAINT [FK_ShipmentPackageRateClass]
--    FOREIGN KEY ([RateClassCode])
--    REFERENCES [dbo].[RateClasses]
--        ([Code])
--    ON DELETE NO ACTION ON UPDATE NO ACTION;


--CREATE INDEX [IX_FK_ShipmentPackageRateClass]
--ON [dbo].[shipmentpackages]
--    ([RateClassCode]);
--GO