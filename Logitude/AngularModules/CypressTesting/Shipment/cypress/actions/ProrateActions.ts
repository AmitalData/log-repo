import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';

export function AssertProrate() {
    BaseAssertion.AssertElementTextEqual(ShipmentSelectors.MasterExpectedAmount, "72.00")
    BaseAssertion.AssertElementTextEqual(ShipmentSelectors.HouseExpectedAmount + BaseSelectors.FirstElement, "24.00")
    BaseAssertion.AssertElementTextEqual(ShipmentSelectors.HouseExpectedAmount + BaseSelectors.LastElement, "48.00")
    BaseAssertion.AssertElementTextEqual(ShipmentSelectors.SumOfAmount, "72.00")
}

export function AssertHouseHasNonEditablePayable(message) {
    BaseAssertion.AssertElementContain(BaseSelectors.OrangeInfo, message)
}