@Pre-Prepare-DocumentType
Feature: View Document
	we want to get document


Scenario: View document
	Given a document
	When get document
	Then the document should be available