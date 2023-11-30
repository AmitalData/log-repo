import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { ActivitiesDetails } from "../models/ActivitiesDetails";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { OverviewSelectors } from "../selectors/OverviewSelectors"
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';

let TaskName = ""


export function NavigatesToOverviewInCRM(){
    cy.Click(OverviewSelectors.CRM,null)
    cy.Click(OverviewSelectors.Overview,null)
    

}


export function NavigatesToTaskInOverview() {
    cy.get(OverviewSelectors.OverviewNew).click()
    cy.get(OverviewSelectors.NEWTASK).click()
}
export function FillTaskWizardsFields(activitiesDetails: ActivitiesDetails) {
    FillSubject("Task_")
    cy.FillLogTextBox(OverviewSelectors.ActivityDescription, activitiesDetails.Description)
    cy.FillLogLov(OverviewSelectors.ActivityPriorityCode, activitiesDetails.PriorityCode, true)
}

export function FillSubject(activityType: string) {
    TaskName = activityType + GenerateCurrentDatetimeString("_")
    cy.FillLogTextBox(OverviewSelectors.ActivitySubject + BaseSelectors.LastElement, TaskName)
}

export function CreateActivity() {
    DefinePostActivityRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostActivityRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Activity, RequestAliases.PostActivity);
}

export function AssertCreateActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostActivity, 200).then((interception) => {
    });
}

export function CompleteActivityFromRecentActivitesList() {
    cy.wait(1000)
    DefineCompleteActivityRequest();
    cy.log(TaskName)
    cy.get(OverviewSelectors.RecentEntityItem).eq(0).last().find(OverviewSelectors.Button).click({ force: true });
}

function DefineCompleteActivityRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetCompleteActivity, RequestAliases.GetCompleteActivity);
}

export function AssertCompleteActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetCompleteActivity, 200);
}

export function GoInsideTaskFromDailySpotlightList() {
    cy.wait(1000)
    DefineTodayActivitiesRequest();
    cy.get(OverviewSelectors.DailySpotlightLink).eq(9).click({ force: true });
}

function DefineTodayActivitiesRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetTodayActivity, RequestAliases.GetTodayActivity);
}
export function AssertTodayActivity() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetTodayActivity, 200);
    cy.get(OverviewSelectors.BackToCRM).click({ force: true })
}

export function CreatePost() {
    cy.get(OverviewSelectors.PostButton).click({ force: true })
}

export function AssertCreatePost() {
    cy.get(OverviewSelectors.PostScrollViewer).contains("TestPost");

    
}

export function CreateNewMessage() {
    cy.get(OverviewSelectors.CreateNewMessage).click({ force: true })
    cy.get(OverviewSelectors.EmailSearchTextBox).type("test")
    cy.get(OverviewSelectors.EmailSearch).contains('Test').click()
    cy.get(OverviewSelectors.MessageTxt).type('Test Message')
    cy.get(OverviewSelectors.SendButton).click()
    /*cy.get("#EmailSearchTextBox_DropDown_0_0").click()*/
}

export function AssertCreateNewMessage() {
    cy.get('.MarginAbsolute0').contains("TestPost");
}

