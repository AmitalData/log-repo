@devTest
Feature: Workflow node
    The user open workflow list, open a flow, edit start configration, and save.

    Scenario: create workflow
        Given the user logged in and navigates to automation workspace
        And open workflows
        And a flow with following details
            | Name        | random        |
            | Description | test workflow |
        When create workflow
        Then the flow should create successfully

    Scenario: edit start configration
        Given edit start configration with following details
            | Object           | Shipment            |
            | ConfigureTrigger | A record is updated |
        When save flow
        Then the flow should save successfully

    Scenario: declare primitive variable
        Given declare a primitive variable with following details
            | Name         | random |
            | DataType     | Text   |
            | DefaultValue | random |

        When save flow
        Then the flow should save successfully

    Scenario: declare record variable
        And declare a Record variable with following details
            | Name     | random  |
            | DataType | Record  |
            | Object   | Package |
        When save flow
        Then the flow should save successfully
