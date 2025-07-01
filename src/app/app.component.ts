import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './core/layout/header/header.component';
import { CartSignalrService } from './features/cart-product/cart-signalr.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { productModel } from './features/product/models/product.model';
import { CartService } from './shared/cart.service';

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
  currentProduct: any = null;

  constructor(private cartSignalRService : CartSignalrService , private snakBar : MatSnackBar , private cartservice : CartService){}

  

  ngOnInit(): void {
  this.cartSignalRService.startConnection(this.cartId);

  this.cartSignalRService.onProductAdded(data => {
    this.currentProduct = data ;
    this.cartservice.setOrderId(data.product.orderId);
  console.log("Received ProductAdded:", data);
  this.snakBar.open(`Added: ${data.product.productName}, Total: ${data.total}`, 'Close', { duration: 4000 });
});

this.cartSignalRService.onProductRemoved(data => {
    this.currentProduct = data ;
   this.cartservice.setOrderId(data.product.orderId);
  this.snakBar.open(`Removed: ${data.product.productName}, Total: ${data.total}`, 'Close', { duration: 4000 });
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
