import { Routes } from '@angular/router';
import { LoginComponent } from './core/auth/pages/login/login.component';
import { HomeComponent } from './features/home/home.component';
import { authGuard } from './core/auth/auth.guard';
import { CategoryComponent } from './features/category/pages/category/category.component';
import { CartProductComponent } from './features/cart-product/cart-product.component';
import { ProductComponent } from './features/product/pages/product/product.component';
import { PaymentComponent } from './features/payment/pages/payment/payment.component';
import { PaymentSuccessComponent } from './features/payment/payment-success/payment-success.component';
import { PaymentCancelComponent } from './features/payment/payment-cancel/payment-cancel.component';

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
    path: 'cartproduct',
    component: CartProductComponent
},
{
  path: 'categories/:categoryId/products',
  component: ProductComponent
},
{
    path: 'payment',
    component : PaymentComponent
},
  { path: 'payment-success',
    component: PaymentSuccessComponent 
},
{ path: 'payment-cancel',
     component: PaymentCancelComponent 
},
{ 
        path: '',
        redirectTo: '/home', 
        pathMatch: 'full'
}




];
