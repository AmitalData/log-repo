import {
  Directive, Input, OnInit, ViewContainerRef, ComponentFactoryResolver,
  ComponentFactory, ComponentRef, Output, EventEmitter, HostListener, ElementRef
} from '@angular/core';
import { SearchListDDLComponent } from './SearchListDDLComponent';

@Directive({
  selector: '[appSearchListDDL]'
})
export class SearchListDDLDirective implements OnInit {
  @Input() set appSearchListDDL(options: any[]) {
    if (this.componentRef) {
      this.componentRef.instance.dropdownOptions = options;
      this.componentRef.instance.showDropdown = options.length > 0;
    }
  }

  private _maxResults: number = null;
  @Input() set maxResults(max: number) {
    this._maxResults = max;
    if (this.componentRef)
      this.componentRef.instance.maxResults = max;
  }

  private _displayPattern: string = '';
  @Input() set displayPattern(pattern: string) {
    this._displayPattern = pattern;
    if (this.componentRef)
      this.componentRef.instance.displayPattern = this._displayPattern;
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
    this.componentRef.instance.maxResults = this._maxResults;
    this.componentRef.instance.displayPattern = this._displayPattern;
    this.componentRef.instance.optionSelected.subscribe((option: any) => this.optionSelected.emit(option));
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elRef.nativeElement.contains(event.target))
      this.componentRef.instance.showDropdown = false;
  }
}
