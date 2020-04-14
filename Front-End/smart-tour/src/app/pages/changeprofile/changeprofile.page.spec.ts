import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ChangeprofilePage } from './changeprofile.page';

describe('ChangeprofilePage', () => {
  let component: ChangeprofilePage;
  let fixture: ComponentFixture<ChangeprofilePage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeprofilePage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ChangeprofilePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
