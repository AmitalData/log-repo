import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentContext } from '../../models/ShipmentContext';
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { when } from "cypress/types/jquery";
let MasterShipmentDetails: ShipmentDetails;
let packagesDetails: PackagesDetails[]
//#endregion

//#region Create master export air shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a shipment with the following details", (dataTable) => {
    MasterShipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    Actions.OpenNewShipmentWizard(MasterShipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(MasterShipmentDetails);
});

When("create shipment", () => {
    Actions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.MasterNumber = interception.response.body.ShipmentNumber;
    })
});
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
Given('the user in the house package tab',()=>{
    Actions.openHouseShipment()
    cy.Navigate(ShipmentSelectors.ShipmentPackagesTab+ShipmentSelectors.LastElementShipment)


})
Given("add a package with the following details", (dataTable) => {
    cy.Navigate(ShipmentSelectors.ShipmenAddpackqges)
    packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.AddHousePackage( packagesDetails)
    cy.Click("#OkOceanPackage", null)
});
When('update shipment',()=>{

   cy.Navigate(ShipmentSelectors.ShipmenSavepackqges+ShipmentSelectors.LastElementShipment)
//ShipmenSavepackqges
})
Then('the shipment should update successfully',()=>{
Actions.UpdateMaster()
})
Then('the package should add successfully',()=>{
cy.get(ShipmentSelectors.FirstRowinpackages).contains('Animals').should('exist')
cy.get(ShipmentSelectors.FirstRowinpackages).contains('100').should('exist')
cy.get(ShipmentSelectors.FirstRowinpackages).contains('5').should('exist')

})
Given('the user in the master package tab',()=>{
    cy.BackButton('Shipment')
   cy.Navigate(ShipmentSelectors.PackagesTab)
})
Given('rebuild master containers by adding new container with the following details',(dataTable)=>{
    cy.Navigate(ShipmentSelectors.ShipmentPackagefromhouse)
    cy.Click(ShipmentSelectors.Addcontainer,'Add')
   cy.Click(ShipmentSelectors.Newcontainermaster,'New Container')
    packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.AddHousePackage( packagesDetails)
    cy.Click(ShipmentSelectors.TEST+ShipmentSelectors.LastElementShipment,'OK')
    
})
//edit-log-grid_0_20_3_0
When('Update the shipment',()=>{
    cy.Navigate(ShipmentSelectors.ConfirmWindowYes)
    cy.Click(ShipmentSelectors.TEST+ShipmentSelectors.LastElementShipment,'OK')
})
Then('the container should add successfully',()=>{
cy.get(ShipmentSelectors.FirstRowinpackages).contains('ContainerId').should('exist')
cy.get(ShipmentSelectors.FirstRowinpackages).contains('100').should('exist')


})
Then('the master container number should be ABCD1111117',()=>{
    cy.get('#edit-log-grid_0_20_3_0').contains('ABCD1111117').should('exist')
})
Given('the user in master package tab',()=>{
    cy.Navigate(ShipmentSelectors.PackagesTab)
})
Given('edit the continer number in master shipment',()=>{
    cy.Navigate(ShipmentSelectors.Editcontainernumberfrommaster)
    cy.Navigate(ShipmentSelectors.ShipmentPackageContainernumber).clear().type('DDDD88889')
   // cy.get('#ShipmentPackage_ContainerNumber').click().clear()
   cy.Click("#OkOceanPackage", null)

})
When('update the master shipment',()=>{
    cy.Navigate(ShipmentSelectors.ShipmentSaveButton+ShipmentSelectors.LastElementShipment)
})
Then('the container should change successfully',()=>{
    cy.get('#edit-log-grid_0_20_3_0').contains('DDDD88889').should('exist')
})
Then('the house container number should be DDDD88889',()=>{
    cy.Navigate(ShipmentSelectors.ShipmentsTab);
    Actions.openHouseShipment()
    cy.Navigate(ShipmentSelectors.ShipmentPackagesTab+ShipmentSelectors.LastElementShipment)
    cy.get('#edit-log-grid_0_40_3_0').contains('DDDD88889').should('exist')
    
})
