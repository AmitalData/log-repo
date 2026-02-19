@regression @bug118310
Feature: Regression Bug 118310 - Column Header Integrity
    Verify column headers in Export/Import lists are displayed correctly (not Base64 encoded)

    Scenario: Export declaration list headers are displayed correctly
        Given the user is logged in to the system
        When the user navigates to Export workspace
        Then the grid should be visible with data
        And column headers should be visible in the list

    Scenario: Import declaration list headers are displayed correctly
        Given the user is logged in to the system
        When the user navigates to Import workspace
        Then the grid should be visible with data
        And column headers should be visible in the list

