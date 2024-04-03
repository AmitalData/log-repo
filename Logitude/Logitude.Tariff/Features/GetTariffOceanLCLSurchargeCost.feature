@Pre-Prepare-Tariff-OceanLCL-Surcharge
Feature: Get Tariff Ocean LCL Surcharge Cost
	We want to get ocean LCL surcharge cost tariff.

Scenario: Get ocean LCL surcharge cost tariff
	When get ocean LCL surcharge cost tariff with TariffId
	Then ocean LCL surcharge cost tariff should be avaliable
