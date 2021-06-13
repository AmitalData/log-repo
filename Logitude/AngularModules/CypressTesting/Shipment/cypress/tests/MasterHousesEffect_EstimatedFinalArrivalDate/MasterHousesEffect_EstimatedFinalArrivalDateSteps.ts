import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentContext } from '../../models/ShipmentContext';
import { MainCarriageLeg } from "cypress/models/MainCarriageLeg";
//#region variables
let MasterShipmentDetails: ShipmentDetails;
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
//#region Update EstimatedFinalArrivalDate when there is actual date
Given("the user in the master's rounting tab", () => {
    cy.Navigate(ShipmentSelectors.RoutingsTab);
});

Given("edit main carriage leg with the following details", (dataTable) => {
    let MainCarriageLeg = Assists.CreateInstance<MainCarriageLeg>(dataTable, true);
    Actions.EditMainCarriageDetails(MainCarriageLeg)
    });
When('update master',()=>{
    Actions.UpdateMaster()
})
Then('the master should update successfully',()=>{
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
})
Then('the master EstimatedFinalArrivalDate should be 2021-05-02',()=>{
    Actions.AsserationEstimatedFinalArrivalDateinMaster()
})
Then('the house EstimatedFinalArrivalDate should be 2021-05-02',()=>{
   Actions.AsserationEstimatedFinalArrivalDateinHouse()  
}) 
//#endregion
//#region Update EstimatedFinalArrivalDate when there are transshipments

Given('the user in master rounting tab',()=>{
    cy.BackButton('Operations')
    Actions.OpenShipment(ShipmentContext.MasterNumber)
    cy.Navigate(ShipmentSelectors.RoutingsTab);
})

Then('the master EstimatedFinalArrivalDate should be 2021-05-04',()=>{
Actions.AsserationEstimatedFinalArrivalDatesinMaster()

})
Then('the house EstimatedFinalArrivalDate should be 2021-05-04',()=>{
    Actions.AsserationEstimatedFinalArrivalDateHousetransshipments()
})
//#endregion
