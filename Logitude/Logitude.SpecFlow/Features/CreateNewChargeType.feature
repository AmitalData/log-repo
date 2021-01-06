Feature: Create New Charge Type
    In order to create a new charge type
	As a user you have to login with your credentials,
	then you can create a new charge type to your tenant

Background:
	Successful login with valid credentials
	Given Users with following credentials
		| Email              | Password    |
		| ahmadb123@mail.com | ahmed13!A15 |
	When Users make login request
	Then Users should have token

Scenario: Create New Charge Type
	Given Charge type with the following properties
		| name             | value                                                 |
		| Code             | {RandomString(3)}                                     |
		| EnglishName      | Test Charge Type                                      |
		| ChargesGroupCode | NONE                                                  |
		| MeasurementId    | Get {Id} from {Code} {FIXD} using {MeasurementViews}  |
		| ChargesGroupId   | Get {Id} from {Code} {NONE} using {ChargesGroupViews} |
	When Create charge type
	Then New charge type should be created