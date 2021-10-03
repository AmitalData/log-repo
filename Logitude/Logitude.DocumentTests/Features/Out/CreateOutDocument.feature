@Pre-Prepare-CreateOutDocument
Feature: Create Out Document
	we want to create out document 
@Smoke
@Release 
Scenario: Create out document
When create Air Manifest out document
Then the Air Manifest out document should be created successfully
When get Air Manifest out document copy
Then the Air Manifest out document copy should be available
When update Air Manifest out document properties
Then Air Manifest out document should be update