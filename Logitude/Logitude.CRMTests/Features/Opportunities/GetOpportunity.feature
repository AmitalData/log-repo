@Pre-Prepare-Activity-Opportunity
Feature: Get Opportunity
	The API retrieves opportunity.

Scenario: Get opportunity
	When get opportunity with OpportunityId
	Then opportunity should be avaliable