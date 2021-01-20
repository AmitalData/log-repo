
 
import {RandomGenerator} from "../../../Helper/RandomGenerator";
export class APIImportDeclarationHelper {
  public NewAPIImportDeclaration(){
    return new Cypress.Promise((resolve) => {
        let R: RandomGenerator= new RandomGenerator();
          var GetUniqueId = R.GenerateRandomNumberByDate();
          var Tenant  = Cypress.env("CustomsTenant");
          var APIURL = Cypress.env("CustomsAPIURL")+"/api/";
        
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
              cy.log("Declaration CustomerId: "+CusstomerGECUId);
              cy.log("Declaration Id: "+ImportDeclaration.Id);
              cy.log("Declaration CustomFileNo: "+ImportDeclaration.CustomFileNo);
             });
           }); 
           resolve('foo')
          });
}

}






