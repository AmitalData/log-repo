@Pre-Prepare
Feature: GET Quote Export Air
	The API retrieves direct export air quote.

Scenario: GET Quote Export Air
	When get quote with QuoteId
	Then quote should be avaliable