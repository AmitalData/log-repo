@Pre-Prepare-Entry
Feature: Update Cross Docks
	We want to update cross docks.

Scenario: update entry cross dock
	Given a packages with the following properties
		| Quantity | Length | Width | Height | Weight |
		| 70       | 10     | 20    | 30     | 40     |
	And entry cross dock
	When update entry cross dock
	Then the entry cross dock should update successfully