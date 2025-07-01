import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../category.service';
import { CategoryModel } from '../../models/category.model';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { NgFor, NgIf } from '@angular/common';

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
    isOfferRoute : boolean = false;
    isAdmin : boolean = false;

    backendBaseUrl = 'https://localhost:7075';

    constructor(private categoryService : CategoryService, private route : ActivatedRoute,
      private authService : AuthService) { }

    ngOnInit(): void {

      const role = this.authService.getRole();
      this.isAdmin = (role === 'Admin');

      this.isOfferRoute = this.route.snapshot.routeConfig?.path === 'offers';
      if(this.isOfferRoute)
      {
        this.fetchCategoriesWithOffers();
      }
      else
      {
        this.fetchCategories();
      }

    }

    fetchCategories() : void {
      this.categoryService.getPaginatedCategories(this.pageNumber,this.pageSize).subscribe({
        next : (response) => {
          this.categories = response.data;
          this.totalCount = response.totalCount;
        },
      error : err => 
      {  
              console.error('Error fetching categories:', err);

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
      
    if (this.isOfferRoute) 
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
}
