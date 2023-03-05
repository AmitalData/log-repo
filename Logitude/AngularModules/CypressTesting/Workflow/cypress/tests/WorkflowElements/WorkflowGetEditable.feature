@devTest
Feature: Workflow node
    The user open workflow list, open a flow, edit start configration, declare primitive variable, and declare record variable .

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

    Scenario: add editable shipment record
        Given add editable record with following details
            | Name         | Editable shipment   |
            | RecordType   | Editable records    |
            | Object       | Shipment            |
            | FilterRecord | From Trigger record |
        When save flow
        Then the flow should save successfully