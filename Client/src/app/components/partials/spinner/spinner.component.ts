import { Component } from '@angular/core';
import {AsyncPipe, NgIf} from "@angular/common";
import {SpinnerService} from '../../../services/spinner.service';

@Component({
  selector: 'app-spinner',
  standalone: true,
    imports: [
        AsyncPipe,
        NgIf
    ],
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.scss'
})
export class SpinnerComponent {
  constructor(public spinnerService: SpinnerService) {}
}
