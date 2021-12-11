import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuoteAddressComponent } from './new-quote-address.component';

describe('NewQuoteAddressComponent', () => {
  let component: NewQuoteAddressComponent;
  let fixture: ComponentFixture<NewQuoteAddressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuoteAddressComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuoteAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
