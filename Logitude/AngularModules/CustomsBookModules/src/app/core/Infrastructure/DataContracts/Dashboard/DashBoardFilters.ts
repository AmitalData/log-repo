export class DashBoardFilters {

    private index: string;
    private title: string;

    constructor(title: string, index: string) {
        this.title = title;
        this.index = index;
    }

    public get Index() { return this.index; }
    public set Index(value: string) { this.index = value; }

    public get Title() { return this.title; }
    public set Title(value: string) { this.title = value; }
}