import {
  Directive, Input, ViewContainerRef, ComponentFactoryResolver,
  ComponentFactory, ComponentRef, Output, EventEmitter, HostListener, ElementRef,
  AfterViewInit
} from '@angular/core';
import { SearchListDDLComponent } from './SearchListDDLComponent';
import { FastSearchSettings } from 'Customs/Services/WebServices/AzureSearchWebService';

@Directive({
  selector: '[appSearchListDDL]'
})
export class SearchListDDLDirective implements  AfterViewInit {
  @Input() set appSearchListDDL(options: any[]) {
    if (this.componentRef && options) {
      this.componentRef.instance.dropdownOptions = options;
      this.componentRef.instance.showDropdown = true;
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

  ngAfterViewInit() {
    const factory: ComponentFactory<SearchListDDLComponent> = this.componentFactoryResolver.resolveComponentFactory(SearchListDDLComponent);
    this.componentRef = this.viewContainerRef.createComponent(factory);
    
    if (!this.componentRef) return;
    this.componentRef.instance.dropdownOptions = this.appSearchListDDL;
    this.componentRef.instance.settings = this._settings;
    this.componentRef.instance.optionSelected.subscribe((option: any) => this.optionSelected.emit(option));
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elRef.nativeElement.contains(event.target) && this.componentRef?.instance)
      this.componentRef.instance.showDropdown = false;
  }
}
