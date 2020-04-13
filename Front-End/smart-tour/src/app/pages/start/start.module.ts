import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { StartPage } from './start.page';
import { ExploreContainerComponentModule } from 'src/app/explore-container/explore-container.module';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  imports: [
    IonicModule,
    CommonModule,
    FormsModule,
    ExploreContainerComponentModule,
    RouterModule.forChild([{ path: '', component: StartPage }]),
    SharedModule 
  ],
  declarations: [StartPage]
})
export class StartPageModule {}
