Feature: Get Card GLAccount Connect
	We want to get card glAccount connect without token.

Scenario: Get card glAccount connect without token.
	When get bank accounts summary without token
	Then The get card glAccount connect API should return you have no permissions