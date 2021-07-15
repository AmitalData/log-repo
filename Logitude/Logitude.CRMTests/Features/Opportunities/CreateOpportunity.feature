Feature: Create Opportunity
	We want to create opportunity.

Scenario: Create opportunity
	Given a opportunity with the following properties
		| property        | Value         |
		| OpportunityType | Expansion     |
		| Stage           | Qualification |
		| Subject         | specflow sub  |
		| ShipmentsCount  | 3             |
		| Rating          | Neutral       |
	When create opportunity
	Then the opportunity should create successfully