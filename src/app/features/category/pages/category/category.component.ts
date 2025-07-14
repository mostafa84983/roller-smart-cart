import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../category.service';
import { CategoryModel } from '../../models/category.model';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { Params } from '@angular/router';
import { CreateCategoryDialogComponent } from '../../dialogs/create-category-dialog/create-category-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { UpdateCategoryDialogComponent } from '../../dialogs/update-category-dialog/update-category-dialog.component';
import { environment } from '../../../../../environments/environment';


@Component({
  selector: 'app-category',
  standalone: true,
  imports: [],
  templateUrl: './category.component.html',
  styleUrl: './category.component.scss'
})
export class CategoryComponent implements OnInit{

    categories : CategoryModel[] = [];
    totalCount : number = 0;
    pageNumber : number =1;
    pageSize   : number =3;

    errorMessage : string = '';
    isOffer : boolean = false;
    isAdmin : boolean = false;

    backendBaseUrl = environment.backendBaseUrl;

    constructor(private categoryService : CategoryService, private activatedRoute : ActivatedRoute,private route :Router,
      private authService : AuthService, private dialog: MatDialog) { }

    ngOnInit(): void {

      const role = this.authService.getRole();
      this.isAdmin = (role === 'Admin');

      this.activatedRoute.queryParams.subscribe((queryParams: Params) => {
      this.isOffer = queryParams['isOffer'] === 'true';
      
      this.pageNumber = 1;
      
      if(this.isOffer)
      {
        this.fetchCategoriesWithOffers();
      }
      else
      {
        this.fetchCategories();
      }
    });
    }

    fetchCategories() : void {
      this.categoryService.getPaginatedCategories(this.pageNumber,this.pageSize).subscribe({
        next : (response) => {
          this.categories = response.data;
          this.totalCount = response.totalCount;
        },
      error : err => 
      {  
      // console.error('Error fetching categories:', err);
       this.errorMessage= err.error ;
      }
      });
    }

    fetchCategoriesWithOffers() : void 
    {
      this.categoryService.getCategoriesWithOffers(this.pageNumber, this.pageSize).subscribe({ 
        next : response => {
          this.categories = response.data;
          this.totalCount = response.totalCount;    
        },
        error : err =>
        {
          this.errorMessage = err.error;
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
        this.fetchCategoriesWithOffers();
      } 
      else 
      {
        this.fetchCategories();    
      }
    }

  getImageUrl(imageFileName: string): string {
  return `${this.backendBaseUrl}/${imageFileName}`;
}


goToProducts(categoryId : number) : void
{
  if(this.isOffer)
  {
    this.route.navigate(['/categories', categoryId, 'products'], {queryParams: { isOffer: this.isOffer } } );  
  }
  else
  {
    this.route.navigate(['/categories', categoryId, 'products'], {queryParams: { isOffer: this.isOffer } });
  }
}


  openCreateCategoryDialog() 
  {
  const dialogRef = this.dialog.open(CreateCategoryDialogComponent, {
    width: '350px',
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result === 'created') {
      this.fetchCategories();
    }
  });
}
openUpdateCategoryDialog(category : CategoryModel)
{
 const dialogRef = this.dialog.open(UpdateCategoryDialogComponent, {
      width: '350px',
      data: {
        categoryId: category.categoryId,
        categoryName: category.categoryName,
        categoryImage: category.categoryImage
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'updated') {
        this.fetchCategories();
      }
    });
}
}
