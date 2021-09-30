@Pre-Prepare-UpdateDocument
Feature: Create Received Document
	We want to create received document
@Smoke
@Release 
Scenario: Create received document
	When create document
	Then document should be available
	Given following new document properties
		| property     | Value |
		| Received     | true  |
		| ReceivedDate | now   |
	When update document
	Then the document should update successfully