import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { Donor } from '../../../../models/donor';
import { GiftService } from '../../../../services/gift.service';
import { ImportsModule } from '../../../../imports';
import { DonorService } from '../../../../services/donor.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
@Component({
  selector: 'app-add-donor',
  standalone: true,
  imports: [ImportsModule],
  templateUrl: './add-donor.component.html',
  styleUrl: './add-donor.component.css'
})
export class AddDonorComponent {
  public donorService = inject(DonorService)
  router = inject(Router)
  location = inject(Location)
  donor: Donor = new Donor("");
  // @Output() openAddDonor = new EventEmitter<void>();
  @Output() donorClosed = new EventEmitter<Donor>();

  ngOnInit() {
    this.donor = history.state.donor;
    if (!this.donor)
      this.donor = new Donor("")
  }

  save() {
    if (this.donor.id === -1) {
      this.donorService.add(this.donor).subscribe(response => {
        console.log(response);
        this.donorClosed.emit(this.donor); 
        this.location.back(); 
      });
    } else {
      this.donorService.update(this.donor).subscribe(response => {
        console.log(response);
        this.donorClosed.emit(this.donor); 
        this.location.back(); 
      });
    }
  }

  onImageError() {
    this.donor.logo = "img/d.jpg"
  }
}
