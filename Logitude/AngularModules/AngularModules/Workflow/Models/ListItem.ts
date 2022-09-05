export class ListItem {
    public Code: string;
    public Name: string;

    constructor(code: string, name: string | null = null) {
        this.Code = code;
        this.Name = name ? name : (code ? code.split(/(?=[A-Z])/).join(" ") : null);
    }
}