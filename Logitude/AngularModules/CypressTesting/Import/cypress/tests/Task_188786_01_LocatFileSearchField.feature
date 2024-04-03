@smoke
Feature: Locat File Search Field
    The user Search for file

    Scenario: Search for file
        Given the user logged in and navigates to Import workspace
        And Search for File 
            | File  | 51340541 |

        When compare to filed Customs File
        Then the comparison succeeded

    Scenario: End

    