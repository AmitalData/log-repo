@smoke
Feature: Is Changed2
    The user send declaration to customs

    Scenario: Send declaration to customs
        Given the user logged in and navigates to Import workspace
        And Search for File 
             | File  | 51340545 |

        When send to customs 
             | Scen  | הצהרת יבוא תקינה  |

        Then the declaration should reset 
             | Scen1  | הצהרת יבוא תקינה  | 
       

    Scenario: Change in Cargo serial data
        Given the user change the Cargo serial data
            | Quantity | 2 |
        When save the declaration
        Then the changes should saved successfully


    Scenario: Send Declaration to Payment
        When the screen of Declaration to Payment is open
        Then the Button Save in the Declaration Payment screen should be disabled successfully "be.disabled"

       