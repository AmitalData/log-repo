#@devrelease
Feature: Customization New Rule
    The user open customization, add new rule, create new shipment, test the rule works in the shipment, inactivate the rule

    Scenario:Create New Rule From Customization with Entity Validation as RuleType 
        Given the user logged in and choose customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And adds new rule with the following details
            | Code                   | Code              |
            | Name                   | AutomationRule    |
            | RuleType               | Entity Validation |
            | TriggerType            | Condition         |
            | NotificationType       | Error             |
            | ActiveForNew           | true              |
            | ActiveForUpdate        | true              |
            | RuleCondition          | Customer          |
            | ConditionFieldValue    | TestShipperExport |
            | RuleField              | Customer Ref 1    |
            | ResultNotificationType | Error             |
        When create rule
        Then the rule should created successfully

    Scenario:Search and edit the created New Rule From Customization
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And search the rule already created 
        When inactivate the rule and save
        Then the rule should saved successfully

    Scenario:Create New Rule From Customization with Field Duplication as RuleType
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And adds new rule with the following details
            | Code                   | Code              |
            | Name                   | AutomationRule    |
            | RuleType               | 	Field Duplication |
            | TriggerType            | Condition         |
            | NotificationType       | Error             |
            | ActiveForNew           | true              |
            | ActiveForUpdate        | true              |
            | RuleCondition          | Customer          |
            | ConditionFieldValue    | TestShipperExport |
            | RuleField              | Customer Ref 1    |
            | ResultNotificationType | Error             |
        When create rule
        Then the rule should created successfully

    Scenario:Search and edit the created New Rule From Customization
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And search the rule already created 
        When inactivate the rule and save
        Then the rule should saved successfully

    Scenario:Create New Rule From Customization with Block Field as RuleType
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And adds new rule with the following details
            | Code                   | Code              |
            | Name                   | AutomationRule    |
            | RuleType               | 	Block Field |
            | TriggerType            | Condition         |
            | ActiveForNew           | true              |
            | ActiveForUpdate        | true              |
            | RuleCondition          | Customer          |
            | ConditionFieldValue    | TestShipperExport |
            | RuleField              | Customer Ref 1    |
            | ResultNotificationType | Error             |
        When create rule
        Then the rule should created successfully

    Scenario:Search and edit the created New Rule From Customization
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And search the rule already created 
        When inactivate the rule and save
        Then the rule should saved successfully
        
    Scenario:Create New Rule From Customization with Required RuleType
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And adds new rule with the following details
            | Code                   | Code              |
            | Name                   | AutomationRule    |
            | RuleType               | Required          |
            | TriggerType            | Condition         |
            | NotificationType       | Error             |
            | ActiveForNew           | true              |
            | ActiveForUpdate        | true              |
            | RuleCondition          | Customer          |
            | ConditionFieldValue    | TestShipperExport |
            | RuleField              | Customer Ref 1    |
            | ResultNotificationType | Error             |
        When create rule
        Then the rule should created successfully

    Scenario: Create direct export air shipment to verify rule execution on create
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the rule with required field result should executed successfully and show the "Customer Ref 1 Field is Required"

    Scenario:Search and edit the created New Rule From Customization
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And search the rule already created 
        When inactivate the rule and save
        Then the rule should saved successfully

    Scenario:Create New Rule From Customization
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And adds new rule with the following details
            | Code                   | Code              |
            | Name                   | AutomationRule    |
            | RuleType               | Required          |
            | TriggerType            | Condition         |
            | NotificationType       | Error             |
            | ActiveForNew           |             |
            | ActiveForUpdate        | true              |
            | RuleCondition          | Customer          |
            | ConditionFieldValue    | TestShipperExport |
            | RuleField              | Customer Ref 1    |
            | ResultNotificationType | Error             |
        When create rule
        Then the rule should created successfully 

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the direct should create successfully
        
    Scenario: Update shipment to verify the rule exection on update
        Given the user fill "100" as ValueOfGoods and "MTA" as a MoveType
        When update shipment
        Then the rule with required field result should executed successfully and show the "Customer Ref 1 Field is Required"

    Scenario:Search the created New Rule From Customization
        Given the user navigate to customization
        And the user choose "Shipment" Object
        And the user clicks on Rules
        And search the rule already created 
        When inactivate the rule and save
        Then the rule should saved successfully