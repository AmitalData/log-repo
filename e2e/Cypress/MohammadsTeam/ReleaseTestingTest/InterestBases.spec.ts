



//import { Login } from './Login';
import { RandomGenerator } from './RandomGenerator'

import { LoginComp } from "../../login/Login.po";
export class InterestSpec {

  private login: LoginComp = new LoginComp();
}
describe('New Interest Base ', () => {

 
//let l: Login= new Login();

let R: RandomGenerator = new RandomGenerator();

  it('New InterestBase Created Successfully', function () {

      
    
      var Inteerstcode = R.GenerateRandomNumberForInterest();
      cy.get('li[id=GeneralMHFullAccounting]').click();
    
      cy.get('li[id=FAInterest]').click();
      cy.get('button[id=NewInterestBase]').click();

      cy.get('input[id=InterestBasesType_Code]').type(Inteerstcode);
      cy.get('input[id=InterestBasesType_LocalName]').type("INTLocalName");
      cy.get('input[id=InterestBasesType_EnglishName]').type("INTEnglishName");
     
      cy.get('#Add').click();
      cy.get('input[id=InterestBasesPeriod_InterestRate]').type("12");
      cy.get('input[id=date_InterestBasesPeriod_InterestBaseStartDate]').type("1/1");
      cy.get('#OkInterest').click();
      cy.get('#CREATEINTERESTBASES').click();
      cy.get('li[id="InterestBasesTypeTHDetails"]').click();
      cy.get('#Edit').click();

      cy.get('#date_InterestBasesPeriod_InterestBaseStartDate').type("1/2");
      cy.get('#OkInterest').click();

      cy.get('#InterestBasesType-SaveClose').click();

     
      
    });
      
 
});



