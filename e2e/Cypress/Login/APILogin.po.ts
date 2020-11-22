/// <reference types="cypress" />


import {AppTool} from '../MohammadsTeam/Helper/AppTool'
import {APILoginHelper} from './APIHelperMethode/APILoginHelper.po'
export class APILoginComp {
 

}
 
it('APILogin Successfully', () => {
  let waited = false
  var aPILoginHelper = new APILoginHelper();
      aPILoginHelper.APILoginSubmit().then((str) => {});
      cy.wrap(null).then(() => {
        return aPILoginHelper.APILoginSubmit().then((str) => {
          expect(str).to.eq('foo')
          expect(waited).to.be.true
        })
      })
});
 

