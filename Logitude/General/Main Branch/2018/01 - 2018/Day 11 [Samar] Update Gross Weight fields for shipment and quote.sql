
update Shipments set GrossWeightPerTon = round(GrossWeightInKG/1000, 3) where GrossWeight is not null

update Quotes set GrossWeightInKG = round(GrossWeight, 3) where GrossWeight is not null and GrossWeightUnitCode = 'KG'
update Quotes set GrossWeightInKG = round(GrossWeight * 0.45359237, 3) where GrossWeight is not null and GrossWeightUnitCode = 'LB'
update Quotes set GrossWeightInKG = round(GrossWeight * 1000, 3) where GrossWeight is not null and GrossWeightUnitCode = 'MT'

update Quotes set GrossWeightPerTon = round(GrossWeightInKG/1000, 3) where GrossWeight is not null