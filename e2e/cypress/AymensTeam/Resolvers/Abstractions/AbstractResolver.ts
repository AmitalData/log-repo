import { Resolvers } from "../Resolvers";


export abstract class AbstractResolver {

    //protected Session: number = 0;

    protected index: number = 0;
    public Index(value: number) {
        this.index = value;
        return this;
    }

    protected parent: string;
    public Parent(value: string) {
        this.parent = value;
        return this;
    }

    protected selector: string;
    public Selector(value: string) {
        this.selector = value;
        return this;
    }

    protected Reset() {
        this.index = 0;
        this.parent = null;
        this.selector = null;
    }

    protected GetContainer(): string {

        cy.get('body').find('SessionComponent').eq(Resolvers.Session).as('Container');

        if (this.parent) {
            cy.get('@Container').find(this.parent).eq(0).as('Container');
        }

        return '@Container';
    }    
}