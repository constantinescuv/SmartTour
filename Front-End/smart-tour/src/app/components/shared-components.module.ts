import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TourConfigComponent } from './tour-config/tour-config.component';
import { IonicModule } from '@ionic/angular';



@NgModule({
  declarations: [TourConfigComponent],
  imports: [
    CommonModule,
    IonicModule
  ],
  exports: [TourConfigComponent]
})
export class SharedComponentsModule { }
