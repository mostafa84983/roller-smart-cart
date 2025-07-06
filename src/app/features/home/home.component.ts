import { Component, OnInit } from '@angular/core';
import { productCartDto } from '../cart-product/productCartDto.model';
import { CartproductService } from '../cart-product/cartproduct.service';
import { CartProductComponent } from '../cart-product/cart-product.component';
import { CartService } from '../../shared/cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CartProductComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
   products: productCartDto[] = [];
   totalPrice : number = 0 ;
   page : number = 1 ;
   pagesize : number = 3 ;
   orderId : number = -1;

   errorMessage : string = ''

   constructor(private cartProductService : CartproductService , private cartservice : CartService, private route : Router){}
  
   ngOnInit(): void {
    this.cartservice.orderid$.subscribe(id => {
    this.orderId = id; 
    if (id > 0) {
    this.startPage();
    }

    this.cartservice.totalPrice$.subscribe(price =>{
      this.totalPrice = price ;
    })
    
  });
}

  startPage(){
  this.cartProductService.getProductsOfCart(this.page, this.pagesize,this.orderId).subscribe( {
      next : data => {
        this.products= data.products;
        this.totalPrice = data.total ;
      },
      error : err =>{
      this.errorMessage = err.error ;
      }
    }) ;
  }

  onPageChange(newPage: number){
    this.page= newPage ;
    this.startPage() ;
  }

   
onCheckout()
{
 this.route.navigate(['/payment']);
}


}
