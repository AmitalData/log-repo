/*import { browser, by, element } from "protractor"


import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { NewVendor } from "./NewVendorGLaccount";*/
import { CustomerGLA } from "./CustomerGLA";
import { Login } from './Login';

import { RandomGenerator } from './RandomGenerator'

import { LoginComp } from "../../login/Login.po";

export class CustomerSpec {

  private login: LoginComp = new LoginComp();
}

describe('CustomerGlAccount Module', function () {

    let R: RandomGenerator = new RandomGenerator();
    let Cs: CustomerGLA = new CustomerGLA();
    let log: Login = new Login();
    it(' New Customer GLAccount Was Created', function () {

  
       // cy.get('li[id=GeneralMHMaintenance]', { timeout: 60000 })
        // this was the only way that worked well :/
        var code = R.GenerateRandomNumberACC();
        Cs.createCustomer('Test Customer GLAccount' + code);
        Cs.activatecustomer('Test Customer GLAccount' + code);






    });
});
