Feature: Create Tariff Ocean LCL Surcharge Cost
	We want to create ocean LCL surcharge cost tariff.

Scenario: Create ocean LCL surcharge cost tariff
	Given an ocean LCL surcharge cost tariff with the following properties
		| property       | Value                          |
		| Freight        | Ocean LCL Surcharge            |
		| Name           | specflow name                  |
		| ContractNumber | 43242312                       |
		| Seller         | Yangming marine transport corp |
		| Currency       | EUR                            |
		| Notes          | specflow note                  |
		| FreightCharge  | Ocean Freight                  |
		| Measurement    | Gross Weight                   |
	When create ocean LCL surcharge cost tariff
	Then the ocean LCL surcharge cost tariff should create successfully