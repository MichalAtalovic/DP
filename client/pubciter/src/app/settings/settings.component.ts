import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { HardResetDialogComponent } from '../dialogs/hard-reset-dialog/hard-reset-dialog.component';
import { AuthorService } from '../services/author.service';
import { SettingsService } from '../services/settings.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  public panelData = { header: 'Settings', iconPath: 'assets/settings_fade.png'}
  public author: any = null;

  constructor(
    public authorService: AuthorService,
    public settingService: SettingsService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.authorService.getAuthors().then(data => {
      if (data && (data as any[]).length > 0) {
        this.author = (data as any[])[0];
      }
    });
  }

  changeLibraryViewType(args: any) {
    this.author.settings.libraryTableView = args.value;
  }

  hardDataReset() {
    this.settingService.hardReset().then(() => {
      console.log('HARD RESET SUCCESSFULLY PERFORMED');
    });
  }

  save() {
    this.authorService.updateAuthor(this.author).then(author => {
      if (author != null) {
        console.log('Author settings updated');
      }
      else {
        console.log('Error: Author settings not updated');
      }
    });
  }

  openHardResetDialog(): void {
    const dialogRef = this.dialog.open(HardResetDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'hard reset') {
        this.hardDataReset();
      }
    });
  }

}
