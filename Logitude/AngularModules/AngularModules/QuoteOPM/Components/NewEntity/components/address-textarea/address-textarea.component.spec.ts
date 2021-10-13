import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressTextareaComponent } from './address-textarea.component';

describe('AddressTextareaComponent', () => {
  let component: AddressTextareaComponent;
  let fixture: ComponentFixture<AddressTextareaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddressTextareaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressTextareaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
