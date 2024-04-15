import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailsFrameComponent } from './details-frame.component';

describe('DetailsFrameComponent', () => {
  let component: DetailsFrameComponent;
  let fixture: ComponentFixture<DetailsFrameComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailsFrameComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DetailsFrameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
