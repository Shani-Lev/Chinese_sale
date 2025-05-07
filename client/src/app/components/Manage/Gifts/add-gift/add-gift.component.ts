import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { GiftService } from '../../../../services/gift.service';
import { Gift } from '../../../../models/gift';
import { FormsModule } from '@angular/forms';
import { ImportsModule } from '../../../../imports';
import { ChangeDetectorRef } from '@angular/core';
import { AddDonorComponent } from '../../Donors/add-donor/add-donor.component';
import { AddCategoryComponent } from '../../Categories/add-Category/add-Category.component';
import { PriceSizeDirective } from '../../../../directives/price-size.directive';
import { DonorService } from '../../../../services/donor.service';
import { CategoryService } from '../../../../services/category.service';
import { Donor } from '../../../../models/donor';
import { Category } from '../../../../models/Category';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-add-gift',
  standalone: true,
  imports: [FormsModule, ImportsModule, AddDonorComponent, AddCategoryComponent, PriceSizeDirective, RouterOutlet],
  templateUrl: './add-gift.component.html',
  styleUrl: './add-gift.component.css'
})
export class AddGiftComponent {
  public giftService = inject(GiftService)
  public donorService = inject(DonorService)
  public categoryService = inject(CategoryService)
  router = inject(Router)

  @Input() gift: Gift = this.giftService.newGift();
  @Input() mtype: string = 'add';
  sizes: any[] = [{ label: 'קטן', value: 1 }, { label: 'בינוני', value: 2 }, { label: 'גדול', value: 3 }];

  @Input() addGiftVisiable: boolean = true
  @Output() openAddGift = new EventEmitter<void>();

  donorVisiable: boolean = false
  categoryVisiable: boolean = false

  donorList?: Donor[];
  categoryList?: Category[];

  // constructor(private cdr: ChangeDetectorRef) { }
  constructor() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        // כאן תוכל לבדוק אם הניווט היה לנתיב של הקומפוננטות הבן
        if (event.url.includes('addDonor') || event.url.includes('addCategory')) {
          // קוד שיבוצע כאשר הקומפוננטות הבן נפתחות
        }
      }
    });
  }

  ngOnInit() {
    this.gift = history.state.gift;
    this.getCategories();
    this.getDonors();
  }
  ngOnChange() {
    this.getCategories();
    this.getDonors();
  }

  save() {
    console.log(this.gift);
    if (this.gift.id == -1) {
      this.giftService.add(this.gift).subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.error('Error fetching data', error);
        })
      this.gift = this.giftService.newGift()
    }
    else {
      this.giftService.update(this.gift).subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.error('Error fetching data', error);
        })
      this.gift = this.giftService.newGift()
    }
    this.router.navigate(['manage/gifts']);
  }
  close() {
    this.router.navigate(['manage/gifts']);
    this.openAddGift.emit();
  }
  openAddDonor() {
    console.log("open add donor");
    this.router.navigate(['manage/gifts/add'])
    this.router.navigate(['manage/gifts/add', { outlets: { addDonor: ['addDonor'] } }]);
    this.donorVisiable = !this.donorVisiable
    this.getDonors();
  }
  openAddCategory() {
    this.categoryVisiable = !this.categoryVisiable
    this.donorVisiable = false
    this.router.navigate(['manage/gifts/add'])
    this.router.navigate(['manage/gifts/add', { outlets: { addCategory: ['addCategory'] } }]);
    // this.router.navigate([{ outlets: { addCategory: ['addCategory'] } }]);
    setTimeout(() =>
      this.getCategories(), 1000)

    console.log("open add category");
  }

  async getDonors() {
    try {
      const response = await this.donorService.getAll().toPromise();
      this.donorList = response;
      console.log(this.donorList);
    } catch (error) {
      console.error('Error fetching categories', error);
    }
  }
  async getCategories() {
    try {
      const response = await this.categoryService.getAll().toPromise();
      this.categoryList = response;
      console.log(this.categoryList);
    } catch (error) {
      console.error('Error fetching categories', error);
    }
  }
  handleDonorClosed(donor: Donor) {
    console.log('הדונור נסגר עם הנתונים:', donor);
    this.getDonors(); 
    this.donorVisiable = false
  }

  handleCategoryClosed(category: Category) {
    console.log('הקטגוריה נסגרה עם הנתונים:', category);
    this.getCategories(); 
    this.categoryVisiable = false
  }

}
