import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule, MatError } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CategoryService } from '../../category.service';
import { UpdateCategoryModel } from '../../models/update-category.model';

@Component({
  selector: 'app-update-category-dialog',
  standalone: true,
  imports: [
      CommonModule,
      ReactiveFormsModule,
      MatDialogModule,
      MatFormFieldModule,
      MatInputModule,
      MatIconModule,
      MatButtonModule
  ],
  templateUrl: './update-category-dialog.component.html',
  styleUrl: './update-category-dialog.component.scss'
})
export class UpdateCategoryDialogComponent implements OnInit{
  categoryForm!: FormGroup;
  errorMessage: string = '';
  selectedFile: File | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private categoryService: CategoryService,
    public dialogRef: MatDialogRef<UpdateCategoryDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UpdateCategoryModel) {}

  ngOnInit(): void {
    this.categoryForm = this.formBuilder.group({
      categoryName: [this.data.categoryName || '']
    });
  }
 onSubmit() {

    const newName = this.categoryForm.get('categoryName')!.value.trim();
    const oldName = (this.data.categoryName || '').trim();

    const isNameChanged = newName !== oldName;
    const isImageChanged = this.selectedFile !== null;

    if (!isNameChanged && !isImageChanged) 
    {
      this.errorMessage = 'Please change category name or select a new image before updating';
      return;
    }

    const formData = new FormData();
    formData.append('CategoryId', this.data.categoryId.toString());

    if (isNameChanged && newName.length > 0) 
    {
      formData.append('CategoryName', newName);
    }

    if (isImageChanged && this.selectedFile) 
    {
      formData.append('CategoryImage', this.selectedFile);
    }

    this.categoryService.updateCategory(formData).subscribe({
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
