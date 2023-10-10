@smoke
Feature: Locat Customer Search Field
    The user Search for Customer

    Scenario: Search for Customer
        Given the user logged in and navigates to Import workspace
        And Search for Customer 
            | Customer  | שריה |

        When compare to filed Customer
        Then the comparison succeeded

    Scenario: End




    