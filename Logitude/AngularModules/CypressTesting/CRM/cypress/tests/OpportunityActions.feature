@stable @all @smoke @smoke3
Feature: Opportunity Actions
    The user creates an Opportunity, close it as won, reopen, close it as lost and copy

    Scenario: Create new Opportunity
        Given the user logged in and navigates to opportunity workspace
        Given fill the following details
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

    Scenario: Close as won
        Given open "Close As Won" action
        When close as won
        Then opportunity stage status should be "Closed Won"
        And following events should appear in events tab
            | Event         | Notes |
            | Closed As Won | Won   |

    Scenario: Reopen
        Given open "ReOpen" action
        When reopen with "Development" as satge
        Then opportunity stage status should be "Development"
        And following events should appear in events tab
            | Event                 | Notes                                        |
            | Opportunity re-opened |                                              |
            | Stage Changed         | Stage changed from Closed Won to Development |

    Scenario: Close as lost
        Given open "Close As Lost" action
        When close as lost with "Lost" as closing reason
        Then opportunity stage status should be "Closed Lost"
        And following events should appear in events tab
            | Event          | Notes |
            | Closed As Lost | Lost  |

    Scenario: Copy opportunity
        Given open "Copy" action
        When create Opportunity
        Then the Opportunity should create successfully