
export class CustomizationMainMenuItem {
    public TextCode: string;
    public Code: string;
    public HtmlView: string;
    public IconCode: string;
    public IconSource: string;
    public IconSelectedSource: string;
    public QuerySection: string;
    public ComponentPath: string;
    public Page: any = null;
    public screenArgs: any = {};
    public IsVisible: boolean;

    constructor(myIcon: string) {
        this.IconCode = myIcon;
        this.IconSource = "./Images/Customization/" + myIcon + ".png";
        this.IconSelectedSource = "./Images/Customization/" + myIcon + ".Selected.png";
    }
}
