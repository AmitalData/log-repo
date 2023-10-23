@smoke
Feature: Locat Declaration Search Field
    The user Search for Declaration

    Scenario: Search for Declaration
        Given the user logged in and navigates to Import workspace
        And Search for Declaration 
            | Declaration  |  23042888681893 |


        When compare to filed Declaration
        Then the comparison succeeded

    Scenario: End

    