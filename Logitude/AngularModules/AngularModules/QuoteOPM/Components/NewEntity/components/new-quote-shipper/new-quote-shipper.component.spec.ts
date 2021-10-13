import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuoteShipperComponent } from './new-quote-shipper.component';

describe('NewQuoteShipperComponent', () => {
  let component: NewQuoteShipperComponent;
  let fixture: ComponentFixture<NewQuoteShipperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuoteShipperComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuoteShipperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
