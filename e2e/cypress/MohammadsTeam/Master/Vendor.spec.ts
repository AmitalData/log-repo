


import { VendorGLAccount } from './Vendor';
import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';
import { LoginComp } from "../../login/Login.po";

export class GLASpec {

  private login: LoginComp = new LoginComp();
}
describe('New Vendor GlAccount ', () => {

  let v: VendorGLAccount = new VendorGLAccount();
let l: Login= new Login();
let R: CreateRandom= new CreateRandom();


  it('New Vendor GlAccount Created Successfully', function () {

      var str = R.createrandomnum();
      l.dologin();
     cy.get('li[id=GeneralMHMaintenance]').click()
   
cy.get('li[id=PAR]')
      v.CreateNewVendor("Test Vendor" + str);
      v.ActivateVendorGlaccount()

     
      
  });
});





