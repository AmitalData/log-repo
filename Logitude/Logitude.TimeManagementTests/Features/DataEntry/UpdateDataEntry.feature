@Pre-Prepare-DataEntry
Feature: Update Data Entry
	We want to update data entry.

Scenario: Update data entry
	Given a data entry
	And following data entry properties
		| property    | Value                 |
		| Minuts      | 60                    |
		| Description | updated specflow desc |
	When update data entry
	Then the data entry should update successfully