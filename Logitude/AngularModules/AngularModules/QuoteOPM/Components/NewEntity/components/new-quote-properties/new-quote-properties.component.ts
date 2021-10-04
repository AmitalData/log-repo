import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-new-quote-properties',
  templateUrl: './new-quote-properties.component.html',
  styleUrls: ['./new-quote-properties.component.scss']
})
export class NewQuotePropertiesComponent implements OnInit {
  @Input() formGroup: FormGroup = new FormGroup({});
  
  keyUp:any;

  getewaySelected: string[] = []
  getewayList: string[] = ['a', 'b']

  airlineSelected: string[] = []
  airlineList: string[] = ['a', 'b']

  destinationSelected: string[] = []
  destinationList: string[] = ['a', 'b']

  incotermSelected: string[] = []
  incotermList: string[] = ['a', 'b']


  constructor(
  ) {}

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('geteway'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('geteway', new FormControl(''));
    this.formGroup.addControl('airline', new FormControl(''));
    this.formGroup.addControl('sepcialServiceType', new FormControl(''));
    this.formGroup.addControl('destination', new FormControl(''));
    this.formGroup.addControl('incoterm', new FormControl(''));
  }

  getewaySearch(event: any) {
    this.getewaySelected = this.getewayList.filter(x => x.includes(event.query));
  }

  airlineSearch(event: any) {
    this.airlineSelected = this.airlineList.filter(x => x.includes(event.query));
  }

  destinationSearch(event: any) {
    this.destinationSelected = this.destinationList.filter(x => x.includes(event.query));
  }

  incotermSearch(event: any) {
    this.incotermSelected = this.incotermList.filter(x => x.includes(event.query));
  }
}
