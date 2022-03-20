Feature: Run Report
	We want to run report.

Scenario: Run Report
	Given report with the following properties
		| property | Value                      |
		| Code     | OSBC                       |
		| Name     | Open Shipments by Customer |
	And filter fields
		| Name     | Value    | DataType | EntityName | PropertyName | SearchBy    |
		| Customer | Abdallah | String   | Card       | CustomerId   | EnglishName |
	When run report
	Then the report should run successfully
	And with values
		| FieldName    | Operation | ValueOne | ValueTwo |
		| CustomerName | Equal     | Abed     |          |