import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuotePickupComponent } from './new-quote-pickup.component';

describe('NewQuotePickupComponent', () => {
  let component: NewQuotePickupComponent;
  let fixture: ComponentFixture<NewQuotePickupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuotePickupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuotePickupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
