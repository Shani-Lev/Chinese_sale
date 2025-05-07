import { Component, inject, ViewChild } from '@angular/core';
import { ImportsModule } from '../../../imports';
import { ManagerTicket } from '../../../models/manager-ticket';
import { ManagerTicketService } from '../../../services/manager-ticket.service';
import { Popover } from 'primeng/popover';
import { PopoverModule } from 'primeng/popover';
import { User } from '../../../models/user';
import { ManageService } from '../../../services/manage.service';

@Component({
  selector: 'app-tickets',
  imports: [ImportsModule, PopoverModule],
  templateUrl: './tickets.component.html',
  styleUrl: './tickets.component.css'
})
export class TicketsComponent {
  ticketService = inject(ManagerTicketService)
  managerService = inject(ManageService)
  @ViewChild('op') op!: Popover;

  tickets: ManagerTicket[] = [];
  users: User[] = [];

  status : string = localStorage.getItem("status") || ""
  sortType : number = 0
  aortOptions: any[] = [{ label: 'נמוך לגבוה', value: 1, icon: 'pi pi-bookmark' }, { label: 'גבוה לנמוך', value: 2, icon: 'pi pi-sliders-v' }, { label: 'רגיל', value: 3, icon: 'pi pi-expand' }];

  getWithSort(){
    
    if (this.sortType == 1){
      this.ticketService.orderBySales().subscribe(
        data => this.tickets = data,
        error => console.log(error)       
      );
    }
    else {if(this.sortType == 2){
      this.ticketService.orderByPrice().subscribe(
        data => this.tickets = data,
        error => console.log(error)       
      );
    }
  else this.getAll()}

  }

  ngOnInit() {
    if (this.status != 'SET')
      this.getAll()
  }
  getAll() {
    if (this.status == 'SET'){
          this.ticketService.getAll().subscribe(
      data => this.tickets = data,
      erroe => console.log(console.log(erroe))
    )

    }
    if (this.status == 'OFF'){
      this.managerService.getRevenue().subscribe(
        data => {this.tickets = data; console.log(this.tickets);
        },
        erroe => console.log(console.log(erroe))
      )
    }
    console.log(this.tickets);

  }

  toggle(event: Event, id : number = -1) {
    this.ticketService.getUser(id).subscribe(
      date => this.users = date,
      error => console.log(error)     
    )
    this.op.toggle(event);
    console.log(this.users);
    
  }
}
