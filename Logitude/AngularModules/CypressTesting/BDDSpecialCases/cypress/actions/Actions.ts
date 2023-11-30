import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { BaseURLs } from "../../../Base/cypress/constants/URLs";
import { LocalSettingsDetails } from "../models/LocalSettingsDetails";
import { BDDSpecialCasesSelectors } from "../selectors/Selectors";
import { Urls } from "../constants/URLs";

export function FillLocalSettingsDetails(localSettingsDetails: LocalSettingsDetails) {
  cy.get(BDDSpecialCasesSelectors.TimeZoneComboBox).find("img").click({ force: true })
  cy.get(BDDSpecialCasesSelectors.ComboBoxItem).find("span").contains(localSettingsDetails.TimeZone).type('{enter}');
  cy.get(BDDSpecialCasesSelectors.DateTimeFormatComboBox).find("img").click({ force: true })
  cy.get(BDDSpecialCasesSelectors.ComboBoxItem).find("span").contains(localSettingsDetails.DateTimeFormat).type('{enter}');
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

export function ValidateDateFormat(dateFormat: string, date: string) {
  cy.get(BDDSpecialCasesSelectors.HAWBDate).focus().should(BaseSelectors.HaveValue, FormateTheDate(date, dateFormat))
}

function FormateTheDate(date: string, format: string) {
  var DateFormat
  var Datelist = date.split("/");
  var dd = Datelist[0];
  var mm = Datelist[1]
  var yyyy = Datelist[2]

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
    OpenEventTab($eventTab, eventTabSelector)
    if (expectedEvent) {
      AssertEventTime(expectedEvent)
    }
  });
}

function OpenEventTab($eventTab, eventTabSelector) {
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
      AssertTimeOneOf(text);
    })
  });
}

function AssertTimeOneOf(text: string) {
  var dateTimeNow = GetTimeZoneDateTime()
  var dateTimeRange = DateTimeRange(dateTimeNow)
  expect(text).to.be.oneOf(dateTimeRange)
}

function GetTimeZoneDateTime() {
  var TimeZone = LocalSettingsDetails.UpdateTime
  cy.log(TimeZone)
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
  if (Number(hour) > 0 && Number(hour) < 12) {
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
  return hour;
}

function DateTimeRange(time: string) {
  var timelist = time.split(":")
  var hour = Number(timelist[0])
  var minutes = Number(timelist[1])
  var dateTimeRange = []
  for (let i = 0; i < 10; i++) {
    if (minutes == 0) {
      hour = subHour(hour)
      minutes = 59
    } else {
      minutes = minutes - 1
    }
    dateTimeRange.push(timeformat(hour) + ":" + timeformat(minutes));
  }
  hour = Number(timelist[0])
  minutes = Number(timelist[1])
  dateTimeRange.push(timeformat(hour) + ":" + timeformat(minutes));
  for (let i = 0; i < 10; i++) {
    if (minutes == 59) {
      hour = hour + 1
      minutes = 0
    } else {
      minutes = minutes + 1
    }
    dateTimeRange.push(timeformat(hour) + ":" + timeformat(minutes));
  }
  return dateTimeRange
}

function subHour(hour: number) {
  if (hour == 0) {
    hour = 23
  } else {
    hour = hour - 1
  }
  return hour
}

function timeformat(time: number) {
  if (time == 0) {
    return "00"
  }
  if (time < 10 && time > 0) {
    return "0" + time
  }
  return time
}