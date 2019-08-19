import { browser, by, element } from 'protractor';
import { CompanyAddressSetting } from './CompanyAddressSetting';
//import { SenarioTest } from './CompanyAddressSettingScenario';
import { LoginComp } from "../login/Login.po";




describe('CompanyAddressSetting', () => {

    let CompanyAddress: CompanyAddressSetting = new CompanyAddressSetting();

    beforeEach(() => {

    });

    browser.ignoreSynchronization = true;


    it('Quicksearch',function(){

      CompanyAddress.QuickSearch();
    });

    it('SearchCompanyAddressEtting',function(){

    CompanyAddress.SearchCompanyAddressEtting();
    });

    it('EditCompanyAddressSitting',function(){

        CompanyAddress.EditCompanyAddressSitting();
        });

 });