


import { VendorGLAccount } from './Vendor';
import { LoginCloud } from './LoginCloud';
import  { CreateRandom } from './CreateRandom';

describe('New Vendor GlAccount ', () => {

  let v: VendorGLAccount = new VendorGLAccount();
let l: LoginCloud= new LoginCloud();
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





