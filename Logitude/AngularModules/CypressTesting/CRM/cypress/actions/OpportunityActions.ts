import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { QuoteURLs } from "../../../Quote/cypress/constants/URLs";
import { QuoteDetails } from "../../../Quote/cypress/models/QuoteDetails";
import * as QuotesActions from '../../../Quote/cypress/actions/Actions';
import { QuoteSelectors } from "../../../Quote/cypress/selectors/Selectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { OpportunityDetails } from "../models/OpportunityDetails";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { OpportunitySelectors } from "../selectors/OpportunitySelectors"
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';

let searchFieldValue = null

export function NavigatesOpportunityWorkSpace() {
    cy.Click(BaseSelectors.CRMMenu, null)
    cy.Click(OpportunitySelectors.Opportunity, null)
    cy.Click(OpportunitySelectors.NEWOPPORTUNITY, null)
}

export function FillOpportunityWizardsFields(opportunityDetails: OpportunityDetails) {
    cy.FillLogLov(OpportunitySelectors.OpportunityType, opportunityDetails.OpportunityType, true)
    cy.FillLogTextBox(OpportunitySelectors.Subject + BaseSelectors.LastElement, "Opportunity_" + GenerateCurrentDatetimeString("_"))
    cy.FillLogLov(OpportunitySelectors.Customer, opportunityDetails.Customer, true)
    cy.FillLogTextBox(OpportunitySelectors.Notes, opportunityDetails.Notes)
}

export function CreateOpportunity() {
    DefinePostOpportunityRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostOpportunityRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Opportunities, RequestAliases.PostOpportunities);
}

export function AssertCreateOpportunity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostOpportunities, 200).then((interception) => {
        searchFieldValue = interception.response.body.Subject
    });
}

export function SearchOpportunity() {
    DefineViewsGetByFiltersRequest();
    cy.FillLogTextBox(OpportunitySelectors.SearchTextboxInput, searchFieldValue);
}

function DefineViewsGetByFiltersRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(searchFieldValue), RequestAliases.GetFilterSearch);
}

export function AssertSearchOpportunity() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function OpenOpportunity() {
    DefineOpportunityGetSingleRequest();
    cy.get(OpportunitySelectors.QuickSearchTextBox)
        .within(() => {
            cy.get('ul > li').eq(0).click({ force: true });
        });
}

function DefineOpportunityGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.OpportunitiesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenOpportunity() {
    AssertOpportunityGetSingle();
    BaseAssertion.AssertElementExist(OpportunitySelectors.GeneralEditScreen)
}

function AssertOpportunityGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function MarkTaskAsComplete() {
    DefineCompleteActivityRequest()
    cy.get(OpportunitySelectors.TaskCompleteButton).eq(0).click({ force: true })
}

function DefineCompleteActivityRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetCompleteActivity, RequestAliases.GetCompleteActivity);
}

export function AssertCompleteActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetCompleteActivity, 200);
}

export function AddCompetitor() {
    DefinePutOpportunityRequest()
    cy.Click(OpportunitySelectors.CompetitorsToggleButton, null)
    cy.get(OpportunitySelectors.CompetitorsCheckBox).eq(0).next("label").click({ force: true })
}

export function RemoveCompetitor() {
    cy.wait(1000)
    DefinePutOpportunityRequest()
    cy.get(OpportunitySelectors.DeleteItemCompetitor).eq(0).click({ force: true })
}

export function AddAdditionalService() {
    DefinePutOpportunityRequest()
    cy.Click(OpportunitySelectors.AdditionalServicesToggleButton, null)
    cy.get(OpportunitySelectors.AdditionalServicesCheckBox).eq(0).next("label").click()
}

export function RemoveAdditionalService() {
    cy.wait(1000)
    DefinePutOpportunityRequest()
    cy.get(OpportunitySelectors.DeleteItemService).eq(0).click({ force: true })
}

export function UpdateOpportunity() {
    DefinePutOpportunityRequest()
    cy.Click(OpportunitySelectors.SaveButton, null)
}

function DefinePutOpportunityRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Opportunities, RequestAliases.PutOpportunities)
}

export function AssertUpdateOpportunity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutOpportunities, 200)
}

export function FillQuoteWizerdFields(quoteDetails: QuoteDetails) {
    QuotesActions.FillMainFields(quoteDetails);
    QuotesActions.FillMainCarriagePorts(quoteDetails);
}

export function CreateQuote() {
    cy.DefineRequestWait(RestAPI.POST, QuoteURLs.Quotes, RequestAliases.Quotes)
    cy.DefineRequestWait(RestAPI.GET, Urls.GetQuotesByOpportunityId, RequestAliases.GetQuotesByOpportunityId)
    cy.Click(QuoteSelectors.CreateQuote, null)
}

export function ChooseQuote() {
    cy.Click(OpportunitySelectors.ConnectQuoteButton, null)
    cy.get(OpportunitySelectors.ConnectQuoteCheckBox).eq(0).click({ force: true })
}

export function ConnectQuote() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetQuotesByOpportunityId, RequestAliases.GetQuotesByOpportunityId)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}