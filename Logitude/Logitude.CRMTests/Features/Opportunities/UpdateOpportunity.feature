@Pre-Prepare-Activity-Opportunity
Feature: Update Opportunity
	We want to update opportunity.

Scenario: Update Opportunity
	Given an opportunity
	And following opportunity properties
		| property        | Value                |
		| Subject         | updated specflow sub |
		| Stage           | Development          |
		| OpportunityType | New Business         |
		| ShipmentsCount  | 5                    |
		| Rating          | Cold                 |
	When update opportunity
	Then the opportunity should update successfully