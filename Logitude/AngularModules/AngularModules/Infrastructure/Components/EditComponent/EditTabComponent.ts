import {Component, ViewChild, ViewContainerRef, ChangeDetectorRef, AfterViewInit}  from '@angular/core';
import {SessionLocator} from '../../Utilities/SessionLocator';

@Component({
    
    templateUrl: './EditTabComponent.html',
})

export class EditTabComponent implements AfterViewInit {
    public TabCode: string;
  public ComponentPath: string;
  public ComponentRef: any;
  @ViewChild('Child', { read: ViewContainerRef, static: false }) ViewContainerRef: ViewContainerRef;
  public ComponentInst: any;
    constructor(private ChangeDetectorRef: ChangeDetectorRef) {

    }

    Run(tabCode: string, componentPath: string) {
        this.TabCode = tabCode;
        this.ComponentPath = componentPath;
        this.LoadComponent();
    }

    private isViewEnitied: boolean = false;
    ngAfterViewInit() {
        this.isViewEnitied = true;
        this.LoadComponent();
    }

    
    public CurrentlySelected: boolean = false;
    LoadComponent() {
        if (this.ComponentPath && this.isViewEnitied) {

            SessionLocator.DynamicLoader.Load(this.ComponentPath, this.ViewContainerRef)
                .then(cmpRef => {
                    this.Selected = true;
                    this.ComponentInst = cmpRef.instance;
                    this.ComponentRef = cmpRef;
                });
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
