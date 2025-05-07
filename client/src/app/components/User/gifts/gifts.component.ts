import { Component, inject } from '@angular/core';
import { ImportsModule } from '../../../imports';
import { GiftService } from '../../../services/gift.service';
import { Gift } from '../../../models/gift';
import { Category } from '../../../models/Category';
import { UserTicket } from '../../../models/user-ticket';
import { UserTicketService } from '../../../services/user-ticket.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-gifts',
  imports: [ImportsModule, RouterLink],
  templateUrl: './gifts.component.html',
  styleUrl: './gifts.component.css'
})
export class GiftsComponent {
  giftService = inject(GiftService)
  ticketService = inject(UserTicketService)
  giftList? : Gift[] 
  status = localStorage.getItem("status") || ""
  visible: boolean = false;
  visibleBuy: boolean = false;
  amount : number = 1
  giftToAddCart? : number = 0
  options: any[] = [{ label: 'מזומן', value: 1 }, { label: 'אשראי', value: 2 }];
  type? : number;
  total : number = 0
  sortType : number = 2
  aortOptions: any[] = [{ label: 'נמוך לגבוה', value: 1, icon: 'pi pi-sort-numeric-down' }, { label: 'גבוה לנמוך', value: 2, icon: 'pi pi-sort-numeric-up-alt' }];
  user? :string
  
  ngOnInit(){
    this.user = localStorage.getItem("role") || "" 
    this.getAllGifts()
    this.type = 1
  }

  getWithSort(){
    console.log("sort"+this.type);
    
    if (this.sortType == 2)
      this.getAllGifts()
    else {
      this.giftService.sortPrice().subscribe(
        data => this.giftList = data,
        error => console.log(error)       
      );
    }
  }
  getAllGifts() {
      this.giftService.getAll().subscribe(
        data => this.giftList = data,
        error => console.log(error)       
      );
      console.log(this.giftList);
  }

  addToCart(giftId : number = 0, amount : number = 1){
    this.ticketService.add(giftId, amount, true).subscribe(
      ()=>this.getAllGifts(),
      error => console.log(error)    
    )
    this.visible = false
  }

  async Buy(){
    await this.ticketService.add(this.giftToAddCart || 0, this.amount, false).subscribe(
      ()=>{this.getAllGifts(); },
      error => console.log(error)    
    )   
  }

  async putBuy(){
    await this.ticketService.Buy(this.giftToAddCart || 0).subscribe(
      ()=>this.getAllGifts(),
      error => console.log(error)    
    )
    this.visibleBuy = false
    this.amount = 1
    this.type = 1
  }

  showDialog() {
    this.visible = true;
}
showDialogBuy() {
  this.visibleBuy = true;
}
}
