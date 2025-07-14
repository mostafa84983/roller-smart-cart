export interface OrderDto {
  orderId: number;
  orderPrice: number;
  orderNumber: string;
  creationDate: Date; 
  orderDiscount: number;
  userId: number;
}