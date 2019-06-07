import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PricelistModificationComponent } from './pricelist-modification.component';

describe('PricelistModificationComponent', () => {
  let component: PricelistModificationComponent;
  let fixture: ComponentFixture<PricelistModificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PricelistModificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PricelistModificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
