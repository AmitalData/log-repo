@Pre-Prepare-Accounting
Feature: CreateApprovedJournal
	We want to create an approved journal
@Smoke
Scenario: Create approved journal
	Given I have the following Journal lines:
		| Line | Action | AccountingDate | DocumentDate | DueDate    | CreditAccountNumber | DebitAccountNumber | LocalAmount | CurrencyId | ExchangeRate |
		| 1    | Credit | 09/01/2021     | 09/01/2021   | 09/24/2021 | Account1          | Account2         | 5           | NIS        | 1            |
		| 2    | Debit  | 09/01/2021     | 09/01/2021   | 09/24/2021 | Account1          | Account2         | 5           | NIS        | 1            |
	And a journal with the following properties
		| property       | Value      |
		| AccountingDate | 09/01/2021 |
		| currencyId     | NIS        |
		| statusCode     | Approved   |
		| typeCode       | 0          |
		| DueDate        | 09/24/2021 |
		| DocumentDate   | 09/01/2021 |
		| journalLines   | 1,2        |
	When create approved journal
	Then the journal should create successfully