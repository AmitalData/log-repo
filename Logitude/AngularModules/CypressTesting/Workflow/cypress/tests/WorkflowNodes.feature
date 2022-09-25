Feature: Workflow node
    The user open workflow list, open a flow, edit start configration, and save.

    Scenario: create workflow
        Given the user logged in and navigates to automation workspace
        And open workflows
        And a flow with following details
            | Name | random |
        When create workflow
        Then the flow should create successfully

    Scenario: edit workflow general inforamtion
        Given edit workflow general inforamtion with following details
            | Name | random |
        When save workflow
        Then the workflow should update successfully

    Scenario: open workflow run history
        When open run history
        Then the instances should appear successfully

    Scenario: edit start configration
        Given edit start configration with following details
            | Object           | Shipment            |
            | ConfigureTrigger | A record is updated |
        And add condition with following details
            | Field     | Custom Lookup           |
            | Operation | Equals                  |
            | Value     | TestAgentExport Contact |
        And add condition group met with 'And' with the following details
            | Field         | Operation  | Value                   |
            | Custom Lookup | Not Equals | TestAgentExport Contact |
            | Custom text   | Contains   | 33                      |
            | Custom Bool   | Equals     | False                   |
            | Custom NText  | Changed    | False                   |
        When save flow
        Then the flow should save successfully

