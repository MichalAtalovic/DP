import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HardResetDialogComponent } from './hard-reset-dialog.component';

describe('HardResetDialogComponent', () => {
  let component: HardResetDialogComponent;
  let fixture: ComponentFixture<HardResetDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HardResetDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HardResetDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
