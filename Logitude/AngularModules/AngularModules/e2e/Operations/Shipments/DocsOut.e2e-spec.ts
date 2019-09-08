import { browser, by, element } from 'protractor';
import { DocOutSenario } from './DocOutSenario';

describe('DocsOut', () => {

  let DocOutSenarios: DocOutSenario = new DocOutSenario();


  beforeEach(() => {

  });
  browser.ignoreSynchronization = true;

  it('OpenDocOutTab', function () {
    DocOutSenarios.OpenDocOutTab();

  });

  it('Successfully Printing Document', function () {
    DocOutSenarios.SuccessfullyPrintingDocument();

  });

  it('Failing Printing Document', function () {
    DocOutSenarios.FailingPrintingDocument();


  });
});
