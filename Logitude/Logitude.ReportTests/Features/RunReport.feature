Feature: Run Report
	We want to run report.

Scenario: Run Report
	Given report with the following properties
		| property | Value                      |
		| Code     | OSBC                       |
		| Name     | Open Shipments by Customer |
	And filter fields
		| Name       | Value    | Table | From        |
		| CustomerId | Abdallah | Card  | EnglishName |
	When run report
	Then the report should run successfully