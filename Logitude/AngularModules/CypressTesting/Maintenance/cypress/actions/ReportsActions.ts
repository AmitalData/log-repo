import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';
import { ReportsDetails } from "../models/ReportsDetails";
import * as GeneralActions from "./BaseActions";

let reportCode = null;

export function GenerateRandomNumber(NumberLength: number) {
    let NewRandomCode = gr.GenerateRandomNumberAndString(NumberLength)
 return NewRandomCode;
}

export function FillReportstDetails(reportsDetails: ReportsDetails) {
    var RandomReportName = GenerateRandomNumber(4);
    var RandomReportCode = GenerateRandomNumber(6);
    cy.FillLogTextBox(MaintenanceSelectors.ReportName, reportsDetails.Name.toLowerCase() == "random" ? RandomReportName : reportsDetails.Name)
    cy.FillLogTextBox(MaintenanceSelectors.ReportCode, reportsDetails.Code.toLowerCase() == "random" ? RandomReportName : reportsDetails.Code)
}

export function CreateReport() {
    DefinePostReportRequest() 
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function DefineGetByFilterRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetByFilter, RequestAliases.GetByFilter);
}

function DefinePostReportRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Report, RequestAliases.PostReport)
}

export function AssertCreateReport() {
    AssertPost()
    AssertGetByFilters()
}

export const AssertPost = () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostReport, 200).then((interception) => {
        let responseBody = interception.response.body;
        reportCode = responseBody.Code;
    });
}

export function AssertGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetByFilter, 200);
}

function ReCreateReport() {
    let stateCode = GenerateRandomNumber(10);
    cy.FillLogTextBox(MaintenanceSelectors.StateCode, stateCode)
    cy.FillLogTextBox(MaintenanceSelectors.StateEnglishName, stateCode)
    cy.FillLogTextBox(MaintenanceSelectors.StateLocalName, stateCode)
    CreateReport();
    AssertCreateReport();
}

export function AssertSearchReport() {
    GeneralActions.AssertSearch(reportCode);
}
export const SearchReport = () => {
    DefineReportViewsGetFilterSearch(reportCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, reportCode);
    AssertReportViewsGetByFilters();
}

export const DefineReportViewsGetFilterSearch = (reportCode: string) => {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(reportCode), RequestAliases.GetFilterSearch);
}

export function AssertReportViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function FillReportTemplate(reportsDetails: ReportsDetails) {
    cy.Click(MaintenanceSelectors.ReportTemplate, null, true)
    cy.Click(MaintenanceSelectors.AddTemplate, null, true)
    cy.FillLogTextBox(MaintenanceSelectors.ReportTemplateDescription, reportsDetails.Description)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK); 
}

export const CheckEvents = () => {
    cy.Click(MaintenanceSelectors.ReportEventTab, null, true)
}
