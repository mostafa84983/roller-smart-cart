import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './core/layout/header/header.component';
import { CartSignalrService } from './features/cart-product/cart-signalr.service';
import { CartService } from './shared/cart.service';
import { ProductAddOrRemoveDto } from './features/cart-product/ProductAddOrRemoveDto';
import { environment } from '../environments/environment';
import { CartproductService } from './features/cart-product/cartproduct.service';
import { productModel } from './features/product/models/product.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  title = 'smart cart';
  cartId ="1234";
  backendBaseUrl = environment.backendBaseUrl;
  currentProduct:any = null;
  ProductDetected: productModel | null = null
  failedProduct: boolean = false;
  productStatus :string = "" ;

  constructor(private cartSignalRService : CartSignalrService , private cartservice : CartService , private cartProductService : CartproductService){}

  

  ngOnInit(): void {
  this.cartSignalRService.startConnection(this.cartId);

  this.cartSignalRService.onProductAdded(data => {
    this.currentProduct = data ;
    this.cartservice.setOrderId(data.product.orderId);
    this.cartservice.setTotalPrice(data.total) ;
    this.productStatus = "Product Added";
});

this.cartSignalRService.onProductRemoved(data => {
   this.currentProduct = data ;
   this.cartservice.setOrderId(data.product.orderId);
   this.cartservice.setTotalPrice(data.total) ;
   this.productStatus = "Product Removed";
});

this.cartSignalRService.onOrderCompleted(() => {
  this.cartservice.setOrderId(-1);
});

this.cartSignalRService.onFailedProduct(() => {
      this.failedProduct = true;
      this.currentProduct = null;
    });

this.cartSignalRService.onProductDetected(data => {
this.ProductDetected = data ;
});
    
}

closePopup() {
  this.currentProduct = null;
  this.ProductDetected = null;
}

getImageUrl(imageFileName: string): string 
{
  return `${this.backendBaseUrl}/${imageFileName}` ;
}

runOCR() {
   this.failedProduct = false;
    console.log("Running OCR fallback...");
    this.closePopup();
    this.cartProductService.OpenOCR(this.cartId).subscribe({
      next : data => {

      },
      error : err => 
      {}
    });
  }

  closePopupOCR()
  {
     this.failedProduct = false;
     this.cartProductService.REDO(this.cartId).subscribe({
      next : data => {

      },
      error : err => 
      {}
    });
  }


}
