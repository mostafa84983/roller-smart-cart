import { Component, Input } from '@angular/core';
import { productCartDto } from './productCartDto.model';

@Component({
  selector: 'app-cart-product',
  standalone: true,
  imports: [],
  templateUrl: './cart-product.component.html',
  styleUrl: './cart-product.component.scss'
})
export class CartProductComponent {
  @Input({required : true}) products : productCartDto[]={} as productCartDto[] ; 
  
  backendBaseUrl = 'https://localhost:7075/';

getImageUrl(imageFileName: string): string 
{
  return `${this.backendBaseUrl}/${imageFileName}` ;
}

}
