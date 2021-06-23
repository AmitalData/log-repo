@Pre-Prepare
Feature: Update Cross Docks
	We want to update cross docks.

Scenario: update entry cross dock
	Given a package with the following properties
		| property | Value |
		| Quantity | 200   |
		| Length   | 100   |
		| Width    | 100   |
		| Weight   | 200   |
		| Height   | 100   |
	And entry cross dock
	When update entry cross dock
	Then the entry cross dock should update successfully