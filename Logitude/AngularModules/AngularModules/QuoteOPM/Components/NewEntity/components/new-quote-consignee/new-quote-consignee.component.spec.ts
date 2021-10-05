import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuoteConsigneeComponent } from './new-quote-consignee.component';

describe('NewQuoteConsigneeComponent', () => {
  let component: NewQuoteConsigneeComponent;
  let fixture: ComponentFixture<NewQuoteConsigneeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuoteConsigneeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuoteConsigneeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
