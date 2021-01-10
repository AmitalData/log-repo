Feature: Get AP Invoice Security Access 
        In order to get an APInvoice from the APInvoices list 
		It should be related to the user's tenant 

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

Scenario: An AP Invoice From  From Other Tenant isn't Gotten Get 
	When The Second user gets the AP Invoice that was requested by the first user
	Then The AP Invoice that was requested by the second user isn't existed