@Pre-Prepare-Accounting
Feature: Update GlAccount
	We want to update a GlAccount

Scenario: Update glAccount
	Given glAccount
	And following new glAccount properties
		| property           | Value                |
		| IsMultiCurrency    | true                 |
		| DisplayNumber      | Unique Number        |
		| AccountTypeCode    | Vendor               |
		| LocalName          | VendGlAccountLocal   |
		| EnglishName        | VendGlAccountEnglish |
		| RevenueExpenseType | Other                |
		| IsControlAccount   | false                |
		| ControlAccountId   | 1921681254           |
	When update glAccount
	Then the glAccount should update successfully