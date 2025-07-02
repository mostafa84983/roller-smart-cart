import { Component, OnInit , Inject} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../product.service';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatError } from '@angular/material/form-field';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-product-dialog',
  standalone: true,
  imports: [CommonModule,
            ReactiveFormsModule,
            MatDialogModule,
            MatFormFieldModule,
            MatInputModule,
            MatIconModule,
            MatButtonModule,
            MatError],
  templateUrl: './create-product-dialog.component.html',
  styleUrl: './create-product-dialog.component.scss'
})
export class CreateProductDialogComponent implements OnInit{


  productForm! : FormGroup;
  errorMessage : string = '';

  selectedFile : File | null = null;

  constructor(private formBuilder : FormBuilder, private productService : ProductService,
             public dialogRef: MatDialogRef<CreateProductDialogComponent>,
             @Inject(MAT_DIALOG_DATA) public data: { categoryId: number } ) {}

  ngOnInit(): void {
    this.productForm = this.formBuilder.group({
      productName : ['',Validators.required],
      productCode : [null,Validators.required],
      productWeight : [null,[Validators.required,Validators.min(0.01)]],
      productPrice : [null,[Validators.required,Validators.min(0.01)]],
      quantity : [null,[Validators.required,Validators.min(1)]],
      // productImage : ['',Validators.required],
      productDescription : [''],
      categoryId : [this.data.categoryId,Validators.required],
    });
  }

  onSubmit()
  {
    if(this.productForm.invalid || !this.selectedFile) 
    {
      this.errorMessage = 'Please fill all required fields and select an image';
      return;
    }

    const formData = new FormData();
    formData.append('ProductName', this.productForm.get('productName')!.value);
    formData.append('ProductCode', this.productForm.get('productCode')!.value);
    formData.append('ProductWeight', this.productForm.get('productWeight')!.value);
    formData.append('ProductPrice', this.productForm.get('productPrice')!.value);
    formData.append('Quantity', this.productForm.get('quantity')!.value);
    formData.append('ProductDescription', this.productForm.get('productDescription')!.value || '');
    formData.append('CategoryId', this.data.categoryId.toString());
    formData.append('ProductImage', this.selectedFile);

    this.productService.createProduct(formData).subscribe({
      next : response => 
      {
          this.productForm.reset();
          this.selectedFile = null;
          this.dialogRef.close('created');
      },
      error : err => 
      {  
       this.errorMessage= err.error;
      }
    });
    
  }

onFileSelected(event: Event): void 
{
  const input = event.target as HTMLInputElement;

  if (input.files && input.files.length > 0) 
  {
    this.selectedFile = input.files[0];
  }
}



}
