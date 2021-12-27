@devrelease
Feature: Automations Create, and Edit from Maintenance
    The user creates an Automations on update and on create,and edits it from the Maintenance Module.

    Scenario: Create new automation onCreate with set field value result
        Given the user logged in
        And the user navigates to  "Automations" in maintenance menu
        And the user select "Masters" entity
        And the user try to add automation onCreate with the following detailes
            | Name             | master automation |
            | Condition1       | customer          |
            | Condition1Value  | sondos            |
            | Condition2       | Accounting Closed |
            | Condition2Value  | false             |
            | ResultType       | Set Fields Value  |
            | resultfield      | Agent             |
            | resultfieldvalue | Agent             |
        When the user save the new automatiion
        Then the new automation should createed successfully


    Scenario: Edit an automation

        Given the user open an automation from "Masters" entity as following
        And edit it
        When update Automation
        Then the automation should updated successfully