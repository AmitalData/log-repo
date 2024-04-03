import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CargoSerialDataActions from '../../actions/CargoSerialDataActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { CargoSerialDataDetails } from '../../models/CargoSerialDataDetails';
import { CargoSerialDataSelectors } from '../../selectors/CargoSerialDataSelectors';
var cargoSerialDataDetails

		
//#region Fill Cargo Serial Data with details
    Given("the user logged in and navigates to Export workspace", () => {
    cy.Login();
    Actions.NavigatesExportsWizerd()
     });

// פילטר לקבלת תיקים לפי שם לקוח וככניסה לתיק הראשון בתוצאות

     Given("Filter for בדיקות אוטומטיות - לא לגעת", (dataTable) => {
        cargoSerialDataDetails = Assists.CreateInstance<CargoSerialDataDetails>(dataTable, true);
        CargoSerialDataActions.SearchFields(cargoSerialDataDetails)

    });
        
        
    Given("fill Cargo Serial Data with the following details", (dataTable) => {
            let cargoSerialDataDetails = Assists.CreateInstance<CargoSerialDataDetails>(dataTable, true);
            CargoSerialDataActions.FillCargoSerialData(cargoSerialDataDetails)
            
        });
        
    When("pushing the save button", () => {
            cy.Click(CargoSerialDataSelectors.SaveButton, null)
        });
        
    Then("the save button will change to Pale Blue", () => {
            CargoSerialDataActions.AssertSaveCargoSerialData()
        });

    Given("the user delete the row", () => { 
        CargoSerialDataActions.DeleteRow()

        });

    When("pushing the save button", () => {
            cy.Click(CargoSerialDataSelectors.SaveButton, null)
        });

    Then("there is no row in the grid", () => {
            CargoSerialDataActions.AssertDeleteRow()
        });   
        
    //#endregion
        