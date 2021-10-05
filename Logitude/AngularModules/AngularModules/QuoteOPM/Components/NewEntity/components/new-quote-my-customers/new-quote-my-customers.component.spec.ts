import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuoteMyCustomersComponent } from './new-quote-my-customers.component';

describe('NewQuoteMyCustomersComponent', () => {
  let component: NewQuoteMyCustomersComponent;
  let fixture: ComponentFixture<NewQuoteMyCustomersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuoteMyCustomersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuoteMyCustomersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
