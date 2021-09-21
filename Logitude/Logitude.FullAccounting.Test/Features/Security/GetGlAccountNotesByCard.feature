@Pre-Prepare-CreateNote
Feature: Get Gl Account Notes By Card
	we want to get gl account notes by card and not authorize user.

Scenario: Get gl account notes by card and not authorize user.
	When get gl account notes by card
	Then the get notes by card API should return you have no permissions