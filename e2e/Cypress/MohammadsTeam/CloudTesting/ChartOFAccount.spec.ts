
import { RandomGenerator } from './RandomGenerator'
import { ChartOfAccount } from './ChartOfAccount'
import { LoginCloud } from './LoginCloud';

describe('ChartOfAccount Module', function () {


  let R: RandomGenerator = new RandomGenerator();
  let C: ChartOfAccount = new ChartOfAccount();
  let log: LoginCloud = new LoginCloud();
  it('Chart Of Account Success', function () {  
    log.dologin();
    cy.get('li[id=GeneralMHMaintenance]').click();
    var chartOfAccountNo = R.GenerateRandomNumber();
    C.CreateNewChartOFAccount(chartOfAccountNo, 'Customer');
    C.EditChartOFAccount(chartOfAccountNo);




  });

});