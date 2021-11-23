import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuoteGeneralComponent } from './new-quote-general.component';

describe('NewQuoteGeneralComponent', () => {
  let component: NewQuoteGeneralComponent;
  let fixture: ComponentFixture<NewQuoteGeneralComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuoteGeneralComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuoteGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
