@smoke
Feature: Add Export Attachment
    The user Add Export Attachment

    Scenario: Add Export Attachment
        Given the user logged in and navigates to Export workspace
        And Search for file and enter to Customs Attachments 

        | File | 4579 |
           

    Scenario: Add new attachment
        Given Add new attachment

            | CustomsDocId | 919012394 |
            | IssueDate    | TODAY |
            

    Scenario: Delete Attachment
        Given the user delete the Attachment 
        When  disconected the Attachment
        Then  the ticket will be empty  
        

   