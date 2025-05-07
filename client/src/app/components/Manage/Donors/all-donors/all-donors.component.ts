import { Component, inject } from '@angular/core';
import { Donor } from '../../../../models/donor';
import { DonorService } from '../../../../services/donor.service';
import { ImportsModule } from '../../../../imports';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { TableRowCollapseEvent, TableRowExpandEvent } from 'primeng/table';
import { ConfirmationService } from 'primeng/api';
import { AllCategoriesComponent } from '../../Categories/all-categories/all-categories.component';

@Component({
  selector: 'app-all-donors',
  imports: [ImportsModule, RouterOutlet, AllCategoriesComponent],
  templateUrl: './all-donors.component.html',
  styleUrl: './all-donors.component.css'
})
export class AllDonorsComponent {
  donorService = inject(DonorService)
  confirmationService = inject(ConfirmationService)
  router = inject(Router)
  activatedRouter = inject(ActivatedRoute)
  my_route: boolean = false;
  openAddDonor: boolean = false
  donors!: Donor[];
  expandedRows: { [key: number]: boolean } = {};
  donorToSent : Donor = new Donor("")
  constructor() { }

  ngOnInit() {
    this.getDonors()
    
    
    this.activatedRouter.url.subscribe(() => {
      let rout = this.activatedRouter.snapshot.url.join('');
      this.my_route = rout.includes("addDodon");
      if (!this.my_route) {
        this.openAddDonor = false;
      }
    });
  }
  ngOnChange() {
    let rout = this.activatedRouter.snapshot.url.join('');
    this.my_route = rout.includes("addDodon")
  }

  getDonors() {
    this.donorService.getAll().subscribe((data) => {
      this.donors = data;
      console.log(this.donors);
    });
    
  }

  addDonor() {
    console.log("add");
    
    this.router.navigate(['manage/donors/addDonor'], { state: { donor: this.donorToSent } }).then(() => {
      this.openAddDonor = true;
    });
    this.getDonors()
  }
  search(event : Event){
    const input = event.target as HTMLInputElement;
    if (input.value.length < 1)
    {
      this.getDonors()
      return;
    }
    this.donorService.search(input.value).subscribe(
      data => this.donors = data,
      error => console.log(error)
      
    )
  }

  async delete(donor: Donor) {
    try {
      if (donor.id) {
        const response = await this.donorService.delete(donor).toPromise();
      }
      this.getDonors()
      console.log(this.donorService);
    } catch (error) {
      console.error('Error fetching categories', error);
    }
  }

  confirm(event: Event, donor: Donor) {
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
        this.delete(donor)
      },
      reject: () => {
        this.confirmationService.close
      }
    });
  }

  update(donor: Donor){
    console.log("update");
    
    this.donorToSent = donor
    this.addDonor()
    this.donorToSent = new Donor("")
  }

  hide() {
    this.getDonors()
    this.router.navigate(['manage/donors'])
    this.openAddDonor = false
  }
  expandAll() {
    // this.expandedRows = this.donors.reduce((acc, p) => (acc[p.id] = true) && acc, {});
  }
  toggleRow(donor: Donor) {
    this.expandedRows[donor.id] = !this.expandedRows[donor.id];
  }
  collapseAll() {
    this.expandedRows = {};
  }
  onRowExpand(event: TableRowExpandEvent) {
    // this.messageService.add({ severity: 'info', summary: 'Product Expanded', detail: event.data.name, life: 3000 });
  }

  onRowCollapse(event: TableRowCollapseEvent) {
    // this.messageService.add({ severity: 'success', summary: 'Product Collapsed', detail: event.data.name, life: 3000 });
  }
  getSeverity(showMe: boolean) {
    switch (showMe) {
      case true:
        return 'success';
      case false:
        return 'warn';
      default:
        return 'danger';
    }
  }
}
