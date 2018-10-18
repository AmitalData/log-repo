
---- We will do that with sp_fulltext_database procedure
--EXEC sp_fulltext_database 'enable'
---- Create catalog
--EXEC sp_fulltext_catalog 'ShipmentCatalog','create'
---- Add some indexes to database
--EXEC sp_fulltext_table 'Shipments', 'create', 'ShipmentCatalog', 'PK_Shipments'
---- add columns for searching to full text search index
--EXEC sp_fulltext_column 'Shipments', 'SearchFields', 'add'
 
---- Activate full text search indexes
--EXEC sp_fulltext_table 'Shipments','activate'
-- -- start full population of catalog
--EXEC sp_fulltext_catalog 'ShipmentCatalog', 'start_full'

CREATE FULLTEXT CATALOG ShipmentCatalog AS DEFAULT;
--CREATE UNIQUE INDEX ui_ukProductDescription ON SalesLT.ProductDescription(ProductDescriptionID); 
CREATE FULLTEXT INDEX ON Shipments(SearchFields) KEY INDEX PK_Shipments ON ShipmentCatalog; 

ALTER FULLTEXT INDEX ON Shipments ENABLE;  
ALTER FULLTEXT INDEX ON Shipments START FULL POPULATION;
 
