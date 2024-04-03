Feature: Create Tariff Ocean LCL Freight Cost
	We want to create ocean LCL freight cost tariff.

Scenario: Create ocean LCL freight cost tariff
	Given an ocean LCL freight cost tariff with the following properties
		| property       | Value             |
		| Freight        | Ocean LCL         |
		| Name           | specflow name     |
		| ContractNumber | 43242312          |
		| Seller         | Maersk lines; INC |
		| Currency       | EUR               |
		| StartDate      | 2021-07-15        |
		| ExpirationDate | 2021-07-18        |
		| FreightCharge  | Ocean Freight     |
		| Notes          | specflow note     |
	When create ocean LCL freight cost tariff
	Then the ocean LCL freight cost tariff should create successfully