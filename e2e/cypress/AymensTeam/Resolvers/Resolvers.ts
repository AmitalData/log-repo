import { IResolver } from './Abstractions/IResolver';
import { WindowResolver } from "./WindowResolver";
import { TextBoxResolver } from "./TextBoxResolver";
import { MainMenuResolver } from "./MainMenuResolver";
import { GridViewResolver } from "./GridViewResolver";
import { LOVResolver } from "./LOVResolver";
import { ToggleButtonResolver } from "./ToggleButtonResolver";
import { ButtonResolver } from "./ButtonResolver";
import { SearchBoxResolver } from "./SearchBoxResolver";
import { EditComponentResolver } from "./EditComponentResolver";
import { DatePickerResolver } from "./DatePickerResolver";
import { RadioButtonResolver } from "./RadioButtonResolver";




export class Resolvers {
    public static Session: number = 0;
    private static iResolvers: IResolver[] = [];
    public static WindowResolver: WindowResolver = new WindowResolver();
    public static TextBoxResolver: TextBoxResolver = new TextBoxResolver();
    public static ButtonResolver: ButtonResolver = new ButtonResolver();
    public static MainMenuResolver: MainMenuResolver = new MainMenuResolver();
    public static GridViewResolver: GridViewResolver = new GridViewResolver();
    public static LOVResolver: LOVResolver = new LOVResolver();
    public static ToggleButtonResolver: ToggleButtonResolver = new ToggleButtonResolver();
    public static SearchBoxResolver: SearchBoxResolver = new SearchBoxResolver();
    public static EditComponentResolver: EditComponentResolver = new EditComponentResolver();
    public static DatePickerResolver: DatePickerResolver = new DatePickerResolver();
    public static RadioButtonResolver: RadioButtonResolver = new RadioButtonResolver();

    constructor() {
        Resolvers.iResolvers.push(Resolvers.WindowResolver);
        Resolvers.iResolvers.push(Resolvers.TextBoxResolver);
        Resolvers.iResolvers.push(Resolvers.MainMenuResolver);
        Resolvers.iResolvers.push(Resolvers.GridViewResolver);
        Resolvers.iResolvers.push(Resolvers.LOVResolver);
        Resolvers.iResolvers.push(Resolvers.ToggleButtonResolver);
        Resolvers.iResolvers.push(Resolvers.ButtonResolver);
        Resolvers.iResolvers.push(Resolvers.SearchBoxResolver);
        Resolvers.iResolvers.push(Resolvers.EditComponentResolver);
        Resolvers.iResolvers.push(Resolvers.DatePickerResolver);
        Resolvers.iResolvers.push(Resolvers.RadioButtonResolver);
    }

    public static ChangeParent(selector: string) {
        this.iResolvers.forEach((item: IResolver) => {
            //item.ChangeParent(selector);
        });
    }

    public static ResetParent() {
        this.iResolvers.forEach((item: IResolver) => {
            //item.ResetParent();
        });
    }

    public static OpenSession() {
        cy.get('.TabControlHead > ul > li.NewTabItem').click({ force: true });

        //TabControlHead
        this.Session++;
    }

    public static SelectSession(index:number) {
        cy.get('.TabControlHead > ul > li').eq(index).click({ force: true });
        this.Session = index;
    }

    public static GetSelectedSession() {
        var d = cy.get('.TabControlHead').find('li.Selected').then(($el) => {
            var x = $el.index();
            cy.log(x.toString());
        })
    }
}