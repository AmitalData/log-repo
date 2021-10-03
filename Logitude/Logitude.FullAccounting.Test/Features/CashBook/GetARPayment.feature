Feature: Get Cashbookt
	We want to get cashbookt.
@Smoke
Scenario: Get cashbookt
	When get cashbookt with cashbooktId
	Then cashbookt should be available