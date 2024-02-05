@smoke
Feature: Declaration Form
    The user Recieved Declaration

    Scenario: Search for file
        Given the user logged in and navigates to Import workspace
        And Search for File 
            | File  | 51340538 |


   When the user click on Forms > Declaration Form
   Then the system should display the declaration that already exist

   Scenario: End


  



       