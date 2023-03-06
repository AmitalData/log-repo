@devTest
Feature: Workflow Create and Edit
    The user open workflow list, create and edit a flow .

    Scenario: create a flow
        Given the user logged in and navigates to automation workspace
        And open workflows list
        And a flow with following details
            | Name        | random        |
            | Description | test workflow |
        When create flow
        Then the flow should create successfully

    Scenario: update a flow
        Given edit start configration with following details
            | Object           | Shipment            |
            | ConfigureTrigger | A record is updated |
        Given edit workflow general inforamtion with following details
            | Name        | random        |
            | Description | test workflow |
        When update flow
        Then the flow should update successfully
