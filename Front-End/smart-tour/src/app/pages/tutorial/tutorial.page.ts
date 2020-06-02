import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IonSlides } from '@ionic/angular';

@Component({
  selector: 'app-tutorial',
  templateUrl: './tutorial.page.html',
  styleUrls: ['./tutorial.page.scss'],
})
export class TutorialPage implements OnInit {

  @ViewChild('slides', {static: true}) slides: IonSlides;

  constructor() { }

  slideOpts = {
    initialSlide: 0,
    speed: 400,
    pagination: top
  };
  currentIndex:Number = 0;

  ngOnInit() {
  }

  getSlideIndex(e: any){
    this.slides.getActiveIndex().then(
      (index)=>{
        this.currentIndex = index;
        console.log(index);
     });
   }

}
