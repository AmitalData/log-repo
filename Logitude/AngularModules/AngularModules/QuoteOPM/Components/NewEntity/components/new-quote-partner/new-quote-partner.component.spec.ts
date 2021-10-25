import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuotePartnerComponent } from './new-quote-partner.component';

describe('NewQuotePartnerComponent', () => {
  let component: NewQuotePartnerComponent;
  let fixture: ComponentFixture<NewQuotePartnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuotePartnerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuotePartnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
