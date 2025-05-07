@smoke
Feature: Cargo Serial Data
    The user insert details to Cargo Serial Data section

    Scenario: Insert and save details in Cargo Serial Data section
        Given the user logged in and navigates to Export workspace
        And Filter for בדיקות אוטומטיות - לא לגעת 
        | SearchField | בדיקות אוטומטיות - לא לגעת |
           

    Scenario: Fill Cargo Serial Data with details
        Given fill Cargo Serial Data with the following details

            | TypeOfQuantity |  כמות אריזות במחסן  |
            | PackagingType | Piece |
            | Quantity |   10 |
            | Weight   |   10 |
            | PackingWeightCode | כל אחד|
            | SignsAndNumbers | Aa123456 |

        When pushing the save button 
        Then the save button will change to Pale Blue


        Scenario: Delete Row
           Given the user delete the row 
           When pushing the save button 
           Then there is no row in the grid    
		

   