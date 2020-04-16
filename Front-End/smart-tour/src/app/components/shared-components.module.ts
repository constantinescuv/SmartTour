import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TourConfigComponent } from './tour-config/tour-config.component';
import { IonicModule } from '@ionic/angular';
import { SlidesComponent } from './slides/slides.component';
import { StartComponent } from './start/start.component';
import { CheckpointComponent } from './checkpoint/checkpoint.component';



@NgModule({
  declarations: [TourConfigComponent, SlidesComponent, StartComponent, CheckpointComponent],
  imports: [
    CommonModule,
    IonicModule
  ],
  exports: [TourConfigComponent, SlidesComponent, StartComponent, CheckpointComponent]
})
export class SharedComponentsModule { }
