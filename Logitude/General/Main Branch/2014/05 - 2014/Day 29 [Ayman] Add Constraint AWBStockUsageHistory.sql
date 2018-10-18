

alter table AWBStockUsageHistories add constraint sm_ShipmentType UNIQUE (ShipmentId,MessageType)