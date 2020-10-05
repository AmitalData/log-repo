import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class RadioButtonResolver extends AbstractResolver implements IResolver {


    public Select() {

  
        cy.get(this.GetContainer())
            .find('.LogitudeRadioButton')
            .within(() =>
                cy.get(this.selector).click({ force: true })
            )
        //this.Reset();
    }


}