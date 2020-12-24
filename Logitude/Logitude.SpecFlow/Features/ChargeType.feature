Feature: ChargeType
	In order to check charge types
	As an administrator
	I want to be able to create a new charge type

Background:
Successful login with valid credentials
	Given The email is ahmadb123@mail.com
	Given The password is ahmed13!A15
	When Make login
	Then The user successfully logged in

Scenario: Create a new charge type
	Given The charge type code is TCT
	And The charge type name is Test Charge Type
	And The charge type group code is NONE
	And The charge type measurement id is 1-22383
	When Try to create the charge type
	Then The charge type will created successfully