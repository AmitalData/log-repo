/// <reference types="cypress" />

import { APILoginHelper } from "../../../login/APIHelperMethode/APILoginHelper.po";
import { APICustomerHelper } from "./APIHelperMethode/APICustomerHelper";


export class GetCreateAPICustomer {
 
}
  
it('Get or Create API Customer Successfully', () => {
  var aPILoginHelper:APILoginHelper = new APILoginHelper();
  var aPICustomerHelper:APICustomerHelper = new APICustomerHelper();
    aPILoginHelper.APILoginSubmit().then((str) => {
    aPICustomerHelper.GetCreateAPICustomer().then((str) => {});
  });
});