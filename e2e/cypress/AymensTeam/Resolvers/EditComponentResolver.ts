import { IResolver } from './Abstractions/IResolver';
import { AbstractResolver } from './Abstractions/AbstractResolver';

export class EditComponentResolver extends AbstractResolver implements IResolver {

    private tab: string;
    public Tab(value: string) {
        this.tab = value;
        return this;
    }

    Reset() {
        super.Reset();
        this.tab = null;
    }

    public ShouldBeOpend() {
        const selector = '.LogitudeEditComponent';

        if (this.index > 0) {
            cy.get(this.GetContainer()).find(selector).should('have.length', (this.index + 1));
        }

        else {
            cy.get(this.GetContainer()).find(selector).eq(this.index);
        }

        this.Reset();
    }

    public ShouldBeClosed() {
        const selector = '.LogitudeEditComponent';

        if (this.index > 0) {
            cy.get(this.GetContainer()).find(selector).should('have.length', this.index);
        }

        else {
            cy.get(this.GetContainer()).find(selector).should('not.exist');
        }

        this.Reset();
    }

    public ShouldBeSelected() {

        const text = this.tab;

        cy.get(this.GetContainer())
            .find('.LogitudeEditComponent')
            .eq(this.index)
            .within(() => {
                cy.get('li.SelectedMenuItem').contains(text);
                cy.get('.TabTitleRow').contains(text);
            });

        this.Reset();
    }

    public HasValidationError(error: string) {

        cy.get(this.GetContainer()).find('.LogitudeEditComponent').eq(this.index).then((element) => {
            cy.wrap(element).find('validationsummary').contains(error);
        });

        this.Reset();
    }

    public Select() {

        const text = this.tab;

        cy.get(this.GetContainer())
            .find('.LogitudeEditComponent')
            .eq(this.index).within(() => {
                cy.get('li.DefaultMenuItem').contains(text).click({ force: true });
        });

        this.Reset();
    }

    public Close() {
        cy.get(this.GetContainer()).find('.LogitudeEditComponent').eq(this.index).then((element) => {
            cy.wrap(element).find('BackButton').click();
        });
        this.Reset();
    }
}