import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveCitationDialogComponent } from './remove-citation-dialog.component';

describe('RemoveCitationDialogComponent', () => {
  let component: RemoveCitationDialogComponent;
  let fixture: ComponentFixture<RemoveCitationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RemoveCitationDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RemoveCitationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
