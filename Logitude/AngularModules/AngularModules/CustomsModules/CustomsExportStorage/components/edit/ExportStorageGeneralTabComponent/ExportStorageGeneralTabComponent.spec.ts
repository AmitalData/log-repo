import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditExportStorageComponent } from './ExportStorageGeneralTabComponent';

describe('EditExportStorageComponent', () => {
  let component: EditExportStorageComponent;
  let fixture: ComponentFixture<EditExportStorageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditExportStorageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditExportStorageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
