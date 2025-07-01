import { Routes } from '@angular/router';
import { LoginComponent } from './core/auth/pages/login/login.component';
import { HomeComponent } from './features/home/home.component';
import { authGuard } from './core/auth/auth.guard';
import { CategoryComponent } from './features/category/pages/category/category.component';
import { CartProductComponent } from './features/cart-product/cart-product.component';

export const routes: Routes = 
[
{
    path : 'login',
    component: LoginComponent
},
{
    path :'home',
    component : HomeComponent,
    canActivate :[authGuard]
},
{
    path: 'categories',
    component : CategoryComponent
},
{
    path: 'offers',
    component : CategoryComponent
},
{
    path: 'cartproduct',
    component: CartProductComponent
},
{ 
        path: '',
        redirectTo: '/home', 
        pathMatch: 'full'
}




];
