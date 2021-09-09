Feature: CreateApprovedJournal
	We want to create an approved journal

Scenario: Create approved journal
	Given I have the following Journal lines:
		| Line | Action | AccountingDate | documentDate | dueDate    | creditAccountId | debitAccountId | localAmount | currencyId | exchangeRate |
		| 1    | זכות   | 3              | 01/09/2021   | 24/09/2021 | TEVA TEST\USD   | רחל2\DM        | 5           | NIS        | 1            |
		| 2    | חובה   | 5              | 01/09/2021   | 24/09/2021 | TEVA TEST\USD   | רחל2\DM        | 5           | NIS        | 1            |
	And a journal with the following properties
		| property       | Value    |
		| AccountingDate | Now      |
		| currencyId     | NIS      |
		| statusCode     | Approved |
		| typeCode       | 0        |
		| journalLines   | 1,2      |
	When create approved journal
	Then the journal should create successfully