import { ShipmentSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion"
import { AMANACStatusDetails } from "../models/AMANACStatusDetails";
import * as Actions from "../actions/Actions"

export function AssertAMANAandCustomsTransmissionsStatuses(Status: string){
    cy.Click(ShipmentSelectors.CustomsTab, null);
    BaseAssertion.AssertElementContain(ShipmentSelectors.StatusValue, Status);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsSendtoCustoms)
    BaseAssertion.AssertElementContain(ShipmentSelectors.CustomsTransmissionsStatusValue, Status);
    cy.Click(ShipmentSelectors.CloseCustomsTransmissions, null)
}

export function AssertAMANAandCustomsTransmissionsStatusDetails(amanacStatusDetails: AMANACStatusDetails){
    cy.Click(ShipmentSelectors.CustomsTab, null);
    var currentDate = Actions.FormatDate(amanacStatusDetails.LastSent);
    BaseAssertion.AssertElementContain(ShipmentSelectors.StatusValue, amanacStatusDetails.Status);
    BaseAssertion.AssertElementContain(ShipmentSelectors.StatusDate, currentDate);
    cy.get(BaseSelectors.LoggedUser).invoke('text').then(text => {
        var SentBy = text.replace(/\s/g, "");
        BaseAssertion.AssertElementContain(ShipmentSelectors.UserName, SentBy);
        })
    
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsSendtoCustoms);
    BaseAssertion.AssertElementContain(ShipmentSelectors.CustomsTransmissionsStatusValue, amanacStatusDetails.Status);
    BaseAssertion.AssertElementContain(ShipmentSelectors.CustomsTransmissionsStatusDate, currentDate);
    cy.get(BaseSelectors.LoggedUser).invoke('text').then(text => {
        var SentBy = text.replace(/\s/g, "");
        BaseAssertion.AssertElementContain(ShipmentSelectors.CustomsTransmissionsUserName, SentBy);
    })
    cy.Click(ShipmentSelectors.CloseCustomsTransmissions, null)
}