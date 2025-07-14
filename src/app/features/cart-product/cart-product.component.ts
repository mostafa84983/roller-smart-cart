import { Component, Input } from '@angular/core';
import { productCartDto } from './productCartDto.model';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-cart-product',
  standalone: true,
  imports: [],
  templateUrl: './cart-product.component.html',
  styleUrl: './cart-product.component.scss'
})
export class CartProductComponent {
  @Input({required : true}) products : productCartDto[]={} as productCartDto[] ; 
  
  backendBaseUrl = environment.backendBaseUrl;

getImageUrl(imageFileName: string): string 
{
  return `${this.backendBaseUrl}/${imageFileName}` ;
}

}
