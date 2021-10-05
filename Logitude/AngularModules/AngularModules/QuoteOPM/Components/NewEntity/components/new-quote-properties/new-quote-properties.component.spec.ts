import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuotePropertiesComponent } from './new-quote-properties.component';

describe('NewQuotePropertiesComponent', () => {
  let component: NewQuotePropertiesComponent;
  let fixture: ComponentFixture<NewQuotePropertiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuotePropertiesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuotePropertiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
