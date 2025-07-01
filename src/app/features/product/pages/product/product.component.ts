import { Component ,OnInit} from '@angular/core';
import { ProductService } from '../../product.service';
import { productModel } from '../../models/product.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [],
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss'
})
export class ProductComponent implements OnInit{

  categoryId! : number;
  pageNumber : number =1;
  pageSize   : number =3;

  products : productModel[] = [];
  totalCount : number = 0;

  errorMessage : string = '';
  isOffer : boolean = false;


  backendBaseUrl = 'https://localhost:7075';


  constructor(private productService : ProductService, private route : ActivatedRoute) {}

  ngOnInit(): void {

    this.route.paramMap.subscribe(params => {

    this.categoryId = +(params.get('categoryId') || 0);

    this.route.queryParamMap.subscribe(queryParams => {
      
    this.isOffer = queryParams.get('isOffer') === 'true';

    if(this.isOffer)
    {
      this.fetchProductsWithOffersOfCategory();
    }
    else
    {
      this.fetchProductsOfCategory();
    }
      }); });
  }


  fetchProductsOfCategory() : void
  {
    this.productService.getPaginatedProductsInCategory(this.categoryId , this.pageNumber, this.pageSize).subscribe({
      next : (response) => {
          this.products = response.data;
          this.totalCount = response.totalCount;
        },
      error : err => 
      {  
       this.errorMessage= err.error ;
      }
    });
  }
  
  fetchProductsWithOffersOfCategory() : void
  {
    this.productService.getPaginatedProductsWithOfferInCategory(this.categoryId , this.pageNumber, this.pageSize).subscribe({
      next : (response) => {
          this.products = response.data;
          this.totalCount = response.totalCount;
        },
      error : err => 
      {  
       this.errorMessage= err.error ;
      }
    });
  }

  totalPages(): number
    {
      return Math.ceil(this.totalCount / this.pageSize);
    }

    getPageNumbers(): number[] 
    {
        return Array.from({ length: this.totalPages() }, (_, i) => i + 1);
    }

    changePage(page : number ) : void
    {
      if (page < 1 || page > this.totalPages()) return;

      this.pageNumber = page;
      
      if (this.isOffer) 
      {
        this.fetchProductsWithOffersOfCategory();
      } 
      else 
      {
        this.fetchProductsOfCategory();    
      }
    }

  getImageUrl(imageFileName: string): string 
  {
    return `${this.backendBaseUrl}/${imageFileName}`;
  }
}
