import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { Constants } from "../constants/Constants";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import {SignatureDetails} from "../models/SignatureDetails";

//#region Signature
export function AddDataFields(signatureDetails: SignatureDetails) {
    if (signatureDetails.Date.toUpperCase() == Constants.Add) {
        AddDataField(MaintenanceSelectors.SignatureDate)
    }
    if (signatureDetails.User.toUpperCase() == Constants.Add) {
        AddDataField(MaintenanceSelectors.SignatureUser)
    }
    if (signatureDetails.Logo) {
        AddDataField(MaintenanceSelectors.SignatureLogo)
    }
    if (signatureDetails.SmallLogo) {
        AddDataField(MaintenanceSelectors.SignatureSmallLogo)
    }
    if (signatureDetails.WideLogo) {
        AddDataField(MaintenanceSelectors.SignatureWideLogo)
    }
    if (signatureDetails.LocalCurrency) {
        AddDataField(MaintenanceSelectors.SignatureLocalCurrency)
    }
    if (signatureDetails.Company) {
        AddDataField(MaintenanceSelectors.SignatureCompany)
    }
    if (signatureDetails.Email) {
        AddDataField(MaintenanceSelectors.SignatureEmail)
    }
    if (signatureDetails.Website) {
        AddDataField(MaintenanceSelectors.SignatureWebsite)
    }
    if (signatureDetails.IATA) {
        AddDataField(MaintenanceSelectors.SignatureIATA)
    }
    if (signatureDetails.VATNo) {
        AddDataField(MaintenanceSelectors.SignatureVATNo)
    }
    if (signatureDetails.AddressID) {
        AddDataField(MaintenanceSelectors.SignatureAddressID)
    }
    if (signatureDetails.SupporteMail) {
        AddDataField(MaintenanceSelectors.SignatureSupporte_mail)
    }
    if (signatureDetails.UserSignatureImage) {
        AddDataField(MaintenanceSelectors.SignatureUserSignatureImage)
    }
}
function AddDataField(dataField: string){

    cy.Click(BaseSelectors.Button, BaseSelectors.ContainAddDataField)
    cy.contains(dataField).click()
    cy.get(BaseSelectors.OkButton).click()
}
export function DefinePutUpdateSignaturesRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Signature, RequestAliases.Signature);
}
//#endregion