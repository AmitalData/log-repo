Feature: Create Opportunity
	We want to create opportunity.

Scenario: Create opportunity
	Given a opportunity with the following properties
		| property        | Value                |
		| OpportunityType | Expansion            |
		| Stage           | Qualification        |
		| Subject         | specflow sub         |
		| Shipments       | 3                    |
		| Rating          | Neutral              |
		| Customer        | TestCustomer         |
		| Contact         | TestCustomer Contact |
	When create opportunity
	Then the opportunity should create successfully