@smoke
Feature: Search Import Declarations
    Verify that the search field correctly filters import declarations by different criteria

    Background:
        Given the user is logged in
        And navigates to Import Declarations workspace

    Scenario: STEP01 - Search by File Number
        When the user enters file number "51840032" in the search field
        Then the system displays only files matching "51840032"
        And the displayed file number matches the search term

    Scenario: STEP02 - Search by Customer Name
        When the user clears the search field
        And the user enters customer name "שריה" in the search field
        Then the system displays only files with customer "שריה"
        And all displayed files have customer names containing the search term

    Scenario: STEP03 - Search by Declaration Number
        When the user clears the search field
        And the user enters declaration number "24023598220501" in the search field
        Then the system displays only files matching declaration "24023598220501"
        And the displayed declaration number matches the search term

