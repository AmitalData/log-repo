@Pre-Prepare-PrintDocumentOut
Feature: Print Document Out
	we want to print document out
@Smoke
@Release 
Scenario: Print document out
	Given a document out
	When print document
	Then the document should be print successfully