Feature: Run Report
	We want to run report.

Scenario: Run Report
	Given report with the following properties
		| property | Value                      |
		| Code     | OSBC                       |
		| Name     | Open Shipments by Customer |
		| Template | open shipment by customer  |
	And filter fields
		| Name     | DisplayValue | DataType | EntityName | PropertyName | SearchBy | Value |
		| Customer | Abed         | String   | Card       | CustomerId   | Code     | 70000 |
	When run report
	Then the report should run successfully
	And with values
		| FieldName    | Operation | ValueOne | ValueTwo |
		| CustomerName | Equal     | Abed     |          |