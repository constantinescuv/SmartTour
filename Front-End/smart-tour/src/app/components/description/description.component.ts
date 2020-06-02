import { Component, OnInit, Input } from '@angular/core';
import { PopoverController } from '@ionic/angular';

@Component({
  selector: 'app-description',
  templateUrl: './description.component.html',
  styleUrls: ['./description.component.scss'],
})
export class DescriptionComponent implements OnInit {

  @Input() description: any;
  @Input() title: any;
  @Input() image: any;
  @Input() rating: any;
  constructor(private popoverController: PopoverController) { }

  ngOnInit() {}

  async closeModal() {
    await this.popoverController.dismiss();
  }

}
