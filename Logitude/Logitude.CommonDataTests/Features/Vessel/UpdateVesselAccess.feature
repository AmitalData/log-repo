@Pre-Prepare-Vessel
Feature: Update Vessel access
	With pre-prepared users authentication and vessel
	We want to test Update Vessel security access.

Scenario: Update Vessel for user's tenant
	Given vessel for user's tenant
	When update vessel for user's tenant
	Then vessel should update successfully

Scenario: Update Vessel for other tenant
	Given vessel for the user's tenant
	When update vessel for other tenant
	Then update should receive error message