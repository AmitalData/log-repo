
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { BaseURLs } from "../../../Base/cypress/constants/URLs";
import { ReportsSelectors } from "../selectors/Selectors";
import { ReportsUrls } from "../constants/ReportsUrls";
import { ReportSettingsDetails } from  "../../cypress/models/ReportSettingsDetails";
import {constants} from "../../../Base/cypress/constants/constants"

export function AssertSendReport(){
    BaseAssertion.AssertStatusCode(RequestAliases.SendReport,200)
}
export function AssertPrintReport(){
    BaseAssertion.AssertWindowOpen(RequestAliases.PrintReportWindowOpen);
}