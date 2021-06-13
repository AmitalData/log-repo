import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentContext } from '../../models/ShipmentContext';
import { MainCarriageLeg } from "cypress/models/MainCarriageLeg";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
//import { when } from "cypress/types/jquery";

//#region variables
let MasterShipmentDetails: ShipmentDetails;

let EventNote;
let shipmentNumber: string;
//#endregion

//#region Create master export air shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a master Shipment with following details", (dataTable) => {
    MasterShipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    Actions.OpenNewShipmentWizard(MasterShipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(MasterShipmentDetails);
});

When("create master", () => {
    Actions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
});

Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.MasterNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Create house export air shipment inside the master
Given("the user in the master's Shipment tab", () => {
    Actions.OpenShipment(ShipmentContext.MasterNumber);

    cy.Navigate(ShipmentSelectors.ShipmentsTab);
});

When("create house with {string} as Shipper", (Shipper) => {
    Actions.CreateNewAttachedHouse(Shipper);
});

Then("the house should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.HouseNumber = interception.response.body.House;
    })
});

Then("the house should connect successfully", () => {
    Actions.CheckBusyIndicator()
    Actions.ValidateCheckHouseCheckBox();
});
//#endregion
//#region Update ActualFinalArrivalDate when there is actual date
Given("the user in the master's rounting tab", () => {
    cy.Navigate(ShipmentSelectors.RoutingsTab);
});


Given("edit main carriage leg with the following details", (dataTable) => {
    let mainCarriageLeg = Assists.CreateInstance<MainCarriageLeg>(dataTable, true);
    Actions.EditMainCarriageLegsAddETAandATA(mainCarriageLeg.MainCarriageETADate,mainCarriageLeg.MainCarriageATADate);
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
    cy.Click(ShipmentSelectors.ShipmentSaveButton, null);
});
When('update master',()=>{
    Actions.UpdateMaster()
})
Then('the master should update successfully',()=>{
    //Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
})
Then('the master ActualFinalArrivalDate should be 2021-05-04',()=>{
    Actions.AsserationAddMainCarriageETAandATADate()
})
Then('the house ActualFinalArrivalDate should be 2021-05-04',()=>{
  cy.BackButton('Operations')
  cy.get('#Shipments-O-Q').click()
  cy.get('#row0col13').contains('04/05/2021').should('exist')
  cy.BackButton('Operations')
  Actions.OpenShipment(ShipmentContext.MasterNumber)
})
//#endregion

//#region Update ActualFinalArrivalDate when there is no actual date
Given('the user in the master rounting tab',()=>{
    Actions.OpenShipment(ShipmentContext.MasterNumber)
    cy.Navigate(ShipmentSelectors.RoutingsTab);
})
Given('edit main carriage leg with the following details',(dataTable)=>{
    let mainCarriageLeg = Assists.CreateInstance<MainCarriageLeg>(dataTable, true);
    Actions.EditMainCarriageLegsAddETAandATA(mainCarriageLeg.MainCarriageETADate,mainCarriageLeg.MainCarriageATADate);
    cy.Click(ShipmentSelectors.ShipmentSaveButton, null);
})
When('update master',()=>{
    Actions.UpdateMaster()
})
Then('the master should update successfully',()=>{
    //Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
})
Then('the master ActualFinalArrivalDate should be null',()=>{
    Actions.AsserationMastershipmentNOActualFinalArrivalDate()
})
Then('the house ActualFinalArrivalDate should be null',()=>{
  Actions.AsserationHouseshipmentNOActualFinalArrivalDate()
})
//#endregion

//#region Update FinalArrivalDate when there are transshipments
Given('the user in the master rounting tab',()=>{
 cy.BackButton('Operations')
  Actions.OpenShipment(ShipmentContext.MasterNumber)
  cy.Navigate(ShipmentSelectors.RoutingsTab);


})
Given('edit main carriage leg with the following details',(dataTable)=>{
    let mainCarriageLeg = Assists.CreateInstance<MainCarriageLeg>(dataTable, true);
    //Actions.EditMainCarriageLegsAddETAandATA(mainCarriageLeg.MainCarriageETADate,mainCarriageLeg.MainCarriageATADate);

    Actions.FillMainCaarriageofshipmentandtransshipments(mainCarriageLeg.Transshipment1FromPortId,mainCarriageLeg.Transshipment1ETA,
    mainCarriageLeg.Transshipment1ATA,mainCarriageLeg.MainCarriageETADate,mainCarriageLeg.MainCarriageATADate)


   // cy.Click(ShipmentSelectors.ShipmentSaveButton, null);
})
//#endregion
