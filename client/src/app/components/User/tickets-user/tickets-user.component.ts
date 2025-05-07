import { Component, inject } from '@angular/core';
import { UserTicketService } from '../../../services/user-ticket.service';
import { ImportsModule } from '../../../imports';

@Component({
  selector: 'app-tickets-user',
  imports: [ImportsModule],
  templateUrl: './tickets-user.component.html',
  styleUrl: './tickets-user.component.css'
})
export class TicketsUserComponent {
  ticketService = inject(UserTicketService)
  
  tickets : any =[]
  status : string =""
  isWin? : boolean

  user? :string
  
  ngOnInit(){
    this.getAll()
    this.user = localStorage.getItem("role") || "" 
    this.status = localStorage.getItem("status") || ""
  }

  getAll(){
    this.ticketService.getNotInBasket().subscribe(
      data => {
        this.tickets = data; console.log(this.tickets);
      },
      error => console.log(error)      
    )
    
  }
  
}
