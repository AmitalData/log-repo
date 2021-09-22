Feature: Put System 1000 File
	we want to check this api security.

Scenario: Put System 1000 File by not authentication user.
	When put system file by not login user
	Then the put system file api should return you have no permissions
Scenario: Put System File by not authorize user.
	When put system file by not authorize user
	Then the put system file api should return you have no permissions