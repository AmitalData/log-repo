import {Component, ViewChild, ViewContainerRef, ChangeDetectorRef}  from '@angular/core';
import {SessionLocator} from '../../Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './EditTabComponent.html',
})

export class EditTabComponent {
    public TabCode: string;
  public ComponentPath: string;
  public ComponentRef: any;
  @ViewChild('Child', { read: ViewContainerRef }) ViewContainerRef: ViewContainerRef;
  public ComponentInst: any;
    constructor(private ChangeDetectorRef: ChangeDetectorRef) {

    }

    Run(tabCode: string, componentPath: string) {
        this.TabCode = tabCode;
        this.ComponentPath = componentPath;
        this.RunComponent();
    }

    private isLoaderReady: boolean = false;
    public CurrentlySelected: boolean = false;
    RunComponent() {

        if (this.ViewContainerRef) {
            SessionLocator.DynamicLoader.Load(this.ComponentPath, this.ViewContainerRef)
                .then(cmpRef => {
                  this.Selected = true;
                  this.ComponentInst = cmpRef.instance;
                  this.ComponentRef = cmpRef;
                });
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private selected: boolean = false;
    get Selected() { return this.selected; }
    set Selected(value: boolean) {
        if (this.selected != value) {
            this.selected = value;

            setTimeout(() => this.RunChangeDetector(), 1);
        }
    }

    RunChangeDetector() {
        if (this.Selected == true) {
            var d = this.TabCode;
            this.ChangeDetectorRef.reattach();
        }

        else {
            var d = this.TabCode;
            this.ChangeDetectorRef.detach();
        }
  }

  DestroyCurrentTab() {
    //this.ComponentInst.ngOnDestroy();
    this.ComponentRef.destroy();
    this.ComponentRef = null;
  }
}
