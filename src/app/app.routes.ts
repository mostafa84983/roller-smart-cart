import { Routes } from '@angular/router';
import { LoginComponent } from './core/auth/pages/login/login.component';
import { HomeComponent } from './features/home/home.component';
import { authGuard } from './core/auth/auth.guard';

export const routes: Routes = 
[
{
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
},
{
    path : 'login',
    component: LoginComponent
},
{
    path :'home',
    component : HomeComponent,
    canActivate :[authGuard]
}




];
