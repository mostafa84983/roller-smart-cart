import { Component ,OnInit} from '@angular/core';
import { ProductService } from '../../product.service';
import { productModel } from '../../models/product.model';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { CommonModule } from '@angular/common';
import { CreateProductDialogComponent } from '../../dialogs/create-product-dialog/create-product-dialog.component';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule,CreateProductDialogComponent],
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss'
})
export class ProductComponent implements OnInit{

  categoryId! : number;
  pageNumber : number =1;
  pageSize   : number =4;

  products : productModel[] = [];
  totalCount : number = 0;

  errorMessage : string = '';
  isOffer : boolean = false;
  isAdmin : boolean = false;
  


  backendBaseUrl = 'https://localhost:7075';


  constructor(private productService : ProductService, private route : ActivatedRoute,
    private authService : AuthService, private dialog: MatDialog) {}

  ngOnInit(): void {

    const role = this.authService.getRole();
    this.isAdmin = (role === 'Admin');

    this.route.paramMap.subscribe(params => {

    this.categoryId = +(params.get('categoryId') || 0);

    this.route.queryParamMap.subscribe(queryParams => {
      
    this.isOffer = queryParams.get('isOffer') === 'true';

    this.pageNumber = 1;

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

  getProductUnit(description: string): string 
  {
  if (description.includes("ml")) return "ml";
  if (description.includes("g")) return "g";

     return "g";
  }

  openCreateProductDialog() 
  {
  const dialogRef = this.dialog.open(CreateProductDialogComponent, {
    width: '350px',
    maxWidth: '500px',
    height: '90vh',   
    autoFocus: false,
    data: { categoryId: this.categoryId },

  });

  dialogRef.afterClosed().subscribe(result => {
    if (result === 'created') {
      this.fetchProductsOfCategory();
    }
  });
}

}
