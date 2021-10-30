@stable @all @smoke @smoke3
Feature: Opportunity Create, Search, and Edit from CRM
    The user creates an Opportunity, searches for and edits it from the CRM Module.

    Scenario: Create export air quote
        Given the user logged in and navigates to quotes workspace
        And a quote with the following details
            | Direction            | Export |
            | TransportMode        | Air    |
            | Shipper              | 70000  |
            | MainCarriageFromPort | LHR    |
            | MainCarriageToPort   | MIA    |
        When create quote
        Then the quote should create successfully

    Scenario: Create new Opportunity
        Given the user navigate Opportunity workspace and fill the following details
            | OpportunityType | New Business |
            | Subject         | CurrentDate  |
            | Customer        | 70000        |
        When create Opportunity
        Then the Opportunity should create successfully

    Scenario: Search for the Opportunity by subject
        When search Opportunity
        Then the Opportunity should appear successfully

    Scenario: Open the Opportunity
        When open Opportunity
        Then the Opportunity should open successfully

    Scenario: Add new task
        Given navigate task wizerd and fill the following details
            | Subject      | CurrentDate     |
            | Description  | new description |
            | PriorityCode | Normal          |
        When create task
        Then the task should create successfully

    Scenario: mark the task as complete
        When press on Complete button
        Then the task should get complete

    Scenario: add Competitors
        Given add Competitor
        When save Opportunity
        Then the Opportunity should update successfully

    Scenario: remove Competitors
        Given remove Competitor
        When save Opportunity
        Then the Opportunity should update successfully

    Scenario: add Additional Services
        Given add Additional Service
        When save Opportunity
        Then the Opportunity should update successfully

    Scenario: remove Additional Services
        Given remove Additional Service
        When save Opportunity
        Then the Opportunity should update successfully

    Scenario: add export air quote from inside the Opportunity
        Given the user navigates to quotes workspace from inside the Opportunity
        And the user fill a quote with the following details
            | Direction            | Export |
            | TransportMode        | Air    |
            | MainCarriageFromPort | LHR    |
            | MainCarriageToPort   | MIA    |
        When create quote from inside the Opportunity
        Then the quote should create successfully
        And the quote should connect successfully

    Scenario: connect Quote
        Given the user choose a quote to connect it to the Opportunity
        When connect quote
        And the quote should connect successfully

    Scenario: Edit general tab
        Given fill "new edit" as description
        When save Opportunity
        Then the Opportunity should update successfully
