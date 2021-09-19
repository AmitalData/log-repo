import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuoteExpectedOrderComponent } from './new-quote-expected-order.component';

describe('NewQuoteExpectedOrderComponent', () => {
  let component: NewQuoteExpectedOrderComponent;
  let fixture: ComponentFixture<NewQuoteExpectedOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuoteExpectedOrderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuoteExpectedOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
