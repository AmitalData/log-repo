

import { APILoginComp } from "../../login/APILogin.po";
import {RandomGenerator} from "../Helper/RandomGenerator";
import { GetCreateAPICustomer } from './GetCreateAPICustomer';
export class NewImportDeclaration {
private login: APILoginComp = new APILoginComp();
private APICustomer: GetCreateAPICustomer = new GetCreateAPICustomer();
}
describe('New API Import Declarations ', () => {

let R: RandomGenerator= new RandomGenerator();

  it('New API Import Declarations Created Successfully', function () {

  var GetUniqueId = R.GenerateRandomNumberByDate();
  var Tenant  = Cypress.env("CustomsTenant");
  var APIURL = Cypress.env("CustomsAPIURL");

	cy.window().then(win=> {
      const Token = win.sessionStorage.getItem('Token')
      const CusstomerGECUId = win.sessionStorage.getItem('CusstomerGECUId')
 
	  cy.request({
      method: 'POST',
      url: APIURL + 'Declarations', 
      headers: {
      'Content-Type': 'application/json',
	    'Token': Token,
     },
     body: {
 
            CustomFileNo : GetUniqueId,
            Tenant : Tenant,
            CustomerId : CusstomerGECUId,
            TransportModeId : 'A',
            DeclarationOfficeCode : '3',
      }
     }).its('body')
     .then(ImportDeclaration => {
      win.sessionStorage.setItem('ImportDeclarationId', ImportDeclaration.Id);
      win.sessionStorage.setItem('CustomFileNo', ImportDeclaration.CustomFileNo);
      cy.log("Declaration Id: "+ImportDeclaration.Id);
     });
   }); 
  });
});





