Feature: Create Get Tariff Air Freight Cost 
	We want to create, get air freight cost tariff.

Scenario: Create air freight cost tariff
	Given a air freight cost tariff with the following properties
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

Scenario: Get air freight cost tariff
	When get air freight cost tariff with TariffId
	Then air freight cost tariff should be avaliable