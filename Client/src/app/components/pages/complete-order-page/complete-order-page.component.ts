import { Component } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-complete-order-page',
  standalone: true,
  imports: [
    NgIf
  ],
  templateUrl: './complete-order-page.component.html',
  styleUrl: './complete-order-page.component.scss'
})
export class CompleteOrderPageComponent {


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
  }

  goToHome(): void {
    this.router.navigate(['/']);
  }
}
