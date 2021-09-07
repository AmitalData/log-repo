@Pre-Prepare-UpdateDataEntry
Feature: Update Data Entry
	We want to Update Data Entry.

Scenario: Update data entry
	Given a data entry
	And following data entry properties
		| property    | Value                  |
		| Employee    | SpecflowTest           |
		| Project     | specflow prject update |
		| Description | specflow desc  update  |
		| Sprint      | specflow sprint update |
		| Location    | Home                   |
		| DateOfWork  | Today - 1 hour         |
		| Minuts      | 60                     |
	When update data entry
	Then the data entry should update successfully

