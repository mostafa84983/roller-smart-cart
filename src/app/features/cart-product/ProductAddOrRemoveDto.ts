export interface ProductAddOrRemoveDto {
  productId: number;
  productName: string;
  productCode: number;
  productWeight: number;
  quantity: number;
  productPrice: number;
  productImage: string;
  productDescription: string;
  isAvaiable: boolean;
  isOffer: boolean;
  offerPercentage: number;
  categoryId: number;
  orderId: string;
}
