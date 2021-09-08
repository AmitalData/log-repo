@Pre-Prepare-OceanInsight
Feature: Get Container Details
	With pre-prepared FCL shipment container data
	We want to get container details.

Scenario: Get container details
	Given Read the file "GetContainerStatusesFeatureData.xml" Data Response
	When get container details request
	Then container details should be change successfully
