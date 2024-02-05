@smoke
Feature: Locat Second Cargo ID Search Field
    The user Search for Second Cargo ID

    Scenario: Search for Second Cargo ID
        Given the user logged in and navigates to Import workspace
        And Search for Second Cargo ID 
            | SecondCargoID | I858585666 |

        When compare to filed Second Cargo ID
        Then the comparison succeeded

        Scenario: End

    