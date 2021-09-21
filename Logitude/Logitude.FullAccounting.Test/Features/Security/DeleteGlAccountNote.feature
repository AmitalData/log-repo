@Pre-Prepare-CreateNote
Feature: Delete GlAccount Note
	we want to post delete gl account note by not authorize user.

Scenario: Get post delete gl account note by not authorize user.
	When get post delete gl account note
	Then The post delete note API should return you have no permissions