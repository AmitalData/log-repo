/*import { browser, by, element } from "protractor"


import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../Helpers/GeneralFunctions';
import { NewVendor } from "./NewVendorGLaccount";*/
import { CustomerGLA } from "./CustomerGLA";
import { LoginCloud } from './LoginCloud';

import { RandomGenerator } from './RandomGenerator'

describe('CustomerGlAccount Module', function () {

    let R: RandomGenerator = new RandomGenerator();
    let Cs: CustomerGLA = new CustomerGLA();
    let log: LoginCloud = new LoginCloud();
    it(' New Customer GLAccount Was Created', function () {

        log.dologin();
       // cy.get('li[id=GeneralMHMaintenance]', { timeout: 60000 })
        // this was the only way that worked well :/
        var code = R.GenerateRandomNumberACC();
        Cs.createCustomer('Test Customer GLAccount' + code);
        Cs.activatecustomer('Test Customer GLAccount' + code);






    });
});
