import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule, MatError } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CategoryService } from '../../category.service';

@Component({
  selector: 'app-create-category-dialog',
  standalone: true,
  imports: [CommonModule,
            ReactiveFormsModule,
            MatDialogModule,
            MatFormFieldModule,
            MatInputModule,
            MatIconModule,
            MatButtonModule,
            MatError],
  templateUrl: './create-category-dialog.component.html',
  styleUrl: './create-category-dialog.component.scss'
})
export class CreateCategoryDialogComponent implements OnInit {

  categoryForm! : FormGroup;
  errorMessage : string = '';

  selectedFile : File | null = null;

  constructor(private formBuilder : FormBuilder, private categoryService : CategoryService,
             public dialogRef: MatDialogRef<CreateCategoryDialogComponent>  ) {}

  ngOnInit(): void {
    this.categoryForm = this.formBuilder.group({
      categoryName : ['',Validators.required],
    });
  }


  onSubmit()
  {
    if(this.categoryForm.invalid || !this.selectedFile) 
    {
      this.errorMessage = 'Please fill all required fields and select an image';
      return;
    }

    const formData = new FormData();
    formData.append('CategoryName', this.categoryForm.get('categoryName')!.value);
    formData.append('CategoryImage', this.selectedFile);

    this.categoryService.createCategory(formData).subscribe({
      next : response => 
      {
          this.categoryForm.reset();
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
