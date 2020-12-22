Feature: ChargeType
	In order to check charge types
	As a customer care
	I want to be able to create a new charge type

@mytag
Scenario: Create a new charge type
	Given The charge type code is TCT
	And The charge type name is Test Charge Type
	And The charge type group code is NONE
	And The charge type measurement code is FIXD
	When Try to create the charge type
	Then The charge type will created successfully