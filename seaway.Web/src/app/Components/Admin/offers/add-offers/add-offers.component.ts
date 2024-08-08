import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PicDocument } from 'src/app/Models/picDocument';

@Component({
  selector: 'app-add-offers',
  templateUrl: './add-offers.component.html',
  styleUrls: ['./add-offers.component.css']
})
export class AddOffersComponent implements OnInit{

  updateMode: boolean = false;
  pictures: PicDocument[] = [];
  imageWidth: number = 170;
  imageMargin: number = 2;
  imageHeight: number = 110;
  files: File[] = [];
  selectedPictures: string[] = [];
  offerId !: number;
  picArrayLength: number = 0;

  constructor(private builder: FormBuilder, private router: Router,  private route: ActivatedRoute,){}

  offerForm: FormGroup = this.builder.group({
    offerName:[''],
    description:[''],
    validFrom:[''],
    validTo:[''],
    price:[''],
    discount:[0],
    createdBy: ['Nitharsha'],
    updatedBy: [''],
    activityPics: this.builder.array([
      this.builder.group({
        picName: [null],
        picValue: [null],
        cloudinaryPublicId: [null]
       })
    ])
  })

  ngOnInit(): void {
    
  }

  addOffer(): void{

  }

  updateSelectedPics(){

  }

  deleteImages(): void{

  }

  onSelect(event: any){

  }

  onRemove(file: File): void{

  }

  redirectToBack(): void{

  }

  resetForm(): void{

  }

}
