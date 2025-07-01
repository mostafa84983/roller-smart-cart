import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

  constructor(private router : Router) {}

  goToCategories(){
    // this.router.navigate(['/categories']);
    this.router.navigate(['/categories'], { queryParams: { isOffer : false } });
  }

  goToOffers(){
    this.router.navigate(['/offers'], { queryParams: { isOffer : true } });
  }
}
