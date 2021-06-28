@Pre-Prepare-Vessel
Feature: Vessel access 
	With pre-prepared users authentication and vessel
	We want to test vessel access.

Scenario: Get vessel for user's tenant
	When get vessel for user's tenant
	Then vessel should available