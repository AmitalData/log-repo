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
            | Name         | TextVar |
            | DataType     | Text    |
            | DefaultValue | random  |
        And declare another primitive variable with following details
            | Name         | NumberVar |
            | DataType     | Number    |
            | DefaultValue | 120       |
        When save flow
        Then the flow should save successfully

    Scenario: assign primitive variable
        Given add assign element with following details
            | Name | Assign variables |
        And assign primitive varaibles with following details
            | VariableName | Operation         | Value        | FromList |
            | TextVar      | Equals <constant> | random       | False    |
            | NumberVar    | Equals <field>    | Packages QTY | True     |
        When save flow
        Then the flow should save successfully

    Scenario: assign primitive variable to different datatype
        Given add second assign element with following details
            | Name | Assign variables2 |
        When assign primitive varaibles in second assignment with following details
            | VariableName | Operation      | Value           | FromList |
            | NumberVar    | Equals <field> | Account Manager | True     |
        Then the field should not appear
