import { Component, OnInit } from '@angular/core';
import { FeedService } from 'src/app/services/feed.service';
import { AlertController, NavController, ToastController, ActionSheetController, PopoverController, LoadingController } from '@ionic/angular';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { DescriptionComponent } from 'src/app/components/description/description.component';
import { CallNumber } from '@ionic-native/call-number/ngx';
import { EmailComposer } from '@ionic-native/email-composer/ngx';


@Component({
  selector: 'app-tour',
  templateUrl: './tour.page.html',
  styleUrls: ['./tour.page.scss'],
})
export class TourPage implements OnInit {

  icons: any[];
  tour: any;
  backup: any;
  automaticClose = true;
  completed = [];
  skipped = [];
  canAdd = 0;
  postData = {
    userId: '',
    images: '',
    date: '',
    duration: 0,
    checkpointNames: '',
    place: ''
    };
  directions = [];
  colors = ['primary', 'secondary', 'tertiary'];

  constructor(private alertController: AlertController, 
              private navController: NavController, 
              private toastController: ToastController, 
              public actionSheetController: ActionSheetController, 
              private feedService: FeedService, 
              private authService: AuthService,
              private http: HttpClient,
              private popoverController: PopoverController,
              private callNumber: CallNumber,
              private emailComposer: EmailComposer,
              private loader: LoadingController) { 
    
  }

  async ngOnInit() {
    if(!localStorage.getItem('user')) {
      this.navController.navigateForward(['login'], { animated: false });
      this.notLoggedInToast();
    }
    else {
      this.displayLoader()
        .then((loader: any) => {

          this.tour = JSON.parse(localStorage.getItem('tour'))['tour'];
          this.backup = JSON.parse(localStorage.getItem('tour'))['backup'];

          if (JSON.parse(localStorage.getItem('completed')) != null && JSON.parse(localStorage.getItem('skipped')) != null && JSON.parse(localStorage.getItem('open')) != null) {
            this.tour[0].open = false;
            this.tour[JSON.parse(localStorage.getItem('open'))].open = true;
            this.completed = JSON.parse(localStorage.getItem('completed'));
            this.skipped = JSON.parse(localStorage.getItem('skipped'));
          }
          else {
            this.tour[0].open = true;
            localStorage.setItem('open', JSON.stringify(0));
            localStorage.setItem('completed', JSON.stringify(this.completed));
            localStorage.setItem('skipped', JSON.stringify(this.skipped));
          }

          this.http.get('assets/subtypeConfig.json').subscribe(res => {
            this.icons = res['icons'];
          });

          if (JSON.parse(localStorage.getItem('tour'))['restaurants'][0] != null)
          {
            var restaurant = JSON.parse(localStorage.getItem('tour'))['restaurants'][0];
            var found = false;
            for(var i = 0; i < this.tour.length; i++) {
              if (this.tour[i].name == restaurant.name) {
                  found = true;
                  break;
              }
            }
            if (JSON.parse(localStorage.getItem('tour'))['restaurantPosition'] != -1 && found == false) {
              this.tour.splice(JSON.parse(localStorage.getItem('tour'))['restaurantPosition'], 0, JSON.parse(localStorage.getItem('tour'))['restaurants'][0]);
              this.tour[JSON.parse(localStorage.getItem('tour'))['restaurantPosition']]['subtype'] = [{'name': 'restaurant'}];
            }
          }
          
          for (var i = 0; i < this.tour.length; i++) {
            this.directions[i] = 'https://www.google.com/maps/dir/' + localStorage.getItem('Latitude') + ',+' + localStorage.getItem('Longitude') + '/' + this.tour[i].latitude + ',+' + this.tour[i].longitude;
          }
          loader.dismiss();
        })
        .catch(err => {
          console.log('got here error');
          //close loader + return Null
          this.loader.dismiss();
          return null;
        });
    }
  }

  async notLoggedInToast() {
    const toast = await this.toastController.create({
      message: 'You are not logged in!',
      color: 'danger',
      duration: 2000
    });

    toast.present();
  }

