Feature: ChargeType
	In order to check charge types
	As an administrator
	I want to be able to create a new charge type

Background:
Successful login with valid credentials
	Given Email is ahmadb123@mail.com and password is ahmed13!A15
	When Make login
	Then The user successfully logged in

Scenario: Create a new charge type
	Given Charge type with the following data
		| name             | value            |
		| Code             | TCT              |
		| EnglishName      | Test Charge Type |
		| ChargesGroupCode | NONE             |
		| MeasurementId    | 1-22383          |
	When Try to create the charge type
	Then The charge type will created successfully