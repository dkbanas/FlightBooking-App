import {ChangeDetectorRef, Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {NavbarComponent} from './components/partials/navbar/navbar.component';
import {AsyncPipe, NgIf} from '@angular/common';
import {SpinnerComponent} from './components/partials/spinner/spinner.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, NgIf, AsyncPipe, SpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'AirFlyer';


}
