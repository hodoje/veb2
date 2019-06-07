import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinesModificationComponent } from './lines-modification.component';

describe('LinesModificationComponent', () => {
  let component: LinesModificationComponent;
  let fixture: ComponentFixture<LinesModificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinesModificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinesModificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
