import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuarantineActionPanelComponent } from './quarantine-action-panel.component';

describe('QuarantineActionPanelComponent', () => {
  let component: QuarantineActionPanelComponent;
  let fixture: ComponentFixture<QuarantineActionPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuarantineActionPanelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuarantineActionPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
