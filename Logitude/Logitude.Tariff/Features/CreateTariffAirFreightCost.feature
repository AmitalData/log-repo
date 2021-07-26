Feature: Create Tariff Air Freight Cost
	We want to create air freight cost tariff.

Scenario: Create air freight cost tariff
	Given an air freight cost tariff with the following properties
		| property       | Value             |
		| Freight        | Air               |
		| Name           | specflow name     |
		| ContractNumber | 43242312          |
		| Seller         | American Airlines |
		| Currency       | EUR               |
		| StartDate      | 2021-07-15        |
		| ExpirationDate | 2021-07-18        |
		| Product        | General           |
		| FreightCharge  | Air Freight       |
		| Notes          | specflow note     |
	When create air freight cost tariff
	Then the air freight cost tariff should create successfully