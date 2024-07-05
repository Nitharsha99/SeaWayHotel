import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PicDocument } from 'src/app/Models/picDocument';

@Component({
  selector: 'app-add-activities',
  templateUrl: './add-activities.component.html',
  styleUrls: ['./add-activities.component.css']
})
export class AddActivitiesComponent implements OnInit{

  updateMode: boolean = true;
  pictures: PicDocument[] = [];
  imageWidth: number = 170;
  imageMargin: number = 2;
  imageHeight: number = 110;
  files: File[] = [];
  selectedPictures: string[] = [];

  constructor(private builder: FormBuilder){}

  activityForm: FormGroup = this.builder.group({
    activityName:[''],
    description:['']
  })

  ngOnInit(): void {
    
  }

  addActivity(){

  }

  updateSelectedPics(id: string): void{

  }

  deleteImages(): void{

  }

  onSelect(event: any): void{

  }

  onRemove(event: any): void{

  }

  redirectToBack(): void{

  }

  resetForm(): void{

  }
}
