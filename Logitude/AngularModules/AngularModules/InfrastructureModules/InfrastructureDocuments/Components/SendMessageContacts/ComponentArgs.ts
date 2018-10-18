


export class SendToComponentArgs {
    // public static SendToComponentLists: any[];
    public static CurrentComponentKey: string;
    public static SendToComponentLists: Array<any>;
    public static AddComponent(sendToComponent: any) {
        if (SendToComponentArgs.SendToComponentLists == null) {
            SendToComponentArgs.SendToComponentLists = new Array<any>();
        }

        var item = SendToComponentArgs.SendToComponentLists.filter(d=> d.key == sendToComponent.key)[0];

        if (!item) {
            SendToComponentArgs.SendToComponentLists.push(sendToComponent);
        }
        else {
            item.Component = sendToComponent.Component;
        }

    }

    constructor() {

    }
}



