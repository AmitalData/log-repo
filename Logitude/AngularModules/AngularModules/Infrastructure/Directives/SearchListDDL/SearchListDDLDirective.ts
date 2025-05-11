import {
  Directive, Input, OnInit, ViewContainerRef, ComponentFactoryResolver,
  ComponentFactory, ComponentRef, Output, EventEmitter, HostListener, ElementRef
} from '@angular/core';
import { SearchListDDLComponent } from './SearchListDDLComponent';
import { FastSearchSettings } from 'Customs/Services/WebServices/AzureSearchWebService';

@Directive({
  selector: '[appSearchListDDL]'
})
export class SearchListDDLDirective implements OnInit {
  @Input() set appSearchListDDL(options: any[]) {
    if (this.componentRef && options) {
      this.componentRef.instance.dropdownOptions = options;
      this.componentRef.instance.showDropdown = options.length > 0;
    }
  }

  private _settings: FastSearchSettings = null;
  @Input() set DDLsettings(val: FastSearchSettings) {
    this._settings = val;
    if (this.componentRef && val)
      this.componentRef.instance.settings = this._settings;
  }

  @Output() optionSelected: EventEmitter<any> = new EventEmitter<any>();
  componentRef!: ComponentRef<SearchListDDLComponent>;

  constructor(
    private viewContainerRef: ViewContainerRef,
    private componentFactoryResolver: ComponentFactoryResolver,
    private elRef: ElementRef
  ) { }

  ngOnInit() {
    const factory: ComponentFactory<SearchListDDLComponent> = this.componentFactoryResolver.resolveComponentFactory(SearchListDDLComponent);
    this.componentRef = this.viewContainerRef.createComponent(factory);
    this.componentRef.instance.dropdownOptions = this.appSearchListDDL;
    this.componentRef.instance.settings = this._settings;
    this.componentRef.instance.optionSelected.subscribe((option: any) => this.optionSelected.emit(option));
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elRef.nativeElement.contains(event.target))
      this.componentRef.instance.showDropdown = false;
  }
}
