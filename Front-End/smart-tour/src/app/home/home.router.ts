import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePage } from './home.page'

const routes: Routes = [
  {
    path: 'home',
    component: HomePage,
    children: [
        {
            path:'start',
            loadChildren:() => import('../pages/start/start.module').then(
                m => m.StartPageModule
            )
        },
        {
            path:'explore',
            loadChildren:() => import('../pages/explore/explore.module').then(
                m => m.ExplorePageModule
            )
        },
        {
            path:'profile',
            loadChildren:() => import('../pages/profile/profile.module').then(
                m => m.ProfilePageModule
            )
        },
        {
            path:'tutorial',
            loadChildren:() => import('../pages/tutorial/tutorial.module').then(
                m => m.TutorialPageModule
            )
        }
    ]
  }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
    
})
export class HomeRouter {}