import {List} from './List';

export class DirectionTransportFilter {
    public filterTitle: string;
    public filterDirectionID: string;
    public filterTransportID: string;

    public get FilterTitle() { return this.filterTitle; }
    public set FilterTitle(newValue: string) { this.filterTitle = newValue; }
    public get FilterDirectionID() { return this.filterDirectionID; }
    public set FilterDirectionID(newValue: string) { this.filterDirectionID = newValue; }
    public get FilterTransportID() { return this.filterTransportID; }
    public set FilterTransportID(newValue: string) { this.filterTransportID = newValue; }



    constructor(title: string, directionID: string, transportID: string) {
        this.FilterTitle = title;
        this.FilterDirectionID = directionID;
        this.FilterTransportID = transportID;

    }


    public static myList() {
        var list: List<DirectionTransportFilter> = new List<DirectionTransportFilter>();
        var f0: DirectionTransportFilter = new DirectionTransportFilter("All", "", "");
        var f1: DirectionTransportFilter = new DirectionTransportFilter("Export", "E", "");
        var f2: DirectionTransportFilter = new DirectionTransportFilter("Import", "I", "");
        var f3: DirectionTransportFilter = new DirectionTransportFilter("Export Air", "E", "A");
        var f4: DirectionTransportFilter = new DirectionTransportFilter("Export Ocean", "E", "O");
        var f5: DirectionTransportFilter = new DirectionTransportFilter("Export Inland", "E", "I");
        var f6: DirectionTransportFilter = new DirectionTransportFilter("Import Air", "I", "A");
        var f7: DirectionTransportFilter = new DirectionTransportFilter("Import Ocean", "I", "O");
        var f8: DirectionTransportFilter = new DirectionTransportFilter("Import Inland", "I", "I");
        var f9: DirectionTransportFilter = new DirectionTransportFilter("Domestic Air", "D", "A");
        var f10: DirectionTransportFilter = new DirectionTransportFilter("Domestic Ocean", "D", "O");
        var f11: DirectionTransportFilter = new DirectionTransportFilter("Domestic Inland", "D", "I");
        var f12: DirectionTransportFilter = new DirectionTransportFilter("Drop Air", "R", "A");
        var f13: DirectionTransportFilter = new DirectionTransportFilter("Drop Ocean", "R", "O");
        var f14: DirectionTransportFilter = new DirectionTransportFilter("Drop Inland", "R", "I");
        list.add(f0);
        list.add(f1);
        list.add(f2);
        list.add(f3);
        list.add(f4);
        list.add(f5);
        list.add(f6);
        list.add(f7);
        list.add(f8);
        list.add(f9);
        list.add(f10);
        list.add(f11);
        list.add(f12);
        list.add(f13);
        list.add(f14);
        return list;




    }
}
