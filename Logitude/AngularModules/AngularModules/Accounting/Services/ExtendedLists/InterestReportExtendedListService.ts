
import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of, Observable } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { InterestReportArguments } from '../../DataContracts/InterestReportArgs';
import { InterestReportPM } from 'Accounting/EntityPMs/InterestReportPM';
import { InterestReportLinesByDatePM } from 'Accounting/EntityPMs/InterestReportLinesByDatePM';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { CustomFieldClass } from 'Infrastructure/DataContracts/CustomFieldClass';


@Injectable()

export class InterestReportExtendedListService {

  private _apiUrl: string;
  private httpClient: HttpClient;
  constructor() {

    this.httpClient = ServiceHelper.HttpClient;
    this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/interestreport';
  }

  getByFilters(filters: ApiQueryFilters) {

      var urlparameters = '/GetInterestReportsByFilters?';
    var mykeys = Object.keys(filters);
    var addtionalFiltersValues = null;
    for (var i in mykeys) {
      var propName = mykeys[i];
      var propValue = filters[propName];

      var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

      if (urlparameters != "?") {
        urlparameters = urlparameters.concat('&');
      }
      if (!ignoreFilter) {
        propValue = encodeURIComponent(propValue);
        urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
      }

      if (propName == "AdditionalFilters" && propValue.length > 0)
        addtionalFiltersValues = JSON.stringify(propValue);

    }
    if (addtionalFiltersValues) {
      urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
    }

    var callUrl = this._apiUrl.concat(urlparameters);

    return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
      map((response: ServiceResponse) => {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        serviceResponse = response;
        return serviceResponse;
      }),
      catchError(ServiceHelper.HandleServiceError));


  }

 
    PutInterestReortStatus(interestReportArgs: InterestReportArguments) {
        return this.httpClient.put(this._apiUrl + "/PutCreateInterestReportInvoiceBatch", JSON.stringify(interestReportArgs), ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }

    IsCreateInvoicedValid(InterestReportPM:InterestReportPM) {
      var mappedEntity: InterestReportPM;
          mappedEntity = this.MapJsonToEntityPM(InterestReportPM, false);
      return this.httpClient.put(this._apiUrl + "/PutIsCreateInvoicedValid", JSON.stringify(mappedEntity) ,  ServiceHelper.GetHttpHeaders()).pipe(
          map(res => {
              var serviceResponse: ServiceResponse;
              serviceResponse = new ServiceResponse();
              var result = res;
              serviceResponse.Result = result;

              return serviceResponse;
          }),
          catchError(ServiceHelper.HandleServiceError));
  }

    GetNumberOfDocumentNotPrinted(interestReportArgs: InterestReportArguments) {
      return this.httpClient.put(this._apiUrl + "/PutNumberOfDocumentNotPrinted", JSON.stringify(interestReportArgs),  ServiceHelper.GetHttpHeaders()).pipe(
          map(res => {
              var serviceResponse: ServiceResponse;
              serviceResponse = new ServiceResponse();
              var result = res;
              serviceResponse.Result = result;

              return serviceResponse;
          }),
          catchError(ServiceHelper.HandleServiceError));
  }
  PrintDocuments(interestReportArgs: InterestReportArguments) {
    return this.httpClient.put(this._apiUrl + "/PrintDocuments", JSON.stringify(interestReportArgs),  ServiceHelper.GetHttpHeaders()).pipe(
        map(res => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var result = res;
            serviceResponse.Result = result;

            return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError));
}
    PutBatchPrint(interestReportArgs: InterestReportArguments) {
      return this.httpClient.put(this._apiUrl + "/PutBatchPrint", JSON.stringify(interestReportArgs), ServiceHelper.GetHttpHeadersForblob()).pipe(
          map(res => {
              var serviceResponse: ServiceResponse;
              serviceResponse = new ServiceResponse();
              var result = res;
              serviceResponse.Result = result;

              return serviceResponse;
          }),
          catchError(ServiceHelper.HandleServiceError));
  }
 
 

GetInterestLastBatchServiceByTenant() {
      return this.httpClient.get(this._apiUrl + "/GetInterestLastBatchServiceByTenant",ServiceHelper.GetHttpHeaders()).pipe(
          map(res => {
              var serviceResponse: ServiceResponse;
              serviceResponse = new ServiceResponse();
              var result = res;
              serviceResponse.Result = result;

              return serviceResponse;
          }),
          catchError(ServiceHelper.HandleServiceError));
  }
    
    CheckNumberOfInterestReportInvoicingWithoutInvoice(interestReportArgs: InterestReportArguments) {
      return this.httpClient.put(this._apiUrl + "/PutCheckNumberOfInterestReportInvoicingWithoutInvoice", JSON.stringify(interestReportArgs), ServiceHelper.GetHttpHeaders()).pipe(
          map(res => {
              var serviceResponse: ServiceResponse;
              serviceResponse = new ServiceResponse();
              var result = res;
              serviceResponse.Result = result;

              return serviceResponse;
          }),
          catchError(ServiceHelper.HandleServiceError));
  }

  PostInterestReportsForEligibleCustomerCreationInBatch(interestCalculationDate:Date) {

    let postUrl:string=ServiceHelper.GetLogitudeURL() + 'api/interestreportsforeligiblecustomercreation';
    return this.httpClient.put(postUrl + "/PutInterestReportsForEligibleCustomerCreationInBatch", interestCalculationDate, ServiceHelper.GetHttpHeaders()).pipe(
        map(res => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var result = res;
            serviceResponse.Result = result;

            return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError));
}
SendSignedInvoices(selectList: string[]) {
     
    return this.httpClient.put(this._apiUrl + "/PutSendSignedInvoices", JSON.stringify(selectList), ServiceHelper.GetHttpHeaders()).pipe(
        map(res => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var result = res;
            serviceResponse.Result = result;

            return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError));
}
MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: InterestReportPM = null) {


  if (!entityPM) {

      entityPM = new InterestReportPM();
  }

  var customFields: Array<string> = [];
  for (var i = 1; i < 11; i++) {
      customFields.push("Field" + i);
  }
  var jsonPMKeys = Object.keys(jsonPM);

  for (var key in jsonPMKeys) {
      if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

          continue;
      }
      var property = jsonPMKeys[key];

      if (customFields.indexOf(property) > -1) {
          if (jsonPM[property]) {
              var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
              entityPM[property] = customFieldClass;
          }
      }
      else {
          entityPM[property] = jsonPM[property];
      }

  }

  this.MapInterestReportLinesByDates(entityPM, jsonPM, mapParent); // Call composition tables map methods



  if (mapParent) {
      entityPM.OldEntityPM = this.clone(entityPM);

      entityPM.OldEntityPM.InterestReportLinesByDates = [];
      for (var item in entityPM.InterestReportLinesByDates) {
          var myInterestReportLinesByDatePM = entityPM.InterestReportLinesByDates[item];
          var newInterestReportLinesByDatePM: InterestReportLinesByDatePM = this.clone(myInterestReportLinesByDatePM);


          entityPM.OldEntityPM.InterestReportLinesByDates.push(newInterestReportLinesByDatePM);
      }

  }
  else {

      entityPM.OldEntityPM = null;
  }
  entityPM.IsDirty = false;
  return entityPM;
}

