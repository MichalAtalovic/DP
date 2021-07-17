import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { NgpImagePickerModule } from 'ngp-image-picker';
import { ErrorStateMatcher, ShowOnDirtyErrorStateMatcher } from '@angular/material/core';
import { DateFormatPipe } from 'src/utils/date-format-pipe';
import { MyLibraryComponent } from './my-library/my-library.component';
import { QuarantineComponent } from './quarantine/quarantine.component';
import { SettingsComponent } from './settings/settings.component';
import { SyncComponent } from './sync/sync.component';
import { SideBarComponent } from './side-bar/side-bar.component';
import { PublicationCardComponent } from './my-library/publication-card/publication-card.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DataService } from './services/data.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { QuarantineDialogComponent } from './dialogs/quarantine-dialog/quarantine-dialog.component';
import { QuarantineActionPanelComponent } from './quarantine/quarantine-action-panel/quarantine-action-panel.component';
import { MatRadioModule } from '@angular/material/radio';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SyncDialogComponent } from './dialogs/sync-dialog/sync-dialog.component';
import { HardResetDialogComponent } from './dialogs/hard-reset-dialog/hard-reset-dialog.component';
import { ChartsModule } from 'ng2-charts';
import { AngularResizedEventModule } from 'angular-resize-event';
import { MyLibraryActionPanelComponent } from './my-library/my-library-action-panel/my-library-action-panel.component';
import { MatMenuModule } from '@angular/material/menu';
import { RemoveCitationDialogComponent } from './dialogs/remove-citation-dialog/remove-citation-dialog.component';
import { ExportDialogComponent } from './dialogs/export-dialog/export-dialog.component';
import { UtilsComponent } from './utils/utils.component';
import { NgxFileDropModule } from 'ngx-file-drop';
import { MatTabsModule } from '@angular/material/tabs';
import { EnumsComponent } from './enums/enums.component';
import { EnumsActionPanelComponent } from './enums/enums-action-panel/enums-action-panel.component';
import { MatIconModule } from '@angular/material/icon';
import { ExportFormatDialogComponent } from './dialogs/export-format-dialog/export-format-dialog.component';
import { MatChipsModule } from '@angular/material/chips';
import { EditCitationDialogComponent } from './dialogs/edit-citation-dialog/edit-citation-dialog.component';
import { RemovePublicationDialogComponent } from './dialogs/remove-publication-dialog/remove-publication-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    MyLibraryComponent,
    QuarantineComponent,
    SettingsComponent,
    SyncComponent,
    SideBarComponent,
    PublicationCardComponent,
    DashboardComponent,
    QuarantineDialogComponent,
    QuarantineActionPanelComponent,
    SyncDialogComponent,
    HardResetDialogComponent,
    MyLibraryActionPanelComponent,
    RemoveCitationDialogComponent,
    ExportDialogComponent,
    UtilsComponent,
    EnumsComponent,
    EnumsActionPanelComponent,
    ExportFormatDialogComponent,
    EditCitationDialogComponent,
    RemovePublicationDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatDialogModule,
    MatSelectModule,
    NgpImagePickerModule,
    MatTableModule,
    MatPaginatorModule,
    MatRadioModule,
    MatInputModule,
    MatCheckboxModule,
    ChartsModule,
    AngularResizedEventModule,
    MatMenuModule,
    NgxFileDropModule,
    MatTabsModule,
    MatIconModule,
    MatChipsModule
  ],
  providers: [
    DateFormatPipe,
    { provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher },
    DataService
  ],
  entryComponents: [
    QuarantineDialogComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
