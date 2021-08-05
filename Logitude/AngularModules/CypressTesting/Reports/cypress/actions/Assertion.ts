import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";

export function AssertSendReport() {
    BaseAssertion.AssertStatusCode(RequestAliases.SendReport, 200)
}
export function AssertPrintReport() {
    BaseAssertion.AssertWindowOpen(RequestAliases.PrintReportWindowOpen);
}