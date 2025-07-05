import { Component, OnInit } from '@angular/core';
import { OrderDto } from './order.model';
import { OrderService } from './order.service';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [],
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent implements OnInit {
 
  orders : OrderDto[] = [] ;
  errorMessage = ''
  constructor(private orderService : OrderService){}
 
  ngOnInit(): void {
    this.orderService.GetOrdersofUser().subscribe({
      next : data => {
      this.orders = data ;
      },
      error : err => {
      this.errorMessage = err.error;
      }
    });
  }

}
