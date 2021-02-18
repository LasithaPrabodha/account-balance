import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';
import { NotFoundComponent } from './pages/error-pages/not-found/not-found.component';
import { ForbiddenComponent } from './pages/forbidden/forbidden.component';
import { HomeComponent } from './pages/home/home.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },

  {
    path: 'auth',
    loadChildren: () =>
      import('./pages/authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
  },
  {
    path: 'upload-balances',
    loadChildren: () =>
      import('./pages/upload-balances/upload-balances.module').then(
        (m) => m.UploadBalancesModule
      ),
    canActivate: [AdminGuard],
  },
  {
    path: 'account-balances',
    loadChildren: () =>
      import('./pages/account-balances/account-balances.module').then(
        (m) => m.AccountBalancesModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'account-reports',
    loadChildren: () =>
      import('./pages/account-reports/account-reports.module').then(
        (m) => m.AccountReportsModule
      ),
    canActivate: [AdminGuard],
  },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: '404', component: NotFoundComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [],
})
export class AppRoutingModule {}
