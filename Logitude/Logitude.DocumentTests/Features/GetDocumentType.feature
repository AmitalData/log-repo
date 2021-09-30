@Pre-Prepare-DocumentType
Feature: Get Document Type
	We want to get document type.
@Smoke
@Release 
Scenario: Get document type
	When get document types
	Then document types should be available