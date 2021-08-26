@Pre-Prepare
Feature: Get Container Status Request
	With pre-prepared FCL shipment container data
	We want to get container status.

Scenario: Create container status request
	When get container status request
	Then get request status OK

Scenario: Check the added communication logs
	When  the communication logs added
	Then the communication logs status should be "Waiting" or "Done"

Scenario: Check the communication logs status after 2 minutes
	Given waiting time is 2 minutes
	When  after the communication logs added
	Then the communication logs status should be or "Done"