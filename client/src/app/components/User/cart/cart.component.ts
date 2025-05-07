import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ImportsModule } from '../../../imports';
import { UserTicket } from '../../../models/user-ticket';
import { UserTicketService } from '../../../services/user-ticket.service';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-cart',
  imports: [ImportsModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  ticketService = inject(UserTicketService)
  confirmationService = inject(ConfirmationService)
  changeDetector = inject(ChangeDetectorRef);
  tickets: any = []
  status: string = ""
  isProcessing: { [key: number]: boolean } = {};
  visible: boolean = false;
  type? : number;
  anount : number = 1;
  giftToAddCart : number = 0
  options: any[] = [{ label: 'מזומן', value: 1 }, { label: 'אשראי', value: 2 }];
  totel : number = 0
  totelToPay : number = 0
  user? :string

  ngOnInit() {
    console.log("im here");
    this.user = localStorage.getItem("role") || "" 
    this.getAll()
    this.status = localStorage.getItem("status") || ""
    this.type = 1
  }

  getAll() {
    this.ticketService.getInBasket().subscribe(
      data => {
        this.tickets = data;
        console.log(this.tickets);
        this.getTotal()
      },
      error => console.log(error)
    );
  }

  buy() {
    if (this.giftToAddCart === -1){
      this.buyAll()
      return
    }
    this.ticketService.Buy(this.giftToAddCart).subscribe(
      data => this.getAll(),
      error => console.log(error)     
    )
    this.totel = 0
  }

  buyAll(){
    this.ticketService.BuyAll().subscribe(
      data => this.getAll(),
      error => console.log(error)     
    )
    this.totel = 0
    this.totelToPay = 0
  }

  add(giftId: number) {
    console.log("add");
    
    this.ticketService.add(giftId, 1 , true).subscribe(
      () => {
        this.getAll();
      },
      error => {
        console.error(error);
      }
    );
  }

  getTotal(){
    for (let i = 0; i < this.tickets.length; i++){
      this.totelToPay += (this.tickets[i].gift.price * this.tickets[i].amount)
    }
  }

  async handleRemove(event: Event, amount: number, id: number, index : number, type: string) {
    console.log("removeaaaaa");
    
    if (this.isProcessing[id]) {
      return;
    }

    this.isProcessing[id] = true;
    try {
      if (type === 'a'){
        const isConfirmed = await this.confirm(event);
        if (!isConfirmed)
          return
      }
      
        console.log("remove");
        
        await this.ticketService.remove(id, amount).toPromise();
        this.getAll();
        // const ticket = this.tickets[index]
        // if (ticket && ticket.amount) {
        //   ticket.amount = amount
        // }
    } catch (error) {
      console.error(error);
    } finally {
      this.isProcessing[id] = false;
    }
  }

  confirm(event: Event): Promise<boolean> {
    return new Promise((resolve) => {
      this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: '?למחוק את הכרטיס להגרלה זו',
        icon: 'pi pi-info-circle',
        rejectButtonProps: {
          label: 'ביטול',
          severity: 'secondary',
          outlined: true,
        },
        acceptButtonProps: {
          label: 'מחק',
          severity: 'danger',
        },
        accept: () => resolve(true),
        reject: () => resolve(false),
      });
    });
  }
}
