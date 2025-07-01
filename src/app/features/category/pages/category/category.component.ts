import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../category.service';
import { CategoryModel } from '../../models/category.model';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { NgFor, NgIf } from '@angular/common';
import { Params } from '@angular/router';


@Component({
  selector: 'app-category',
  standalone: true,
  imports: [NgIf,NgFor],
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

    backendBaseUrl = 'https://localhost:7075';

    constructor(private categoryService : CategoryService, private activatedRoute : ActivatedRoute,private route :Router,
      private authService : AuthService) { }

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
}
