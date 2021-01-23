import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MyLibraryComponent } from './my-library/my-library.component';
import { QuarantineComponent } from './quarantine/quarantine.component';
import { SettingsComponent } from './settings/settings.component';
import { SyncComponent } from './sync/sync.component';

const routes: Routes = [
  { path: '', component: MyLibraryComponent },
  { path: 'library', component: MyLibraryComponent },
  { path: 'quarantine', component: QuarantineComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'sync', component: SyncComponent },

  { path: '', redirectTo: '/library', pathMatch: 'full' },
  { path: 'quarantine', redirectTo: '/quarantinr', pathMatch: 'full' },
  { path: 'settings', redirectTo: '/settings', pathMatch: 'full' },
  { path: 'sync', redirectTo: '/sync', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
