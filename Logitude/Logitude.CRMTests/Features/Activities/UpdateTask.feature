@Pre-Prepare-Activity-Task
Feature: Update Task
	We want to update task.

Scenario: Update task
	Given task
	And following task properties
		| property      | Value                 |
		| Subject       | updated specflow sub  |
		| Description   | updated specflow desc |
		| StartDateTime | 2021-09-15 14:40      |
		| DueDate       | 2021-12-15 14:40      |
		| Priority      | Low                   |
	When update task
	Then the task should update successfully