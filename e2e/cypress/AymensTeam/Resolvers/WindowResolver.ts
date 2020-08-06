import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class WindowResolver extends AbstractResolver implements IResolver {

    private childPath: string;
    public ChildPath(value: string) {
        this.childPath = value;
        return this;
    }

    Reset() {
        super.Reset();
        this.childPath = null;
    }

    public ShouldBeOpend() {
        const selector = '.LogitudeWindow';

        if (this.index > 0) {
            cy.get(this.GetContainer()).find(selector).should('have.length', (this.index + 1));
        }

        else {
            cy.get(this.GetContainer()).find(selector).eq(this.index);
        }

        this.Reset();
    }

    public ShouldBeClosed() {
        const selector = '.LogitudeWindow';

        if (this.index > 0) {
            cy.get(this.GetContainer()).find(selector).should('have.length', this.index);
        }

        else {
            cy.get(this.GetContainer()).find(selector).should('not.exist');
        }

        this.Reset();
    }

    public ShouldBeOpendWithPath(ngComponentPath: string, indexOfWindow: number = 0) {
        const selector = 'div[code=\"' + ngComponentPath + '\"]';
        cy.get(selector).eq(indexOfWindow);
    }

    public ShouldBeClosedWithPath(ngComponentPath: string, indexOfWindow: number = 0) {
        const selector = 'div[code=\"' + ngComponentPath + '\"]';
        //cy.get('body').find(selector).eq(indexOfWindow).should('not.exist');


        cy.get(selector).should('have.length', 0)

    }

    public CloseOk() {

        cy.get(this.GetContainer()).find('.LogitudeWindow').eq(this.index).then((element) => {
            cy.wrap(element).find('.RedButton').contains('Ok', { matchCase: false }).click();
        });

        this.Reset();
    }

    public HasValidationError(error: string) {

        cy.get(this.GetContainer()).find('.LogitudeWindow').eq(this.index).then((element) => {
            cy.wrap(element).find('validationsummary').contains(error);
        });

        this.Reset();
    }
}