import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ChangeprofilePage } from './changeprofile.page';

const routes: Routes = [
  {
    path: '',
    component: ChangeprofilePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ChangeprofilePageRoutingModule {}
