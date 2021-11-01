import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitationCategoryDialogComponent } from './citation-category-dialog.component';

describe('CitationCategoryDialogComponent', () => {
  let component: CitationCategoryDialogComponent;
  let fixture: ComponentFixture<CitationCategoryDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CitationCategoryDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitationCategoryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
