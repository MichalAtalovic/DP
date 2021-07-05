import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnumsActionPanelComponent } from './enums-action-panel.component';

describe('EnumsActionPanelComponent', () => {
  let component: EnumsActionPanelComponent;
  let fixture: ComponentFixture<EnumsActionPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EnumsActionPanelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EnumsActionPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
