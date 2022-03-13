Feature: Run Report
	We want to run report.

Scenario: Run Report
	Given report with the following properties
		| property | Value                      |
		| Code     | OSBC                       |
		| Name     | Open Shipments by Customer |
	And filter fields
		| Name | Value | Operator |
		| 70   | 10    | 20       |
		| 50   |       |          |
	When run report
	Then the report should run successfully