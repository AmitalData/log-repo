

update Reports set FeatureId = (select Id from Features where Code = 'Report.Features.DetailedShipmentCharges') where Code = 'DSCA'