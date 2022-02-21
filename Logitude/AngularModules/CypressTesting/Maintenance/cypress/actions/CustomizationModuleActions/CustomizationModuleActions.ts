import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";

import { AutomationsDetails } from "../../models/AutomationsModuleDetails/AutomationsDetails"
import { CustomizationSelectors } from "../../selectors/CustomizationModuleSelectors/CustomizationSelectors"
import { CustomizationConstants } from "../../constants/CustomizationConstants/CustomizationConstants";
import { CustomizationURLs } from "../../constants/CustomizationURLs/CustomizationURLs";
import { CustomizationRequestAliases } from "../../constants/CustomizationURLs/CustomizationRequestAliases";


export function ChooseCustomization() {

    cy.get(CustomizationSelectors.Settings).click()
    cy.get(CustomizationSelectors.Customization).click()

}

export function ChooseObjectTable(objecttable: string){
    cy.wait(500)
    cy.get(CustomizationSelectors.Search).type(objecttable)
}