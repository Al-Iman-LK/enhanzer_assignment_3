import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { PurchaseBillComponent } from './purchase-bill/purchase-bill.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'purchase-bill', component: PurchaseBillComponent },
  { path: '**', redirectTo: '/login' }
];
