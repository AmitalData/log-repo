Feature: Get AP Invoice Security Access

Background:
    Successful login with valid credentials for set of users
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: An AP Invoice From A User's Tenant is Gotten
	When The First user gets the first AP Invoice from AP Invoices list
	Then The AP Invoice which is related to the first user tanent is existed

Scenario: Get AP Invoice From Other Tenant
	When Second user get the AP Invoice that requested by first user
	Then AP Invoice for second user should not be exists