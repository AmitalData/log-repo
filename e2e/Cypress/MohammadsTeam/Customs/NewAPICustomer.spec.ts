

import {RandomGenerator} from "../Helper/RandomGenerator";
import { APILoginComp } from "../../login/APILogin.po";
export class NewAPICustomer {
private login: APILoginComp = new APILoginComp();
}
describe('New API Customer Successfully', () => {

let R: RandomGenerator= new RandomGenerator();

  it('New Import Declarations Created Successfully', function () {

  var Tenant  = Cypress.env("CustomsTenant");
  var APIURL = Cypress.env("CustomsAPIURL");

	cy.window().then(win=> {
      const Token = win.sessionStorage.getItem('Token')
      cy.request({
        method: 'POST',
        url: APIURL +'Customers',  
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
      .its('body')
      .then(customerPM => {
       win.sessionStorage.setItem('CusstomerGECUId', customerPM.Id);
       win.sessionStorage.setItem('CusstomerGECUCode', customerPM.Code);
       cy.log("customer Id: "+customerPM.Id);
      });
   }); 
  });
});





