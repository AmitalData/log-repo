


import { VendorGLAccount } from './Vendor';

import  { CreateRandom } from './CreateRandom';
import { LoginComp } from "../../login/Login.po";

export class GLASpec {

  private login: LoginComp = new LoginComp();
}
describe('New Vendor GlAccount ', () => {

  let v: VendorGLAccount = new VendorGLAccount();

let R: CreateRandom= new CreateRandom();


  it('New Vendor GlAccount Created Successfully', function () {

      var str = R.createrandomnum();
 
      cy.get('li[id=GeneralMHMaintenance]').click({ force: true })
   
      cy.get('li[id=PAR]').click({ force: true })
      v.CreateNewVendor("Test Vendor" + str);
      v.ActivateVendorGlaccount()

     
      
  });
});





