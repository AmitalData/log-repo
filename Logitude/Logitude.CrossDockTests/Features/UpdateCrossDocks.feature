@Pre-Prepare
Feature: Update Cross Docks
	We want to update cross docks.

Scenario: update entry cross dock
	Given a packages with the following properties
		| Quantity | Length | Width | Height | Weight |
		| 70       | 10     | 20    | 30     | 40     |
	And entry cross dock
	When update entry cross dock
	Then the entry cross dock should update successfully

Scenario: update release cross dock
	Given CustomerRef1 'customer1test' and House 'houseTest'
	And release cross dock
	When update release cross dock
	Then the release cross dock should update successfully