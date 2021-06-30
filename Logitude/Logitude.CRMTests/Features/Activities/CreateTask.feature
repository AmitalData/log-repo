Feature: Create Task
	We want to create task.

Scenario: Create task
	Given a task with the following properties
		| property           | Value            |
		| Subject            | specflow sub     |
		| Description        | specflow desc    |
		| StartDateTime      | 2021-06-15 14:40 |
		| DueDate            | 2021-07-15 14:40 |
		| Priority           | Normal           |
	When create task
	Then the task should create successfully