Feature: CreateNewChargeType
    In order to create a new charge type
	As a user you have to login with your credentials,
	then you can create a new charge type to your tenant

Background:
	successful login with valid credentials
	Given user have the following Login Properties
		| Email              | Password    |
		| ahmadb123@mail.com | ahmed13!A15 |
	When the user call Login API
	Then the user will have a token

Scenario: create new charge type
	Given user add a charge type with the following properties
		| name             | value                   |
		| EnglishName      | Test Charge Type        |
		| ChargesGroupCode | NONE                    |
		| MeasurementId    | Get id from code {FIXD} |
		| ChargesGroupId   | Get id from code {NONE} |
	When the user call create charge type API
	Then a new charge type should be added