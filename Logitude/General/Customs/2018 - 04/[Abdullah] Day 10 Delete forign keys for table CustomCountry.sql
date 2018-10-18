--
-- [ SQL Server script ]
-- delete TarrifCode forighn keys
--
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Customs].[CustomsCountry_CountryGroup]') AND parent_object_id = OBJECT_ID(N'[Customs].[CustomsCountries]'))
ALTER TABLE [Customs].[CustomsCountries] DROP CONSTRAINT [CustomsCountry_CountryGroup]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Customs].[Country_CountryGroup]') AND parent_object_id = OBJECT_ID(N'[Customs].[CustomsCountries]'))
ALTER TABLE [Customs].[CustomsCountries] DROP CONSTRAINT [Country_CountryGroup]
GO


--
-- Oracle script
--
--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Customs].[CustomsCountry_CountryGroup]') AND parent_object_id = OBJECT_ID(N'[Customs].[CustomsCountries]')) 
--	THEN ALTER TABLE Customs.CustomsCountries DROP;
--END IF; 
--CONSTRAINT CustomsCountry_CountryGroup


--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Customs].[Country_CountryGroup]') AND parent_object_id = OBJECT_ID(N'[Customs].[CustomsCountries]')) 
--	THEN ALTER TABLE Customs.CustomsCountries DROP;
--END IF;
--CONSTRAINT Country_CountryGroup


