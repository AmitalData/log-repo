import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BtnMenuPlusComponent } from './btn-menu-plus.component';

describe('BtnMenuPlusComponent', () => {
  let component: BtnMenuPlusComponent;
  let fixture: ComponentFixture<BtnMenuPlusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BtnMenuPlusComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BtnMenuPlusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
