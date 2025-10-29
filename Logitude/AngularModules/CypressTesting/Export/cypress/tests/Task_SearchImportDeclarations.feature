@smoke
Feature: Search Import Declarations
    Verify that the search field correctly filters import declarations by different criteria

    Background:
        Given the user is logged in
        And navigates to Import Declarations workspace

    Scenario: STEP01 - Search by File Number
        When the user enters file number "51340479" in the search field
        Then the system displays only files matching "51340479"
        And the displayed file number matches the search term

    Scenario: STEP02 - Search by Customer Name
        When the user clears the search field
        And the user enters customer name "שריה" in the search field
        Then the system displays only files with customer "שריה"
        And all displayed files have customer names containing the search term

    Scenario: STEP03 - Search by Declaration Number
        When the user clears the search field
        And the user enters declaration number "25043848690453" in the search field
        Then the system displays only files matching declaration "25043848690453"
        And the displayed declaration number matches the search term

    Scenario: STEP04 - Search by Cargo Identifier
        When the user clears the search field
        And the user enters cargo identifier "ZRH0190728" in the search field
        Then the system displays only files with cargo identifier "ZRH0190728"
        And all displayed files contain the cargo identifier in their data

