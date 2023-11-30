Feature: Run Report
	We want to run report.

Scenario: Run Report
	Given report with the following properties
		| property | Value                      |
		| Code     | OSBC                       |
		| Name     | Open Shipments by Customer |
		| Template | open shipment by customer  |
	And filter fields
		| Name     | Value        | Map        |
		| Customer | TestCustomer | CustomerId |
	When run report
	Then the report should run successfully
	And with values
		| FieldName                           | Operation | ValueOne | ValueTwo |
		| SelectedCustomerName                | Equal     | Ahmed    |          |
		| OpenShipmentsRecord.Open.Id         | LessThan  | 100      |          |
		| OpenShipmentsRecordList.ShipperName | Equal     | Ahmed    |          |