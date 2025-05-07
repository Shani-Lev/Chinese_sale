import { Component, inject } from '@angular/core';
import { ImageModule } from 'primeng/image';
import { CategoryService } from '../../../../services/category.service';
import { Category } from '../../../../models/Category';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { ImportsModule } from '../../../../imports';
import { ConfirmationService, MessageService } from 'primeng/api';
import { MessageModule } from 'primeng/message';
import { RippleModule } from 'primeng/ripple';
import { MessagesModule } from 'primeng/messages';

@Component({
  selector: 'app-all-categories',
  imports: [ImportsModule, RouterOutlet, MessagesModule, RippleModule],
  templateUrl: './all-categories.component.html',
  styleUrl: './all-categories.component.css',
})
export class AllCategoriesComponent {
  categoryService = inject(CategoryService)
  router = inject(Router)
  activatedRouter = inject(ActivatedRoute)
  my_route: boolean = false;
  confirmationService = inject(ConfirmationService)
  categories : Category[]  = []
  openAdd: boolean = false

  ngOnInit(){
    this.getCategories()
    this.activatedRouter.url.subscribe(() => {
      let rout = this.activatedRouter.snapshot.url.join('');
      this.my_route = rout.includes("addCategory");
      if (!this.my_route) {
        this.openAdd = false;
      }
    });
     }

  ngOnChange() {
    let rout = this.activatedRouter.snapshot.url.join('');
    this.my_route = rout.includes("addCategory")
    this.getCategories()
  }

  getCategories() {
    // this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Message Content' });
    this.categoryService.getAll().subscribe((data) => {
      this.categories = data;
      console.log(this.categories);
    });  
    
  }
  addCategort() { 
    this.router.navigate(['manage/categories/addCategory']).then(() => {
      this.openAdd = true;
      this.getCategories()
    });
    this.getCategories()
  }

  delete(category : Category){
    this.categoryService.delete(category).subscribe((data) => {
      this.getCategories()
    });
  }

  hide() {
    this.getCategories()
    this.router.navigate(['manage/categories'])
    this.openAdd = false
  }
  
  confirm2(event: Event, category: Category) {
    console.log(this.categories);
    // this.categories.push(category)
    // this.categories.sort()
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: '?אתה בטוח שברצונך למחוק',
      icon: 'pi pi-info-circle',
      rejectButtonProps: {
        label: 'ביטול',
        severity: 'secondary',
        outlined: true
      },
      acceptButtonProps: {
        label: 'מחק',
        severity: 'danger'
      },
      accept: () => {
        this.delete(category)
      },
      reject: () => {
        this.confirmationService.close
        
      }
    });
  }
}
