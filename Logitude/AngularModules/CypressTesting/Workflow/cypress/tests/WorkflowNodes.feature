Feature: Workflow node
    The user open workflow list, open a flow, edit start configration, and save.

    Scenario: create workflow
        Given the user logged in and navigates to automation workspace
        And open workflows
        And a flow with following details
            | Name | random |
        When click create
        Then the flow should create successfully

    Scenario: edit start configration
        Given edit start configration with following details
            | Object           | Shipment            |
            | ConfigureTrigger | A record is updated |
        And click Ok
        When click save
        Then the flow should save successfully

