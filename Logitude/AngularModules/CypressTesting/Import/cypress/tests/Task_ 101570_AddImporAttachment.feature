@smoke
Feature: Add Import Attachment
    The user Add Import Attachment

    Scenario: Add Import Attachment
        Given the user logged in and navigates to Import workspace
        And Search for file and enter to Customs Attachments 

        | File | 51340617 |
           

    Scenario: Add new attachment
        Given Add new attachment

        | SearchFieldsId | 51340617 |
           
            

    Scenario: Delete Attachment
        Given the user delete the Attachment 
        When  disconected the Attachment
        Then  the ticket will be empty  
        

   