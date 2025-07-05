import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule, MatError } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ProductService } from '../../product.service';
import { UpdateProductModel } from '../../models/update-product.model';

@Component({
  selector: 'app-update-product-dialog',
  standalone: true,
  imports: [
      CommonModule,
      ReactiveFormsModule,
      MatDialogModule,
      MatFormFieldModule,
      MatInputModule,
      MatIconModule,
      MatButtonModule,
      MatError
  ],  templateUrl: './update-product-dialog.component.html',
  styleUrl: './update-product-dialog.component.scss'
})
export class UpdateProductDialogComponent implements OnInit{
  productForm!: FormGroup;
  errorMessage: string = '';
  selectedFile: File | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private productService: ProductService,
    public dialogRef: MatDialogRef<UpdateProductDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UpdateProductModel) {}

    ngOnInit(): void {
      this.productForm = this.formBuilder.group({
      productName: [this.data.productName || ''],
      productCode: [this.data.productCode || '', [Validators.min(1)]],
      productWeight: [this.data.productWeight || '', [Validators.min(0.1)]],
      quantity: [this.data.quantity || '', [Validators.min(1)]],
      productPrice: [this.data.productPrice || '', [Validators.min(0.1)]],
      productDescription: [this.data.productDescription || ''],
      isAvaiable: [this.data.isAvaiable || true]
    });
    }
 onSubmit() {
  if (this.productForm.invalid) {
    this.errorMessage = 'Please correct the validation errors before submitting';
    return;
  }

  const formValue = this.productForm.value;
  const formData = new FormData();

  formData.append('ProductId', this.data.productId.toString()); 

    if (formValue.productName.trim()) formData.append('ProductName', formValue.productName.trim());
    if (formValue.productCode) formData.append('ProductCode', formValue.productCode.toString());
    if (formValue.productWeight) formData.append('ProductWeight', formValue.productWeight.toString());
    if (formValue.quantity) formData.append('Quantity', formValue.quantity.toString());
    if (formValue.productPrice) formData.append('ProductPrice', formValue.productPrice.toString());
    if (formValue.productDescription.trim()) formData.append('ProductDescription', formValue.productDescription.trim());

    formData.append('IsAvaiable', formValue.isAvaiable.toString());
    
    if (this.selectedFile) 
    {
    formData.append('ProductImage', this.selectedFile);
    }

  this.productService.updateProduct(formData).subscribe({
    next: () => {
      this.dialogRef.close('updated');
    },
    error: err => {
      this.errorMessage = err.error;
    }
  });
}




  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }


}


