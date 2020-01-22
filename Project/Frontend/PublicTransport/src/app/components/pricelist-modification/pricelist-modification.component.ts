import { Pricelist } from './../../models/pricelist.model';
import { PricelistHttpService } from './../../services/pricelist-http.service';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-pricelist-modification',
  templateUrl: './pricelist-modification.component.html',
  styleUrls: ['./pricelist-modification.component.scss']
})
export class PricelistModificationComponent implements OnInit {

  activePricelist: Pricelist
  currentDate: Date
  minDate = new Date().toISOString().slice(0,10)
  hourly: number
  daily: number
  monthly: number
  yearly: number

  allPriceLists: Pricelist[];

  constructor(private pricelistService: PricelistHttpService) {
    this.activePricelist = new Pricelist();
  }

  ngOnInit() {
    this.pricelistService.getActivePricelist().subscribe(
      (data: Pricelist) => {
        this.activePricelist = data;
        this.currentDate = this.activePricelist.fromDate;
        this.hourly = this.activePricelist.hourly;
        this.daily = this.activePricelist.daily;
        this.monthly = this.activePricelist.monthly;
        this.yearly = this.activePricelist.yearly;
      },
      (err) => {
        console.log(err);
      }
    );

    this.getAllPriceLists();
  }

  getAllPriceLists() {
    this.pricelistService.getAll().subscribe(
      (data: Pricelist[]) => {
        console.log(data);
        this.allPriceLists = data;
      }
    )
  }

  updatePricelist(newPricelist: Pricelist, form: NgForm){
    newPricelist.fromDate = this.currentDate;
    this.pricelistService.post(newPricelist).subscribe(
      (data) => {
        form.reset();       
        this.getAllPriceLists();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  parseDate(dateString: string): Date {
    if (dateString) {
        return new Date(dateString);
    }
    return null;
}

}
