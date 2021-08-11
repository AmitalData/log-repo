Feature: Create Tariff Ocean FCL Freight Cost
	We want to create ocean FCL freight cost tariff.

Scenario: Create ocean FCL freight cost tariff
	Given an ocean FCL freight cost tariff with the following properties
		| property       | Value             |
		| Freight        | Ocean FCL         |
		| Name           | specflow name     |
		| ContractNumber | 43242312          |
		| Seller         | Maersk lines; INC |
		| Currency       | EUR               |
		| StartDate      | 2021-07-15        |
		| ExpirationDate | 2021-07-18        |
		| FreightCharge  | Ocean Freight     |
		| Notes          | specflow note     |
		| ContainerType  | ContainerId       |
	When create ocean FCL freight cost tariff
	Then the ocean FCL freight cost tariff should create successfully