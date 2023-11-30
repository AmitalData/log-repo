@Pre-Prepare-Tariff-OceanFCL-Surcharge
Feature: Get Tariff Ocean FCL Surcharge Cost
	We want to get ocean FCL surcharge cost tariff.

Scenario: Get ocean FCL surcharge cost tariff
	When get ocean FCL surcharge cost tariff with TariffId
	Then ocean FCL surcharge cost tariff should be avaliable
