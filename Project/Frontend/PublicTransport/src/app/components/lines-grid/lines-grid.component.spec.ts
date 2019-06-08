import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinesGridComponent } from './lines-grid.component';

describe('LinesGridComponent', () => {
  let component: LinesGridComponent;
  let fixture: ComponentFixture<LinesGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinesGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinesGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
