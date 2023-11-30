@devrelease
Feature: Overview task Create, and Edit from CRM
    The user creates a task, and complete it #, and creates a new post from the CRM Module.

    Scenario: Create new task 
        Given the user logged in and open Overview in CRM
        And navigate task wizerd and fill the following details
            | Subject      | CurrentDate          |
            | Description  | new description      |
            | PriorityCode | Normal               |
        When create task
        Then the task should create successfully

    Scenario: Mark a task as complete from Upcoming Activities list
        When press on Complete button
        Then the task should get update


    Scenario: Go into the task from Daily Spotlight list
        When press on redirect button from today column
        Then the task should opend


    Scenario: Create new post 
        Given navigate post and fill the following details
        When create post
        Then the post should create successfully

    Scenario: Create new message 
        Given navigate message and fill the following details
        When create new message
        Then the message should create successfully