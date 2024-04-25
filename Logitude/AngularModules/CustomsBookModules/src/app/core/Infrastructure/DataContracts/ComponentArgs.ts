
export class ComponentArgs {
    public static CurrentComponentKey: string;
    public static ComponentLists: Array<any>;
    public static AddComponent(component: any) {
        if (ComponentArgs.ComponentLists == null) {
            ComponentArgs.ComponentLists = new Array<any>();
        }

        var item = ComponentArgs.ComponentLists.filter(d => d.key == component.key)[0];

        if (!item) {
            ComponentArgs.ComponentLists.push(component);
        }
        else {
            item.Component = component.Component;
        }

    }

    constructor() {

    }
}
