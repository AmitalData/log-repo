@devrelease
Feature: Customization New Rule
    The user open customization, add new rule, create new shipment, test the rule works in the shipment, inactivate the rule

    Scenario:Create New Rule From Customization
        Given the user logged in and choose customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And adds new rule with the following details
            | Code                   |    5555       |
            | Name                   | AutomationRule |
            | RuleType               | Required       |
            | TriggerType            | Condition      |
            | NotificationType       | Error          |
            | ActiveForNew           | true           |
            | ActiveForUpdate        | true           |
            | RuleCondition          | Customer       |
            | ConditionFieldValue    |                |
            | RuleField              | Customer Ref 1 |
            | ResultNotificationType | Error          |
        When create rule
        Then the rule should created successfully
