Feature: Create Tariff Ocean FCL Surcharge Cost
	We want to create ocean FCL surcharge cost tariff.

Scenario: Create ocean FCL surcharge cost tariff
	Given an ocean FCL surcharge cost tariff with the following properties
		| property       | Value               |
		| Freight        | Ocean FCL Surcharge |
		| Name           | specflow name       |
		| ContractNumber | 43242312            |
		| Seller         | TestAgentExport     |
		| Currency       | EUR                 |
		| Notes          | specflow note       |
		| FreightCharge  | Ocean Freight       |
		| Measurement    | Gross Weight        |
		| ContainerType  | ContainerId         |
	When create ocean FCL surcharge cost tariff
	Then the ocean FCL surcharge cost tariff should create successfully