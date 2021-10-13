import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuoteDeliveryComponent } from './new-quote-delivery.component';

describe('NewQuoteDeliveryComponent', () => {
  let component: NewQuoteDeliveryComponent;
  let fixture: ComponentFixture<NewQuoteDeliveryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuoteDeliveryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuoteDeliveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
