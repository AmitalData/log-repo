



import { CreateRandom } from './CreateRandom';
import { BankAccount } from './BankAccount';
import { LoginComp } from "../../login/Login.po";

export class BankSpec {

    private login: LoginComp = new LoginComp();
  }
describe('New Bank Account ', () => {

    let B: BankAccount = new BankAccount();

    let R: CreateRandom = new CreateRandom();


    it('New Bank Account Created Successfully', function () {

        var str = R.createrandomnum();
      
        cy.get('li[id=GeneralMHMaintenance]').click()

        cy.get('li[id=PAR]')
        var g = 'BankGLAccount' + str;
        var d = 'BankDeffGLAccount' + str;
        var t = 'BankTransGLAccount' + str;
        var ba= 'Bank'+str
        cy.get('li[id=GeneralMHFullAccounting]').click();
        cy.get('li[id=FAGLAccouts]').click();
        B.CreateBankGLAccount(g)
        B.CreateBankGLAccount(d)
        B.CreateBankGLAccount(t)

        B.CreateNewBankAccount(ba, g, d, t);
        cy.log('New Bank Creaed Successfully');


    });
});


