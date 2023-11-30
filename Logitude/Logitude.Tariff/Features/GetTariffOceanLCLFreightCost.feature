@Pre-Prepare-Tariff-OceanLCL
Feature: Get Tariff Ocean LCL Freight Cost
	We want to get ocean LCL freight cost tariff.

Scenario: Get ocean LCL freight cost tariff
	When get ocean LCL freight cost tariff with TariffId
	Then ocean LCL freight cost tariff should be avaliable
