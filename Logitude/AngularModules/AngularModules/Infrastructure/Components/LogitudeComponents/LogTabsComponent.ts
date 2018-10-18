declare var window: any;
import {Input, Output, Component, OnInit, OnChanges, EventEmitter, QueryList, AfterViewInit, OnDestroy, ChangeDetectorRef, ViewChild, ViewChildren} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {SessionComponent} from '../Session/SessionComponent';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {LocationDirective} from '../../Utilities/LocationDirective';
import {AppTool} from '../../Tools';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    selector: 'LogTabs',
    templateUrl: "./LogTabsComponent.html",
})
export class LogTabsComponent implements AfterViewInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    @Input() TabsSource: LogTab[] = [];
    @Input() Disabled: boolean = false;
    @Input() NoBorder: boolean = false;
    @Input() IsFixedTabs: boolean = false; // disable new tab button
    @Input() HideCloseButton: boolean = false;
    
    @Output() AddTabClicked: EventEmitter<any> = new EventEmitter;
    @Output() CloseTabClicked: EventEmitter<any> = new EventEmitter;
    @Output() SelectedTabChanged: EventEmitter<any> = new EventEmitter;
    IsRTL: boolean = false;
    IsOverCloseButton: boolean = false;

    constructor(private cd: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting != undefined) {
            this.IsRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl" ? true : false;
        }
    }

    private timerToken: any;
    ngAfterViewInit() {
        this.timerToken = setTimeout(() => {
            if (this.TabsSource.length > 0) {
                this.SelectedTab = this.TabsSource[0];

                //this.LoadTab(this.TabsSource[0]);
                //this.SelectedTabItem = this.TabsSource[0];
                //this.SelectedTabItem.IsSelected = true;
            }
        }, 10);
        ////this.cd.detectChanges();
        
    }

    selectedTab: LogTab;
    @Input()
    public get SelectedTab() {
        return this.selectedTab;
    }
    public set SelectedTab(tab: LogTab) {
        if (this.selectedTab != tab) {
            var t = tab;
            if (AppTool.IsNullOrEmpty(t)) return;
            if (t.IsSelected) return;
            this.TabsSource.forEach(item => { item.IsSelected = false; }); // Reset Selection
            tab.IsSelected = true;
            this.LoadTab(t);
            this.selectedTab = t;
        }
    }

    AddTab() {
        if (this.Disabled)
            return;
        this.resetSelection();
        this.AddTabClicked.emit();
        this.cd.detectChanges();
        this.SelectedTab = this.TabsSource[this.TabsSource.length - 1]; // select last tab
    }
    CloseTab(tab) {
        if (this.TabsSource.length <= 1) return;
        this.CloseTabClicked.emit(tab);
        this.cd.detectChanges();
        //this.SelectedTab = this.TabsSource[this.TabsSource.length - 1]; // select last tab
    }
    SelectionChanged(tabItem: LogTab) {
        if (this.IsOverCloseButton) return;
        //console.log(this.TabsSource);
        this.SelectedTab = tabItem;
        this.SelectedTabChanged.emit(tabItem);
    }

    LoadTab(tabItem: LogTab) {
        this.cd.detectChanges();
        let locs = this.AllLocations.toArray();
        let myLocation: LocationDirective = locs.filter(f => f.Code == tabItem.Code)[0];
        if (AppTool.IsNullOrEmpty(tabItem.ComponentPath)) {
            console.warn("No component path in tab!");
            return;
        }
        if (!tabItem.IsTabLoaded) {
            if (myLocation != null) {
                SessionLocator.DynamicLoader.Load(tabItem.ComponentPath,
                    myLocation.viewContainerRef).then(cmpRef => {
                        tabItem.IsTabLoaded = true;
                        cmpRef.instance.ComponentRef = cmpRef;
                        tabItem.ComponentReference = cmpRef.instance;
                        cmpRef.instance.SetTabArgs({
                            EntityPM: tabItem.EntityPM,
                            Tab: tabItem,
                            Disabled: this.Disabled,
                            Parent: tabItem.Parent,
                            DecErrors: tabItem.DecErrors,
                        });
                    });
            }
        }
    }

    resetSelection() {
        //unselect all
        this.TabsSource.forEach(item => {
            item.IsSelected = false;
        });
        this.SelectedTab = null;
    }

    onListMouseDown(event,tab) {
        console.log(event);
        if (!AppTool.IsNullOrEmpty(event)) {
            if (event.which == '2') { // mouse wheel click
                //this.CloseTab(tab);
            }
        }
    }
}

export class LogTab {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    public DecErrors: any;
    public EntityPM: any;
    public Parent: any;
    public Code: string;
    public Header: string;
    //public ComponentName: string;
    public ComponentPath: string;
    public Index: number;
    public IsSelected: boolean = false;
    public IsTabLoaded: boolean = false;
    public SessionComponent: SessionComponent;
    public ComponentReference: any;
    constructor() {
        this.Index = SessionLocator.Index;
    }

    SetHeader(myHeader: string) {
        this.Header = myHeader;
    }

    //public IconSource: string = null;
    //public IconSourceGray: string = null;
    //SetIcon(myIcon: string) {
    //    this.IconSource = "./Images/Menu/" + myIcon + ".png";
    //    this.IconSourceGray = "./Images/Menu/" + myIcon + ".Gray.png";
    //}
}

