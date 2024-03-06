import { Component, OnInit, SecurityContext } from '@angular/core';
import { Activity } from 'src/app/Models/activity';
import { ActivityService } from 'src/app/Services/activity.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-sub-home-page',
  templateUrl: './sub-home-page.component.html',
  styleUrls: ['./sub-home-page.component.css']
})
export class SubHomePageComponent implements OnInit{

  activities: Activity[] = [];

  constructor(private activityService: ActivityService,
              private sanitizer: DomSanitizer){}

  ngOnInit(): void {
    this.activityService.GetActivities().subscribe(data => {
      
      data.forEach(element => {
        if(element.activityPics != null){
          //element.activityPics.push('data:image/png;base64,${element.activityPics[0].picValue}') ;
          element.activityPics[0].picValue = this.getImageUrl(element.activityPics[0].picValue);
          console.log("imag", element.activityPics[0].picValue);
        }
 
      });
      this.activities = data;
      console.log("data", data);
      console.log("activity", this.activities);
    });
  }

  getImageUrl(base64ImageData: string): string {
    const binaryImageData = atob(base64ImageData);
    const uint8Array = new Uint8Array(binaryImageData.length);
    for (let i = 0; i < binaryImageData.length; ++i) {
      uint8Array[i] = binaryImageData.charCodeAt(i);
    }
    const blob = new Blob([uint8Array], { type: 'data:image/png;base64' });
    const imageUrl = URL.createObjectURL(blob);
    return imageUrl;
    //return this.sanitizer.bypassSecurityTrustUrl(imageUrl);
  }

}
