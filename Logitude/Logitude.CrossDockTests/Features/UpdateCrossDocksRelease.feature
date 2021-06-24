@Pre-Prepare
Feature: Update Cross Docks Release
	We want to update cross docks release.

Scenario: update release cross dock
	Given CustomerRef1 'customer1test' and House 'houseTest'
	And release cross dock
	When update release cross dock
	Then the release cross dock should update successfully