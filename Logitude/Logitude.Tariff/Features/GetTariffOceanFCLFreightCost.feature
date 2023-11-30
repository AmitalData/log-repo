@Pre-Prepare-Tariff-OceanFCL
Feature: Get Tariff Ocean FCL Freight Cost
	We want to get ocean FCL freight cost tariff.

Scenario: Get ocean FCL freight cost tariff
	When get ocean FCL freight cost tariff with TariffId
	Then ocean FCL freight cost tariff should be avaliable
