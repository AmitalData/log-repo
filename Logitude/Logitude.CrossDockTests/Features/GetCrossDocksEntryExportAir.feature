@Pre-Prepare-Entry
Feature: Get Cross Docks Entry Export Air
	The API retrieves entry export air cross dock.

Scenario: Get Cross Docks Entry Export Air
	When get cross docks with CrossDockId
	Then cross dock should be avaliable