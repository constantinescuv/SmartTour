import { Component } from '@angular/core';
import { NavController, ToastController, PopoverController, ModalController } from '@ionic/angular';
import { PopoverComponent } from 'src/app/components/popover/popover.component';
import { AuthService } from 'src/app/services/auth.service';
import { FeedService } from 'src/app/services/feed.service';
import { DetailComponent } from 'src/app/components/detail/detail.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage {

  constructor(private navController: NavController, 
    private toastController: ToastController,
    private popoverController: PopoverController,
    private authService: AuthService,
    private feedService: FeedService,
    private modalCtrl: ModalController) {}

  feed = [];
  desc = [];
  image=JSON.parse(localStorage.getItem('user')).image;
  firstName=JSON.parse(localStorage.getItem('user')).firstName;
  lastName=JSON.parse(localStorage.getItem('user')).lastName;
  placesVisited=JSON.parse(localStorage.getItem('user')).placesVisited;
  toursCompleted=JSON.parse(localStorage.getItem('user')).toursCompleted;
  loading = true;

  async ngOnInit() {
    if(!localStorage.getItem('user')) {
      this.navController.navigateForward(['login'], { animated: false });
      this.notLoggedInToast();
    } else {
      if(localStorage.getItem('tour') != null) {
        this.navController.navigateForward(['tour'], { animated: false });
      }
      const res = await this.authService.refreshProfile(JSON.parse(localStorage.getItem('user')));
      localStorage.setItem('user', JSON.stringify(res.body));

      const feedRes = await this.feedService.getPosts(JSON.parse(localStorage.getItem('user')));
      this.loading = false;
      this.feed = JSON.parse(JSON.stringify(feedRes.body));
      this.feed = this.feed.sort(this.compare);

      for (var i = 0; i < this.feed.length; i++){
        var s = this.feed[i]["place"];
        s = s.split(',');
        var re = new RegExp(s[0],"g");
        if ((this.feed[i]["place"].match(re) || []).length > 1){
          this.feed[i]["place"] = s[0] + ',' + s[s.length - 1];
        }

        const monthNames = {"01" : "January", "02" : "February", "03" : "March", "04" : "April", "05" : "May", "06" : "June", "07" : "July", "08" : "August", "09" : "September", "10" : "October", "11" : "November", "12" : "December"};
        var date = this.feed[i]["date"].split('/');
        this.feed[i]["date"] = String(parseInt(date[0])) + ' ' + monthNames[date[1]];

        var names = this.feed[i]["checkpointNames"].split('@');
        names.pop();
        this.feed[i]["checkpointNames"] = names;

        this.desc[i] = "Visited " + this.feed[i]["checkpointNames"][0];
        for (var j = 1; j < this.feed[i]["checkpointNames"].length; j++){
          this.desc[i] += ", " + this.feed[i]["checkpointNames"][j];
        }
        this.desc[i] += " in a " + String(this.feed[i]["duration"] + "-hour Tour!");

        var images = this.feed[i]["images"].split('@');
        images.pop();
        this.feed[i]["images"] = images;
      }
      console.log(this.feed.length);
      
    }
  }

  compare(p1, p2) {
    const d1 = p1.dt_created;
    const d2 = p2.dt_created;

    let comparison = 0;
    if (d1 < d2) {
      comparison = 1;
    } else if (d1 > d2) {
      comparison = -1;
    }
    return comparison;
  }

  async notLoggedInToast() {
    const toast = await this.toastController.create({
      message: 'You are not logged in!',
      color: 'danger',
      duration: 2000
    });

    toast.present();
  }

  async presentPopover(ev: any) {
    const popover = await this.popoverController.create({
      component: PopoverComponent,
      event: ev,
      translucent: true
    });
    return await popover.present();
  }

  async presentModal(tour) {
    const modal = await this.modalCtrl.create({
      component: DetailComponent,
      cssClass: 'my-custom-class',
      componentProps: {
        'tour': tour
      }
    });
    return await modal.present();
  }

  expand(tour) {
    this.presentModal(tour);
  }
}

