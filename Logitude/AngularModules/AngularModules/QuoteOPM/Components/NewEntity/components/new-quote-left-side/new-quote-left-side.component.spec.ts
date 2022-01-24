import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewQuoteLeftSideComponent } from './new-quote-left-side.component';

describe('NewQuoteLeftSideComponent', () => {
  let component: NewQuoteLeftSideComponent;
  let fixture: ComponentFixture<NewQuoteLeftSideComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewQuoteLeftSideComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewQuoteLeftSideComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
