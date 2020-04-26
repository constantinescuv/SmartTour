import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TourConfigComponent } from './tour-config/tour-config.component';
import { IonicModule } from '@ionic/angular';
import { SlidesComponent } from './slides/slides.component';
import { StartComponent } from './start/start.component';



@NgModule({
  declarations: [TourConfigComponent, SlidesComponent, StartComponent],
  imports: [
    CommonModule,
    IonicModule
  ],
  exports: [TourConfigComponent, SlidesComponent, StartComponent]
})
export class SharedComponentsModule { }
