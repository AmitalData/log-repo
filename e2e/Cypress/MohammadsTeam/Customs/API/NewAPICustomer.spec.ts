

import {RandomGenerator} from "../../Helper/RandomGenerator";
import { APILoginComp } from "../../../login/APILogin.po";
import { APILoginHelper } from "../../../login/APIHelperMethode/APILoginHelper.po";
import { APICustomerHelper } from "./APIHelperMethode/APICustomerHelper";
export class NewAPICustomer {
private login: APILoginComp = new APILoginComp();
}
 
  it('New API Customer Successfully', () => {
    var aPILoginHelper:APILoginHelper = new APILoginHelper();
    var aPICustomerHelper:APICustomerHelper = new APICustomerHelper();
    cy.wrap(null).then(() => {
      return aPILoginHelper.APILoginSubmit().then((str) => {
        cy.wrap(null).then(() => {
          return aPICustomerHelper.GetCreateAPICustomer().then((str) => {
          })
        })
        
      })
    })
  });
  
 