  async displayLoader() {
    const loading = await this.loader.create({
      message: 'Loading'
    });
    await loading.present();
    return loading;
  }

  async expand(index) {
    const popover = await this.popoverController.create({
      component: DescriptionComponent,
      translucent: true,
      componentProps: {
        'description': this.tour[index].description,
        'title': this.tour[index].name,
        'image': this.tour[index].photo.images.medium.url,
        'rating': this.tour[index].rating
      }
    });
    return await popover.present();
  }

  toggleSection(index) {
    this.tour[index].open = !this.tour[index].open;

    if (this.automaticClose && this.tour[index].open) {
      this.tour.filter((item, itemIndex) => itemIndex != index).map(item => item.open = false);
    }
  }

  toggleItem(index, childIndex) {
    this.tour[index].children[childIndex].open = !this.tour[index].children[childIndex].open;
  }
  
  cancelAlert() {
    var header = 'Close Tour';
    var message = 'You will not be able to return to this tour if you decide to close it and all progress will be lost. Are you sure?';
    var buttons = [
      { text: 'Yes', handler: async () => {
        const toast = await this.toastController.create({
          message: 'Tour cancelled!',
          color: 'danger',
          duration: 2000
        });

        localStorage.removeItem('tour');
        localStorage.removeItem('skipped');
        localStorage.removeItem('completed');
        localStorage.removeItem('open');

        toast.present();

        this.navController.navigateForward(['home/start'], { animated: false });
      }}, 
      'No'];
    this.presentAlert(header, message, buttons);
  }

  async doneAlert(i) {
    var header = 'Confirm';
    var message = 'Mark this checkpoint as completed?';
    var buttons = [
      { text: 'Yes', handler: async () => {
        const toast = await this.toastController.create({
          message: 'Proceed to the next checkpoint!',
          color: 'success',
          duration: 2000
        });

        this.completed[i] = true;
        localStorage.setItem('completed', JSON.stringify(this.completed));
        this.authService.incrementPlaces(JSON.parse(localStorage.getItem("user")));
        this.toggleSection(i);
        if(i + 1 < this.tour.length){
          if(this.canAdd > 0 && this.backup[i + 1].length > 0){
            this.getBackupCheckpoint(i);
          }
          else{
            this.toggleSection(i + 1);
            localStorage.setItem('open', JSON.stringify(i + 1));
          }
        }
        toast.present();
      }}, 
      'No'];
    this.presentAlert(header, message, buttons);
  }

  async finishAlert() {
    if (this.completed.length == 0) {
      var header = 'Error';
      var message = 'Not enough checkpoints visited to complete this Tour! Try canceling it instead.';
      var button = [{ 
        text: 'Ok', handler: () => { console.log('can not complete tour')}
      }];

      this.presentAlert(header, message, button);
    } else {
      var header = 'Confirm';
      var message = 'Mark this tour as completed?';
      var buttons = [
        { text: 'Yes', handler: async () => {
          localStorage.removeItem('tour');
          localStorage.removeItem('open');
          localStorage.removeItem('completed');
          localStorage.removeItem('skipped');
          const toast = await this.toastController.create({
            message: 'Tour completed!',
            color: 'success',
            duration: 2000
          });

          toast.present();

          this.postData.userId = JSON.parse(localStorage.getItem('user'))["userId"];

          for (var i = 0; i < this.tour.length; i++) {
            if (this.completed[i]){
              this.postData.images += this.tour[i].photo.images.medium.url + "@";
              this.postData.checkpointNames += this.tour[i].name + "@";
            }
          }

          var today = new Date();
          var dd = String(today.getDate()).padStart(2, '0');
          var mm = String(today.getMonth() + 1).padStart(2, '0');
          this.postData.date = dd + "/" + mm;

          if (localStorage.getItem('TimeRange') == "less than 2 hours")
            var duration = 2;
          else if (localStorage.getItem('TimeRange') == "less than 5 hours")
            var duration = 5;
          else if (localStorage.getItem('TimeRange') == "less than 8 hours")
            var duration = 8;
        
          this.postData.duration = duration;

          this.postData.place = this.tour[0]['location_string'];

          this.feedService.addPost(this.postData);

          this.authService.incrementTours(JSON.parse(localStorage.getItem("user")));

          this.navController.navigateForward(['home/profile'], { animated: false });
        }
      }, 
        'No'];
      this.presentAlert(header, message, buttons);
    }
    
  }

