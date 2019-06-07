import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulesModificationComponent } from './schedules-modification.component';

describe('SchedulesModificationComponent', () => {
  let component: SchedulesModificationComponent;
  let fixture: ComponentFixture<SchedulesModificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulesModificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulesModificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
