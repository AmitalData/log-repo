@Pre-Prepare-SendDocumentOut
Feature: Send Document Out
	we want to send document out
@Smoke
@Release 
Scenario: Send document out
	Given a document out for send
	When send document
	Then the document should be send successfully