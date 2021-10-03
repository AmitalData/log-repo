@Pre-Prepare-ViewDocument
Feature: View Document
	we want to get document

@Smoke
@Release 
Scenario: View document
	Given a document
	When get document
	Then the document should be available