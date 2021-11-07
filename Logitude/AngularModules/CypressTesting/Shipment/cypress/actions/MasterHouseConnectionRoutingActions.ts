import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { ShipmentSelectors } from '../selectors/Selectors';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';

export function FillPreForwarding(transportMode: string, fromPort: string) {
    cy.Click(ShipmentSelectors.RoutingToggle_Number + BaseSelectors.LastElement, null)
    cy.Click(ShipmentSelectors.PreForwarding, null)
    cy.FillLogLov(ShipmentSelectors.ShipmentPreForwardingTransportModeId, transportMode, true)
    cy.FillLogLov(ShipmentSelectors.ShipmentPreForwardingFromPortId, fromPort, false)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}

export function FillOnForwarding(transportMode: string, toPort: string) {
    cy.Click(ShipmentSelectors.RoutingToggle_Number + BaseSelectors.LastElement, null)
    cy.Click(ShipmentSelectors.OnForwarding, null)
    cy.FillLogLov(ShipmentSelectors.ShipmentOnForwardingTransportModeId, transportMode, true)
    cy.FillLogLov(ShipmentSelectors.ShipmentOnForwardingToPortId, toPort, false)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}

export function AssertPreOnCarriageDisabled(message) {
    BaseAssertion.AssertElementContain(BaseSelectors.OrangeInfo, message)
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsCancel)
}