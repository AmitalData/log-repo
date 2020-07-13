
import { RandomGenerator } from './RandomGenerator'
import { ChartOfAccount } from './ChartOfAccount'
import { Login } from './Login';

import { LoginComp } from "../../login/Login.po";

export class ChartSpec {

  private login: LoginComp = new LoginComp();
}

describe('ChartOfAccount Module', function () {


  let R: RandomGenerator = new RandomGenerator();
  let C: ChartOfAccount = new ChartOfAccount();
  let log: Login = new Login();
  it('Chart Of Account Success', function () {  
   
    cy.get('li[id=GeneralMHMaintenance]').click();
    var chartOfAccountNo = R.GenerateRandomNumber();
    C.CreateNewChartOFAccount(chartOfAccountNo, 'Customer');
    C.EditChartOFAccount(chartOfAccountNo);




  });

});