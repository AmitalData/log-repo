Feature: Create Phone Call
	We want to create phone call.

Scenario: Create phone call
	Given a phone call with the following properties
		| property    | Value            |
		| Subject     | specflow sub     |
		| Description | specflow desc    |
		| Duration    | 30               |
		| DueDate     | 2021-07-15 14:40 |
		| Priority    | Normal           |
	When create phone call
	Then the phone call should create successfully