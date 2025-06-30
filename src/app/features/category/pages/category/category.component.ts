import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../category.service';
import { CategoryModel } from '../../models/category.model';
import { ActivatedRoute } from '@angular/router';

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
    pageSize   : number =4;

    errorMessage : string = '';
    isOfferRoute : boolean = false;

    constructor(private categoryService : CategoryService, private route : ActivatedRoute) { }

    ngOnInit(): void {

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
          this.categories = response.Data;
          this.totalCount = response.TotalCount;
        },
      error : err => 
      {  
       this.errorMessage= err.error ;
      }
      });
    }

    fetchCategoriesWithOffers() : void 
    {
      this.categoryService.getCategoriesWithOffers(this.pageNumber, this.pageSize).subscribe({ 
        next : response => {
          this.categories = response.Data;
          this.totalCount = response.TotalCount;        },
        error : err =>
        {
          this.errorMessage = err.error;
        }

      });
    }

    totalPages(): number
    {
      return this.totalCount / this.pageSize;
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
}
