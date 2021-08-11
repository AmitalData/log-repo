@Pre-Prepare-Tariff-Air-Surcharge
Feature: Get Tariff Air Surcharge Cost
	We want to get air surcharges cost tariff.

Scenario: Get air surcharge cost tariff
	When get air surcharge cost tariff with TariffId
	Then air surcharge cost tariff should be avaliable
