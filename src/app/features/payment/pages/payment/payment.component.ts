import { Component, OnInit } from '@angular/core';
import { PaymentService } from '../../payment.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.scss'
})
export class PaymentComponent implements OnInit{

    errorMessage : string = ''

  constructor(private paymentService : PaymentService,private router : Router) {}

  ngOnInit(): void {

    const orderIdStr = localStorage.getItem('orderId');
    const orderId = orderIdStr ? parseInt(orderIdStr, 10) : -1;
      // console.log('PaymentComponent ngOnInit triggered');
    this.paymentService.createCheckoutSession(orderId).subscribe({
      next: response => 
      {
        // console.log('it works');
          window.location.href = response.value; 
      },
      error: err => {
      this.errorMessage = err.error ;
      }
    });


  }


  
}