Feature: Update Document
	We want to update a document

Scenario: Update document
	Given document
	And following new document properties
		| property           | Value                |
		| IsMultiCurrency    | true                 |
		| DisplayNumber      | Unique Number        |
		| AccountTypeCode    | Vendor               |
		| LocalName          | VendGlAccountLocal   |
		| EnglishName        | VendGlAccountEnglish |
		| RevenueExpenseType | Other                |
		| IsControlAccount   | false                |
		| ControlAccountId   | 1921681254           |
	When update document
	Then the document should update successfully