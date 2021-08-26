@Pre-Prepare
Feature: Get Container Statuses
	With pre-prepared FCL shipment container data
	We want to get container status.

Scenario: Get container statuses
	Given Response "XML" Data
	When get container status request
	Then container Statuses should be change successfully