MapInterestReportLinesByDates(entityPM: InterestReportPM, jsonPM: any, mapParent: boolean = true) {

  var oldInterestReportLinesByDates: InterestReportLinesByDatePM[] = [];
  if (entityPM.OldEntityPM && !mapParent) {
      oldInterestReportLinesByDates = entityPM.OldEntityPM.InterestReportLinesByDates;
  }

  entityPM.InterestReportLinesByDates = new Array<InterestReportLinesByDatePM>();
  for (var item in jsonPM.InterestReportLinesByDates) {
      var jItem = jsonPM.InterestReportLinesByDates[item];
      if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
          continue;
      }
      var newInterestReportLinesByDatePM: InterestReportLinesByDatePM;

      if (mapParent) {
          newInterestReportLinesByDatePM = new InterestReportLinesByDatePM(entityPM);
      }
      else {
          newInterestReportLinesByDatePM = new InterestReportLinesByDatePM(null);
      }

      var pmKeysArray = Object.keys(jItem);
      for (var pmKey in pmKeysArray) {
          if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
              continue;
          }
          var pmProperty = pmKeysArray[pmKey];
          newInterestReportLinesByDatePM[pmProperty] = jItem[pmProperty];
      }


      if (mapParent) {
          newInterestReportLinesByDatePM.UniqueKey = Guid.newGuid();
          newInterestReportLinesByDatePM.ChangeSetOp = "None";
          jItem.ChangeSetOp = "None";
          newInterestReportLinesByDatePM.OldEntityPM = this.clone(newInterestReportLinesByDatePM);


      }
      else {
          if (newInterestReportLinesByDatePM.UniqueKey) {

              if (jItem.IsDirty)
                  newInterestReportLinesByDatePM.ChangeSetOp = "Update";
          }
          else {
              newInterestReportLinesByDatePM.ChangeSetOp = "Insert";
          }

          newInterestReportLinesByDatePM.OldEntityPM = null;
          newInterestReportLinesByDatePM.EntityParentPM = null;
      }

      newInterestReportLinesByDatePM.IsDirty = false;
      entityPM.InterestReportLinesByDates.push(newInterestReportLinesByDatePM);
  }
  if (oldInterestReportLinesByDates) {

      for (var itemKey in oldInterestReportLinesByDates) {
          if (entityPM.InterestReportLinesByDates.filter(p => p.UniqueKey === oldInterestReportLinesByDates[itemKey].UniqueKey).length === 0) {

              if (oldInterestReportLinesByDates[itemKey]) {
                  //oldInterestReportLinesByDates[itemKey].ChangeSetOp = "Delete";
                  //entityPM.InterestReportLinesByDates.push(oldInterestReportLinesByDates[itemKey]);
                  var oldItemJson = oldInterestReportLinesByDates[itemKey];
                  var deletedPM: InterestReportLinesByDatePM = new InterestReportLinesByDatePM(null);
                  var pmKeys = Object.keys(oldItemJson);
                  for (var key in pmKeys) {

                      if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                          continue;
                      }

                      var property = pmKeys[key];
                      deletedPM[property] = oldItemJson[property];
                  }


                  deletedPM.IsDirty = false;
                  deletedPM.ChangeSetOp = "Delete";

                  deletedPM.OldEntityPM = null;
                  entityPM.InterestReportLinesByDates.push(deletedPM);
              }
          }
      }
  }
}
public clone(jsonPM: any) {
  var entityPM: any;
  entityPM = {};

  var jsonPMKeys = Object.keys(jsonPM);
  for (var key in jsonPMKeys) {

      if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
          continue;
      }

      var property = jsonPMKeys[key];
      entityPM[property] = jsonPM[property];

  }
  return entityPM;
}

}

export class PDFDocumentInvoices {
  public Document:any;
  public ARInvoiceNumbersNotPrinted:string[];
  public InterestReportNumbersNotPrinted:string[];

}
