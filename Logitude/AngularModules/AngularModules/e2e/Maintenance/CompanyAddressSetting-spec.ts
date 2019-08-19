import { browser, by, element } from 'protractor';
import { CompanyAddressSetting } from './CompanyAddressSetting';
import { CompanyAddressSettingScenario } from './CompanyAddressSettingScenario';
import { LoginComp } from "../login/Login.po";




describe('CompanyAddressSetting', () => {

    let CompanyAddress: CompanyAddressSetting = new CompanyAddressSetting();
    let CompanyAddressScenario: CompanyAddressSettingScenario = new CompanyAddressSettingScenario();

    beforeEach(() => {

    });

    browser.ignoreSynchronization = true;


    it('Quicksearch', function () {

        CompanyAddress.QuickSearch();
        //  CompanyAddressScenario.Quicksearch();
    });


    it('SearchCompanyAddressSetting', function () {

        CompanyAddress.SearchCompanyAddressSetting();
        //CompanyAddressScenario.SearchCompanyAddressSetting();
    });


    it('EditCompanyAddressSitting', function () {

        CompanyAddress.EditCompanyAddressSitting();
        // CompanyAddressScenario.EditCompanyAddressSitting();

    });

});