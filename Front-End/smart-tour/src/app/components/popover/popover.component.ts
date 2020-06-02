import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PopoverController } from '@ionic/angular';

@Component({
  selector: 'app-popover',
  templateUrl: './popover.component.html',
  styleUrls: ['./popover.component.scss'],
})
export class PopoverComponent implements OnInit {

  constructor(private router: Router, public popoverController: PopoverController) { }

  ngOnInit() {}
  
  NavigateToChangeProfile(){
    this.router.navigate(['changeprofile']);
    this.popoverController.dismiss().then(() => { this.popoverController = null; });
  
  }

  logout(){
    localStorage.removeItem('user');
    this.router.navigate(['login']);
    this.popoverController.dismiss().then(() => { this.popoverController = null; });
  }
}
