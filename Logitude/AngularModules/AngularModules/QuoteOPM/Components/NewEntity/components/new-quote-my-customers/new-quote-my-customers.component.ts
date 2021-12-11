import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-new-quote-my-customers',
  templateUrl: './new-quote-my-customers.component.html',
  styleUrls: ['./new-quote-my-customers.component.scss']
})
export class NewQuoteMyCustomersComponent implements OnInit {
  @Input() formGroup: FormGroup = new FormGroup({});

  customers1Selected: string[] = []
  customers1List: string[] = ['a', 'b']

  customers2Selected: string[] = []
  customers2List: string[] = ['a', 'b']

  keyUp:any;
  
  constructor(
  ) { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('customers1'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('customers1', new FormControl(''));
    this.formGroup.addControl('customers2', new FormControl(''));
  }

  searchCustomers1(event: any) {
    this.customers1Selected = this.customers1List.filter(x => x.includes(event.query));
  }

  searchCustomers2(event: any) {
    this.customers2Selected = this.customers2List.filter(x => x.includes(event.query));
  }
}
