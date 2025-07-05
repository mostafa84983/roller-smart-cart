import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CartService } from '../../../shared/cart.service';

@Component({
  selector: 'app-payment-success',
  standalone: true,
  imports: [],
  templateUrl: './payment-success.component.html',
  styleUrl: './payment-success.component.scss'
})
export class PaymentSuccessComponent implements OnInit{

  sessionId? : string;
  
  constructor(private activatedRoute : ActivatedRoute, private cartSerivce : CartService, private route : Router) {}

  ngOnInit(): void {
    
    this.sessionId = this.activatedRoute.snapshot.queryParamMap.get('sessionId') ?? undefined;
    console.log('session id : ' ,this.sessionId);
    
    if(this.sessionId)
    {
      this.cartSerivce.setOrderId(-1);
    }
  }

  goToHome()
  {
 this.route.navigate(['/home']);
  }
}
