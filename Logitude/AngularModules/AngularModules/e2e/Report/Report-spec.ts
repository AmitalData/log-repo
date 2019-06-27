import { browser, by, element } from 'protractor';
import { SenarioTest } from './SenarioTest';
import { LoginComp } from "../login/Login.po";

describe('Report', () => {

    let Senarios: SenarioTest = new SenarioTest();
    let page: LoginComp = new LoginComp();

    beforeEach(() => {
        browser.driver.manage().window().maximize();
        browser.ignoreSynchronization = true;
    });

        it('Run Report Sucssefuly', function () {
            Senarios.ReportScienarios();
        });
        it('Run Report Faield', function () {
            Senarios.FailedScienarios();
        });




});




