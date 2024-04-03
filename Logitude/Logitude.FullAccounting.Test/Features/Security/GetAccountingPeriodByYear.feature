Feature: Get Accounting Period By Year
	we want to check this api security.

Scenario: Get accounting period by year by not authentication user.
	When get accounting period by year by not login user
	Then the get accounting period by year api should return you have no permissions

Scenario: Get accounting period by year by not authorize user.
	When get accounting period by year by not authorize user
	Then the get accounting period by year api should return you have no permissions