Feature: Get System 1000 Flat File
	we want to check this api security.

Scenario: Get System 1000 Flat File by not authentication user.
	When Get System 1000 Flat File by not login user
	Then The Get System 1000 Flat File API should return you have no permissions

Scenario: Get System 1000 Flat File by not authorize user.
	When Get System 1000 Flat File by not authorize user
	Then The Get System 1000 Flat File API should return you have no permissions