@smoke
Feature: IsChanged_SI
    The user send declaration to customs

    Scenario: Send declaration to customs
        Given the user logged in and navigates to Import workspace
        And Filter for Correct Draft Declaration 
            | DeclarationStatus  | 13 |

        When send to customs 
        | Scen  | הצהרת יבוא תקינה  |

        Then the declaration should reset 
        | Scen1  | הצהרת יבוא תקינה  | 


    Scenario: Change in SI - Field Total SI
        Given the user change the Total SI
            | TotalSI  | 300 |
       
        

    Scenario: Send Declaration to Payment
        When the screen of Declaration to Payment is open
        Then the Button Save in the Declaration Payment screen should be disabled successfully "be.disabled"

       