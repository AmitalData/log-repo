
@smoke
Feature: Export Currency rates in a batch
    The user Check Export Currency rates, choose dates and send to the customs

    Scenario: Check Export Currency rates in a batch
        Given the user logged in and navigates to Export workspace
        And fill Export Currency rates with the following details
            | FromDate        | Today |
            | ToDate          | Today |
            | CurrencyTypeId  | USD   |


        Scenario: Enter to Requests Sheets
        Given the user logged in and fill the following details  
        
            | ManageCustomsRequests  | שאילתא לשערי מטבע |
            | Request status         | All |


        When clicking the serche botton
        Then the request status will be Answer was analyze 

   