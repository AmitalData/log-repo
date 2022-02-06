import * as Actions from "../../../actions/Actions";
import * as StandaloneAction from "../../../actions/StandaloneAction";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../../models/ShipmentDetails";
import { PickupDelivaryDetails } from "../../../models/PickupDelivaryDetails";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../selectors/Selectors"
import { RestAPI } from '../../../../../Base/cypress/constants/RestAPI';
import { URLs } from '../../../constants/URLs';
import { BaseSelectors } from '../../../../../Base/cypress/selectors/BaseSelectors';
import { ShipmentConstants } from "../../../constants/constants";

let shipmentDetails: ShipmentDetails;
let PickupDelivarytData: PickupDelivaryDetails;
let shipmentNumber: string;

//#region create direct shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
  });
  
  Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
    cy.FillLogLov(ShipmentSelectors.ShipmentShipper, shipmentDetails.Shipper, true)
  });
  
  When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
  });
  
  Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
      shipmentNumber = interception.response.body.ShipmentNumber
    })
  });
  //#endregion

  //#region Add Delivary
  Given("the user open the shipment and navigate to RoutingsTab workspace", () => {
    Actions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    cy.Click(ShipmentSelectors.AddDelivery, null)
    cy.Click(BaseSelectors.Button, ShipmentConstants.AddDelivery)
  });
  
  Given("add a new Delivery leg with the following details", (dataTable) => {
    PickupDelivarytData = Assists.CreateInstance<PickupDelivaryDetails>(dataTable, true);
    StandaloneAction.FillPickUpDelivaryDetails(PickupDelivarytData);
  });  
  
  When("save the Delivery", () => {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(BaseSelectors.SaveButton + BaseSelectors.LastElement, null)
  });

  Then("the direct should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
  });
//#endregion

//#regionThe can't add new packages in delivery
  Given("the user in the packages tab",()=>{
    cy.Click(ShipmentSelectors.PickUpDeliveryPackages, null)
  })

  When("click add container",()=>{
    cy.Click(ShipmentSelectors.AddContainerFromDelivary, null, true)
  })

  Then("the Blus button disappear in window",()=>{
    BaseAssertion.AssertElementNotHaveClass(BaseSelectors.LogitudeIconButton,null)
  })