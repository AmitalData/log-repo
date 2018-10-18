
-- Run this SQL then update all Tenants
-- This will Add the Code to the Query Columns, and to the New and Edit City Screen

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'CountryCity.GeneralTabScreen' and ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity'))
GO

delete from QueryColumns where QueryId = (select Id from Queries where Code = 'Country Cities' and ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity'))
GO