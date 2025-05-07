import { Component, inject, TemplateRef, ViewChild } from '@angular/core';
import { StatusService } from '../../../services/status.service';
import { Status } from '../../../models/status';
import { ImportsModule } from '../../../imports';
import { ConfirmationService } from 'primeng/api';
import moment from 'moment';
import { GiftService } from '../../../services/gift.service';
import { CategoryService } from '../../../services/category.service';
import { DonorService } from '../../../services/donor.service';

class EventItem {
  status?: string;
  text?: string;
  icon?: string;
  color?: string;
  image?: string;
  isActive?: boolean = false
}

@Component({
  selector: 'app-status',
  imports: [ImportsModule],
  templateUrl: './status.component.html',
  styleUrl: './status.component.css'
})
export class StatusComponent {
  statusService = inject(StatusService)
  confirmationService = inject(ConfirmationService)
  giftService = inject(GiftService)
  categoryService = inject(CategoryService)
  donorService = inject(DonorService)
  @ViewChild('cd') cd!: TemplateRef<any>;

  status?: Status;
  buttonText: string = ""
  buttonSmallText: string = ""
  events?: EventItem[];
  timeText?: string
  nextTime?: any
  visiblea: boolean = false
  date: Date = new Date()
  minDate?: Date
  type?: number
  showActions:boolean = false
  num : number = 0
  constructor() { }

  ngOnInit() {
    this.getStatus()
    this.minDate = new Date()
  }

  getStatus() {
    console.log("set");
    let neww : boolean = this.status?.text === 'OFF'
    console.log(neww);
    console.log("get");
    this.statusService.get().subscribe(
      data => {
        this.status = data;
        console.log("status:", data);
        localStorage.setItem("status", data.text || "")
        this.initParameters()
        if (data.text === 'SET' && this.num<1){
          this.confirmNew()
          this.showActions=true
        }
        this.num = 3
      },
      error => {
        console.error("Error occurred while fetching status:", error);
      }
    );
  }

  save() {
    console.log(this.date);
    this.date = moment(this.date, 'Hebrew').toDate();
    console.log(this.date);
    
    this.visiblea = false
    if (this.type === 1)
      this.setStatus()
    else this.updateStatus()   
  }

  setStatus() {

    this.statusService.set(this.date).subscribe(
      data => {
        this.getStatus()
        this.initParameters()

      },
      error => {
        console.error("Error occurred while fetching status:", error);
      }
    );
  }
  updateStatus() {
    console.log("update");

    this.statusService.put(this.date).subscribe(
      data => {
        this.getStatus()
        this.initParameters()
      },
      error => {
        console.error("Error occurred while fetching status:", error);
      }
    );
  }

  clear(num: number){
    if (num === 1)
        this.giftService.deleteAll().subscribe()
    if (num === 2)
      this.categoryService.deleteAll().subscribe()
    if (num === 3)
      this.donorService.deleteAll().subscribe()
  }
  initParameters() {
    // this.nextTime = new Date(this.status?.lottoryEnd)
    this.events = [
      { status: 'בעריכה', text: "אתה יכול להגדיר ולערוך את המכירה הסינית. משתמשים יוכלו רק לצפות במתנות שכבר הוספת", icon: 'pi pi-cog', color: '#9C27B0', image: 'c1.png', isActive: false },
      { status: 'פעיל', text: "המכירה פתוחה לציבור. משתמשים יכולים לצפות בה ולקנות כרטיסית. תוכל לצפות במכירות ובקוני הכרטיסים", icon: 'pi pi-shopping-cart', color: '#673AB7', image: 'c2.png', isActive: false },
      { status: 'הסתיים', text: "משתמשים לא יוכלו יותר לקנות כרטיסים, תוכל לערוך את ההגרלה עבור כל מתנה לצפות בדוחות ובזוכים", icon: 'pi pi-gift', color: '#FF9800', image: 'c3.png', isActive: false },
    ];
    switch (this.status?.text) {
      case "SET": {
        this.buttonText = "פתח אפשרות לקניית כרטיסים"
        this.buttonSmallText = "לאחר ההפעלה, משתמשים יוכלו לרכוש כרטיסים, אך לא תוכל לערוך יותר את המתנות"
        this.events[0].isActive = true
        this.timeText = "זמן פתיחת המכירה"
        break;
      }
      case "ON": {
        this.buttonText = "סגור אפשרות לקניית כרטיסים"
        this.buttonSmallText = "משתמשים לא יוכלו לרכוש כרטיסים, תוכל לערוך את ההגרלה על כל מתנה"
        this.events[1].isActive = true
        this.timeText = "זמן ההגרלה"
        break;
      }
      case "OFF": {
        this.buttonText = "פתח מכירה סינית חדשה"
        this.buttonSmallText = "כרטיסים שמשתמשים רכשו ימחקו, תוכל לערוך מחדש את המכירה"
        this.events[2].isActive = true
      }
    }
    console.log(this.events);
  }

  confirm(event: Event) {
    this.type = 1
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: this.buttonSmallText,
      header: 'שינוי סטטוס',
      icon: 'pi pi-info-circle',
      rejectButtonProps: {
        label: 'ביטול',
        severity: 'secondary',
        outlined: true
      },
      acceptButtonProps: {
        label: 'אישור',
      },
      accept: () => {
        this.type = 1;
        if (this.status?.text === 'SET') {
          this.visiblea = true
        }
        else this.save()
      },
      reject: () => {
        this.confirmationService.close
      }
    });
  }

  confirmNew() {
    this.confirmationService.confirm({
        header: 'Are you sure?',
        message: 'Please confirm to proceed.',
    });
}
}
