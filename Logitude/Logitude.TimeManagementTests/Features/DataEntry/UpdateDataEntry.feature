@Pre-Prepare-DataEntry
Feature: Update Data Entry
	We want to Update Data Entry.

Scenario: Update data entry
Given a data entry
	And following data entry properties
		| property    | Value           |
		| Employee    | SpecflowTest    |
		| Project     | specflow prject |
		| Description | specflow desc   |
		| Sprint      | specflow sprint |
		| Location    | Office          |
		| DateOfWork  | Today           |
		| Minuts      | 30              |
	When update data entry
	Then data entry should be Updated



	Scenario: Update ticket
	Given a ticket
	And following ticket properties
		| property    | Value                    |
		| EntityType  | Quote                    |
		| Subject     | updated specflow subject |
		| Description | updated specflow desc    |
	When update ticket
	Then the ticket should update successfully