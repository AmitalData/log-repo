@Pre-Prepare-Tariff-Air
Feature: Get Tariff Air Freight Cost
	We want to get air freight cost tariff.

Scenario: Get air freight cost tariff
	When get air freight cost tariff with TariffId
	Then air freight cost tariff should be avaliable
