import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../Utilities/LocationDirective';
import { PrintingRow } from '../../Services/BatchPrintService';

@Component({
    selector: 'MultiPrintMainComponent',
    templateUrl: 'MultiPrintMainComponent.html',
})

export class MultiPrintMainComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private PageChild_MTUP: any = null;
    private PageChild_MTHE: any = null;
    public PrintingRows: PrintingRow[];
    public DocumentId: string;
    WindowArgs: any;

    constructor() {
        this.PrintingRows = [];
    }

    ngOnInit() {
    }

    SetWindowArgs(windowArgs) {
        this.WindowArgs = windowArgs;
        this.RunComponent();
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;
        if (this.timerToken) clearTimeout(this.timerToken);
        if (this.Retries < 20) this.timerToken = setTimeout(() => this.RunComponent(), 1);
    }

    RunComponent() {
        if (!this.AllLocations) this.RunComponentTimer();
        if (this.AllLocations && this.AllLocations.toArray().length == 0) this.RunComponentTimer();
        else if (this.AllLocations) this.SetSelectedItem("MTUP");
    }

    SetSelectedItem(tabCode: string) {
        this.SelectedTabCode = tabCode;
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    SelectionChanged() {
        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
        if (myLocation == null) return;
        switch (this.SelectedTabCode) {
            //Print
            case "MTUP": {
                if (this.PageChild_MTUP == null) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/MultiPrint/PrintComponent', myLocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_MTUP = cmpRef.instance;
                            this.PageChild_MTUP.SetWindowArgs({ args: this.WindowArgs, parentComponent: this });
                        });
                }
                break;
            }
            //Handle Errors
            case "MTHE": {
                if (this.PageChild_MTHE == null) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/MultiPrint/MultiPrintErrorHandlerComponent', myLocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_MTHE = cmpRef.instance;
                            this.PageChild_MTHE.SetWindowArgs({ PrintingRows: this.PrintingRows, DocumentId: this.DocumentId });
                        });
                }
                break;
            }
        }
    }

    NextButtonClicked() {
        this.SelectedTabCode = "MTHE";
    }   
}
