Feature: Create Data entry
	We want to create Data entry.

Scenario: Create data entry
	Given a data entry with the following properties
		| property    | Value           |
		| Employee    | SpecflowTest    |
		| Project     | specflow prject |
		| WINumber    | 100001          |
		| Description | specflow desc   |
		| Sprint      | specflow sprint |
		| Location    | Office          |
		| DateOfWork  | Today           |
		| Minuts      | 30              |
	When create data entry
	Then the data entry should create successfully