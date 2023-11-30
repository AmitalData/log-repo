import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { SignatureDetails } from "../models/SignatureDetails";

function AddDataField(dataField: string) {
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainAddDataField)
    cy.contains(dataField).click()
    cy.get(BaseSelectors.RedButton + BaseSelectors.LastElement).click()
}
export function AddDataFields(signatureDetails: SignatureDetails) {
    AddDataField(signatureDetails.Date)
    AddDataField(signatureDetails.Logo)
    AddDataField(signatureDetails.SmallLogo)
    AddDataField(signatureDetails.WideLogo)
    AddDataField(signatureDetails.LocalCurrency)
    AddDataField(signatureDetails.Company)
    AddDataField(signatureDetails.Email)
    AddDataField(signatureDetails.Website)
    AddDataField(signatureDetails.IATA)
    AddDataField(signatureDetails.VATNo)
    AddDataField(signatureDetails.AddressID)
    AddDataField(signatureDetails.SupporteMail)
    AddDataField(signatureDetails.UserSignatureImage)
}

export function DefinePutUpdateSignaturesRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Signature, RequestAliases.Signature);
}