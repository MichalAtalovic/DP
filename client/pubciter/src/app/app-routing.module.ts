import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EnumsComponent } from './enums/enums.component';
import { MyLibraryComponent } from './my-library/my-library.component';
import { QuarantineComponent } from './quarantine/quarantine.component';
import { SettingsComponent } from './settings/settings.component';
import { SyncComponent } from './sync/sync.component';
import { UtilsComponent } from './utils/utils.component';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'library', component: MyLibraryComponent },
  { path: 'quarantine', component: QuarantineComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'sync', component: SyncComponent },
  { path: 'enums', component: EnumsComponent },
  { path: 'utils', component: UtilsComponent },

  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'quarantine', redirectTo: '/quarantine', pathMatch: 'full' },
  { path: 'settings', redirectTo: '/settings', pathMatch: 'full' },
  { path: 'sync', redirectTo: '/sync', pathMatch: 'full' },
  { path: 'enums', redirectTo: '/enums', pathMatch: 'full' },
  { path: 'utils', redirectTo: '/utils', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
