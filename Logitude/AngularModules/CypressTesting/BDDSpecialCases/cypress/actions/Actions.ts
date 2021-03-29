
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { BaseURLs } from "../../../Base/cypress/constants/URLs";
import { LocalSettingsDetails } from "../models/LocalSettingsDetails";
import { BDDSpecialCasesSelectors } from "../selectors/Selectors";
import { Urls } from "../constants/URLs";


export function FillLocalSettingsDetails(localSettingsDetails: LocalSettingsDetails) {
  cy.get(BDDSpecialCasesSelectors.TimeZoneComboBox).find("img").click()
  cy.get(BDDSpecialCasesSelectors.ComboBoxItem).find("span").contains(localSettingsDetails.TimeZone).click({ force: true });
  cy.get(BDDSpecialCasesSelectors.DateTimeFormatComboBox).find("img").click()
  cy.get(BDDSpecialCasesSelectors.ComboBoxItem).find("span").contains(localSettingsDetails.DateTimeFormat).click({ force: true });
}

export function UpdateLocalSettings() {
  DefinePutTenant()
  cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertUpdateLocalSettings() {
  AssertPutTenant()
}

function DefinePutTenant() {
  cy.DefineRequestWait(RestAPI.PUT, Urls.tenants, RequestAliases.PutTenant);
}

function AssertPutTenant() {
  BaseAssertion.AssertStatusCode(RequestAliases.PutTenant, 200);
}

export function ValidateDateFormat(dateFormat: string) {
  var todayDate = new Date
  cy.get(BDDSpecialCasesSelectors.HAWBDate).should(BaseSelectors.HaveValue, FormateTheDate(todayDate, dateFormat))
}

function FormateTheDate(date: Date, format: string) {
  var DateFormat
  var dd = date.getUTCDate().toString();
  var mm = (date.getUTCMonth() + 1).toString();
  var yyyy = date.getFullYear().toString();

  if (Number(dd) < 10) {
    dd = "0" + dd;
  }
  if (Number(mm) < 10) {
    mm = "0" + mm;
  }

  if (format == "MM/dd/yyyy") {
    DateFormat = mm + '/' + dd + '/' + yyyy;
  } else if (format == "dd/MM/yyyy") {
    DateFormat = dd + '/' + mm + '/' + yyyy;
  }

  return DateFormat;
}

export function ValidateTimeInEventsTab(expectedEvent: string, eventTabSelector: string) {
  cy.get(eventTabSelector).then(($eventTab) => {
    OpenEventTab($eventTab,eventTabSelector)
    if (expectedEvent) {
      AssertEventTime(expectedEvent)
    }

  });
}

function OpenEventTab($eventTab , eventTabSelector) {
  cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetTraceEventsForEntity, RequestAliases.GetTraceEventsForEntity);
  if ($eventTab.hasClass("SelectedMenuItem")) {
    cy.Click(BaseSelectors.RefreshImg + BaseSelectors.LastElement, null, true);
  } else {
    cy.Click(eventTabSelector, null, true);
  }
  BaseAssertion.AssertStatusCode(RequestAliases.GetTraceEventsForEntity, 200);
}

function AssertEventTime(expectedEvent: string) {
  cy.get(BaseSelectors.EventItemBox).contains(expectedEvent).eq(0).parents(BaseSelectors.EventItemBox).within(() => {
    cy.get(BDDSpecialCasesSelectors.EventDateTime(expectedEvent)).invoke('text').then((text) => {
      assert.equal(text, GetTimeZoneDateTime());
    })
  });
}

function GetTimeZoneDateTime() {
  var TimeZone = LocalSettingsDetails.UpdateTime
  var TimeList = TimeZone.split(",")
  TimeZone = TimeList[1]
  TimeZone = TimeZone.replace(" ", "");
  var DateTimeList = TimeZone.split(":")
  DateTimeList[0] = HourFormat(DateTimeList[0], DateTimeList[2])
  TimeZone = DateTimeList[0] + ":" + DateTimeList[1]
  return TimeZone;
}

function HourFormat(hour: string, AMPM: string) {
  if (AMPM.includes("AM")) {
    return FormatAMTimes(hour)
  } else {
    return FormatPMTimes(hour)
  }
}

function FormatPMTimes(hour: string) {
  if (Number(hour) > 0 && Number(hour) < 10) {
    return (Number(hour) + 12).toString();
  }
  if (Number(hour) == 12) {
    return "12"
  }
}

function FormatAMTimes(hour: string) {
  if (Number(hour) > 0 && Number(hour) < 10) {
    return "0" + hour
  }
  if (Number(hour) == 12) {
    return "00"
  }
}