  async presentAlert(header, message, buttons) {
    const alert = await this.alertController.create({
      header: header,
      message: message,
      buttons: buttons
    });

    await alert.present();
  }

  async skipCheckpointActionSheet(index) {
    var tour = this.tour
    var backup = this.backup

    var options = [];
    for (var i = 0; i < this.backup[index].length; i++){
      var button = this.createButton(index, i);
      options.push(button);
    }

    var skipButton = this.skipButton(index);

    options.push(skipButton);
    
    options.push({
        text: 'Cancel',
        role: 'cancel',
        handler: () => {
          console.log('Cancel clicked');
        }});

    if (options.length == 2){
      const actionSheet = await this.actionSheetController.create({
        header: 'No replacements available for this checkpoint, you will be notified when an additional attraction can be added without causing major changes to the path of your Tour!',
        buttons: options
      });

      await actionSheet.present();
    }
    else{
      const actionSheet = await this.actionSheetController.create({
        header: 'Do any of these places sound better? Change the unwanted checkpoint or skip it entirely!',
        buttons: options
      });

      await actionSheet.present();
    }

  }

  async getBackupCheckpoint(index){
    var options = [];
    for (var i = 0; i < this.backup[index + 1].length; i++){
      var button = this.createAddButton(index + 1, i);
      options.push(button);
    }
    
    options.push({
        text: 'Cancel',
        role: 'cancel',
        handler: () => {
          console.log('Cancel clicked');
        }});

      const actionSheet = await this.actionSheetController.create({
        header: 'We have found some places you might be interested in close by! Choose one to add it to the Tour or keep everything as it is.',
        buttons: options
      });

      await actionSheet.present();
  }

  createButton(index, i) {
    var button = {
      text: this.backup[index][i].name,
      icon: 'arrow-forward',
      handler: () => {
        var aux = this.tour[index];
        this.tour[index] = this.backup[index][i];
        this.backup[index][i] = aux;
        var localTour = JSON.parse(localStorage.getItem('tour'));
        localTour['tour'] = this.tour;
        localTour['backup'] = this.backup;
        localStorage.setItem('tour', JSON.stringify(localTour));
        this.toggleSection(index);
        console.log(this.tour);

      }
    };
    return button
  }

  createAddButton(index, i) {
    var button = {
      text: this.backup[index][i].name,
      icon: 'arrow-forward',
      handler: () => {
        console.log(i);
        this.tour.splice(index, 0, this.backup[index][i]);
        this.backup.splice(index, 0, []);
        this.backup[index + 1].splice(i, 1);
        this.canAdd -= 1;
        this.toggleSection(index);
      }
    };
    return button
  }

  skipButton(i) {
    var button = {
      text: 'Skip',
      role: 'destructive',
      handler: async () => {
        this.skipped[i] = true;
        localStorage.setItem('skipped', JSON.stringify(this.skipped));

        const toast = await this.toastController.create({
          message: 'Checkpoint skipped!',
          color: 'danger',
          duration: 2000
        });
        
        this.canAdd += 1;
        this.toggleSection(i);
        if(i + 1 < this.tour.length){
          this.toggleSection(i + 1);
          localStorage.setItem('open', JSON.stringify(i + 1));
        }
        toast.present();
      }
    };
    return button
  }

  skipCheckpoint(checkpoint) {
    var index = this.tour.findIndex(x => x.name === checkpoint.name);

    this.skipCheckpointActionSheet(index);
  }

  call(number) {
    this.callNumber.callNumber(number, true)
    .then(res => console.log('Launched dialer!', res))
    .catch(err => console.log('Error launching dialer', err));
  }

  email(address) {
    let send_email = {
      to: address,
      cc: [],
      bcc: [],
      attachment: [],
      subject: '',
      body: ''
    }
    this.emailComposer.open(send_email)
  }

}
