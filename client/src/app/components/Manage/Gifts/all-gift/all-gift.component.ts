import { Component, inject } from '@angular/core';
import { AddGiftComponent } from '../add-gift/add-gift.component';
import { ButtonModule } from 'primeng/button';
import { ImportsModule } from '../../../../imports';
import { GiftService } from '../../../../services/gift.service';
import { Gift } from '../../../../models/gift';
import { Observable } from 'rxjs';
import { Category } from '../../../../models/Category';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router, RouterOutlet } from '@angular/router';
import { Donor } from '../../../../models/donor';

@Component({
  selector: 'app-all-gift',
  standalone: true,
  imports: [AddGiftComponent, ImportsModule, RouterOutlet],
  templateUrl: './all-gift.component.html',
  styleUrl: './all-gift.component.css'
})
export class AllGiftComponent {
  confirmationService = inject(ConfirmationService)
  addGiftVisiable: boolean = false
  giftService = inject(GiftService)
  router = inject(Router)
  // messageService = inject(MessageService)
  giftList?: Gift[];
  giftToSend = this.giftService.newGift()
  type = 'add'
  status : string = localStorage.getItem("status")?.toString() || ""
  searchParameters: any;
  visible: boolean = false;

  ngOnInit() {
    // status = localStorage.getItem("status") || ""
    this.getAllGifts()
    console.log(this.giftList); 
    this.searchParameters = {
      name:"",
      donor:"",
      minSales:0,
      price:10,
    } 
  }

  ngOnChange() {
    this.getAllGifts()
  }
  openAddGift() {
    this.addGiftVisiable = true
    this.router.navigate(['manage/gifts/add'], { state: { gift: this.giftToSend } });
    this.giftToSend = this.giftService.newGift()
    this.type = 'add'
    this.getAllGifts()
  }

  async getAllGifts() {
    try {
      const response = await this.giftService.getAll().toPromise();
      this.giftList = response;
      console.log(this.giftList);
    } catch (error) {
      console.error('Error fetching categories', error);
    }
  }

  search(){
    console.log(this.searchParameters);
    
    this.giftService.search(this.searchParameters.name, this.searchParameters.donor, this.searchParameters.price, this.searchParameters.minSales).subscribe(
      data => {this.giftList = data; console.log(data);
      },
      error => console.log(error)       
    );
  }

  async delete(gift: Gift) {
    try {
      if (gift.id) {
        const response = await this.giftService.delete(gift.id).toPromise();
      }
      console.log(this.giftService);
    } catch (error) {
      console.error('Error fetching categories', error);
    }
  }
  showDialog() {
    this.visible = true;
}

  confirm(event: Event, gift: Gift) {
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
        this.delete(gift)
      },
      reject: () => {
        this.confirmationService.close
      }
    });
  }


}
