Feature: Create Tariff Air Surcharge Cost
	We want to create air surcharge cost tariff.

Scenario: Create air surcharge cost tariff
	Given an air surcharge cost tariff with the following properties
		| property       | Value             |
		| Freight        | Air Surcharge     |
		| Name           | specflow name     |
		| ContractNumber | 43242312          |
		| Seller         | American Airlines |
		| Currency       | EUR               |
		| Notes          | specflow note     |
		| FreightCharge  | Air Freight       |
		| Measurement    | Gross Weight      |
	When create air surcharge cost tariff
	Then the air surcharge cost tariff should create successfully