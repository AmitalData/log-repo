import {browser,by,element} from 'protractor';
import{FieldsHelper} from '../Helpers/FieldsHelper'
import{ GeneralFunctions } from'../Helpers/GeneralFunctions';

describe('contacts',function(){
let x:FieldsHelper=new FieldsHelper();
let z:GeneralFunctions=new GeneralFunctions();

it('',function(){
z.GoToMainMenu('General.MH.Contacts');
browser.driver.sleep(5000);
});


})