/// <reference types="cypress" />

import { APILoginComp } from "../../login/APILogin.po";
import {APIHelper} from "../Helper/APIHelper";
import {ApiQueryFilters} from "../Helper/ApiQueryFilters";


export class GetCreateAPICustomer {
  private login: APILoginComp = new APILoginComp();

}
 
it('Get or Create API Customer Successfully', () => {
 
   
  var Tenant  = Cypress.env("CustomsTenant");

	cy.window().then(win=> {
    const Token = win.sessionStorage.getItem('Token');
    var filters = new ApiQueryFilters();
         filters.GetAll = true;
         filters.addAdditionalFilter("EnglishName", "GE:Cusstomer", null, null, "Equal", false, false, false, "number");

          var _APIHelper = new APIHelper();
          var URLParameter = _APIHelper.CreateParametersUrl(filters);


    cy.request({
      method: 'GET',
      url: Cypress.env("LocalAPIURL") +'CardViews' + URLParameter,  
      headers: {
        'Content-Type': 'application/json',
        'Token': Token,
       },
    }).then(response => {
      var Result = response.body.Result[0];
      cy.window().then(win => { 
       if(Result!=null && Result!=""){
        cy.log(Result.Id+" " + Result.Code);
        win.sessionStorage.setItem('CusstomerGECUId', Result.Id);
        win.sessionStorage.setItem('CusstomerGECUCode', Result.Code);
       }
       else {
        cy.request({
          method: 'POST',
          url: Cypress.env("LocalAPIURL") +'Customers',  
          headers: {
            'Content-Type': 'application/json',
            'Token': Token,
           },
          body: {
            EnglishName: "GE:Cusstomer",
            LocalName: "GE:Cusstomer",
            CityName : "Guaynabo",
            CountryName : "Guaynabo",
            Tenant : Tenant,
            PartnerTypeId : "CS",
            InActive : false,
            IsCustomer : true,
            EnableConsolidationInvoices : false,
            IsActiveForMobile : false
        
          },
         
        })
       }
     });
    });
    


})
})


