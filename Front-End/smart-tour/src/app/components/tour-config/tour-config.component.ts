import { Component, OnInit, Input } from '@angular/core';
import { ToastController, ActionSheetController } from '@ionic/angular';

@Component({
  selector: 'app-tour-config',
  templateUrl: './tour-config.component.html',
  styleUrls: ['./tour-config.component.scss'],
})
export class TourConfigComponent implements OnInit {

  @Input('tourConfig') tourConfig: any;

  constructor(private toastCtrl: ToastController, private actionSheetController: ActionSheetController) { }

  ngOnInit() {}

  async selectItem(tourConfig) {
    localStorage.setItem(tourConfig.type, tourConfig.name);
    if (tourConfig.type == 'TimeRange') {
      await this.EatingBreakPopup();
    }
    let toast = await this.toastCtrl.create({
      message: `${tourConfig.name} set`,
      duration: 2000
    });
    toast.present();
  }

  
  async EatingBreakPopup() {
    var options = [];

    options.push({
      text: 'No',
      role: 'cancel',
      handler: () => {
        this.noBreak();
      }});

    options.push({
        text: 'Yes',
        handler: () => {
          this.searchRestaurants();
        }});

    const actionSheet = await this.actionSheetController.create({
      header: 'The time range is big enough to add an additional stop at a restaurant to eat. Would you like to see the options?',
      buttons: options
    });

    await actionSheet.present();
  }
  noBreak() {
    localStorage.setItem('EatingBreak', 'false');
  }

  searchRestaurants() {
    localStorage.setItem('EatingBreak', 'true');
  }

}
