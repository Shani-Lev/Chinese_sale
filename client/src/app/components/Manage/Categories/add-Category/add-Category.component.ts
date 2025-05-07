import { Component, EventEmitter, inject, Output } from '@angular/core';
import { GiftService } from '../../../../services/gift.service';
import { Category } from '../../../../models/Category';
import { FormsModule } from '@angular/forms';
import { ImportsModule } from '../../../../imports';
import { CategoryService } from '../../../../services/category.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { MessageService } from 'primeng/api';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-add-Category',
  imports: [FormsModule, ImportsModule],
  templateUrl: './add-Category.component.html',
  styleUrl: './add-Category.component.css'
})
export class AddCategoryComponent {
  public categoryService = inject(CategoryService)
  router = inject(Router)
  location = inject(Location)

  category : Category = new Category("") 

  @Output() categoryClosed = new EventEmitter<Category>();
  save() {
    this.categoryService.add(this.category).subscribe(response => {
      console.log(response);
      this.categoryClosed.emit(this.category); 
      this.location.back();
    });
  }

}
