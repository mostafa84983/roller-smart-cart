import { Component, OnInit } from '@angular/core';
import {  Router } from '@angular/router';

@Component({
  selector: 'app-payment-cancel',
  standalone: true,
  imports: [],
  templateUrl: './payment-cancel.component.html',
  styleUrl: './payment-cancel.component.scss'
})
export class PaymentCancelComponent implements OnInit{

  constructor(private route : Router) {}
  ngOnInit(): void {
    
  }

  goToHome()
  {
     this.route.navigate(['/home']);
  }
}
