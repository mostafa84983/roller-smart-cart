import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './core/layout/header/header.component';
import { CartSignalrService } from './features/cart-product/cart-signalr.service';
import { CartService } from './shared/cart.service';
import { ProductAddOrRemoveDto } from './features/cart-product/ProductAddOrRemoveDto';

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
  backendBaseUrl = 'https://localhost:7075/';
  currentProduct:any = null;
  productStatus :string = "" ;

  constructor(private cartSignalRService : CartSignalrService , private cartservice : CartService){}

  

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

}

closePopup() {
  this.currentProduct = null;
}

getImageUrl(imageFileName: string): string 
{
  return `${this.backendBaseUrl}/${imageFileName}` ;
}


}
