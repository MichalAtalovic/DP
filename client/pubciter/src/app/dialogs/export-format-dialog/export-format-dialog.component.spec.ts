import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportFormatDialogComponent } from './export-format-dialog.component';

describe('ExportFormatDialogComponent', () => {
  let component: ExportFormatDialogComponent;
  let fixture: ComponentFixture<ExportFormatDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExportFormatDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportFormatDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
