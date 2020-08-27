
import { Glaccount }  from './Glaccount'
import { RandomGenerator } from './RandomGenerator'

import { LoginComp } from "../../login/Login.po";

export class GLASpec {

  private login: LoginComp = new LoginComp();
}
describe('GLAccount Module', function () {
 

    let GL: Glaccount = new Glaccount();
    let R: RandomGenerator = new RandomGenerator();




    

  it(' New GLAccount Was Created And Updated', function () {
   

   // cy.get('li[id=PAR]',{timeout: 60000})
  
   cy.get('li[id="GeneralMHFullAccounting"]').click();
    cy.get('#FAGLAccouts').click();
   

  
    var GlaccountNumber = R.RandomNum();
    var name = 'My Auto GLAccount';

   GL.CreateNewGLAccount(name+GlaccountNumber);
   GL.EditGLAccount(name +GlaccountNumber);
   



  });
});
