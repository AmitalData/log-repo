export class ListItem {
    public Code: string;
    public Name: string;

    constructor(code: string) {
        this.Code = code;
        this.Name = code?.split(/(?=[A-Z])/)?.join(" ");
    }
}