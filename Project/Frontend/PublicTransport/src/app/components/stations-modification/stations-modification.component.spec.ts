import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StationsModificationComponent } from './stations-modification.component';

describe('StationsModificationComponent', () => {
  let component: StationsModificationComponent;
  let fixture: ComponentFixture<StationsModificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StationsModificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StationsModificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
