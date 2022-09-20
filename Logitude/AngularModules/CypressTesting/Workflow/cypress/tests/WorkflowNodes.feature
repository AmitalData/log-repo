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
        And add condition with following details
            | Field     | Main Carriage Final ATA |
            | Operation | Less Than               |
            | Value     | TODAY                   |
        And add condition group met with 'Or' with the following details
            | Field                | Operation | Value               |
            | Profit Differences   | Not Equal | Profit Differences1 |
            | Containers Numbers   | Equal     | 5                   |
            | Agent                | Equal     | TestAgentExport     |
            | Description of Goods | Ends With | xyz                 |
        When save flow
        Then the flow should save successfully

    Scenario: add secound level group in start configration
        Given add second level condition group met with 'And' with the following details
            | Field            | Operation    | Value  |
            | Create Date      | Greater Than | random |
            | Chargable Weight | Less Than    | 3.5    |
        When save flow
        Then the flow should save successfully