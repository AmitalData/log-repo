@Pre-Prepare
Feature: Get Cross Docks Release 
	The API retrieves release cross dock.

Scenario: Get Cross Docks Release
	When get release cross docks with CrossDockId
	Then release cross dock should be avaliable