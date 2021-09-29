@Pre-Prepare-UpdateDocument
Feature: Update Document
	We want to update a document

Scenario: Update document
	Given document
	And following new document properties
		| property     | Value |
		| Received     | true  |
		| ReceivedDate | now   |
	When update document
	Then the document should update successfully