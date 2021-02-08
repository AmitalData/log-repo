import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"

let ShipmentData: ShipmentDetails;

Given("the user logged in and navigates to shipments workspace", () => {
  // cy.Login()
  // cy.Click(BaseSelectors.OperationsMenu, null)
  // cy.Click(Selectors.ShipmentTab, null)
});

Given("a direct shipment with the following details",
  (dataTable) => {
  //  let shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
  //  ShipmentData = shipmentDetails;
  //  Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
  //  Actions.FillShipmentWizardsFields(ShipmentData);

   //let shipmentDetails = CreateInstance<ShipmentDetails>(dataTable, true);
   //cy.log(shipmentDetails.Shipper);

   let listOfShipmentDetails = CreateSet<ShipmentDetails>(dataTable);
   listOfShipmentDetails.forEach(element => {
     cy.log(element.ShipmentLevel + " - " + element.Direction + " - " + element.TransportMode);
   });
});

When("create shipment", () => {
  //Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
  //BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200);
});




function CreateInstance<T>(dataTable: any, isVerticalTable?: boolean): T{
  let dataTableModel: T;

  if(!isVerticalTable){
    dataTableModel = dataTable.hashes()[0] as T;
  }else{
    let tempDataTableModel = {};
    dataTable.raw().forEach((raw: any[]) => {
      tempDataTableModel[raw[0]] = raw[1];
    });
    dataTableModel = tempDataTableModel as T;
  }

  return dataTableModel;
}


function CreateSet<T>(dataTable: any): T[]{
  return dataTable.hashes() as T[];
}