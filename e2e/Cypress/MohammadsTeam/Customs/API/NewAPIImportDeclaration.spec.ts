

import { APILoginHelper } from "../../../login/APIHelperMethode/APILoginHelper.po";
import { APICustomerHelper } from "./APIHelperMethode/APICustomerHelper";
import { APIImportDeclarationHelper } from "./APIHelperMethode/APIImportDeclarationHelper.spec";
export class NewAPIImportDeclaration {
 
}
 
it('New API Import Declarations ', () => {

  var aPILoginHelper:APILoginHelper = new APILoginHelper();
  var aPICustomerHelper:APICustomerHelper = new APICustomerHelper();
  var aPIImportDeclarationHelper:APIImportDeclarationHelper = new APIImportDeclarationHelper();
 

  cy.wrap(null).then(() => {
    return aPILoginHelper.APILoginSubmit().then((str) => {
      cy.wrap(null).then(() => {
        return aPICustomerHelper.GetCreateAPICustomer().then((str) => {
          cy.wrap(null).then(() => {
            return aPIImportDeclarationHelper.NewAPIImportDeclaration().then((str) => {
            })
          })
        })
      })
      
    })
  })

});



