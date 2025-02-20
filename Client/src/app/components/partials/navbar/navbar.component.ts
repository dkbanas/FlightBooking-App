import {Component, ViewChild} from '@angular/core';
import {NgbCollapse, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {LoginModalComponent} from '../../modals/login-modal/login-modal.component';
import {RegisterModalComponent} from '../../modals/register-modal/register-modal.component';
import {AuthService} from '../../../services/auth.service';
import {NgIf} from '@angular/common';
import {AccountDetailsResponse} from '../../../models/AccountDetailsResponse';
import {RouterLink, RouterLinkActive} from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    NgbCollapse,
    LoginModalComponent,
    RegisterModalComponent,
    NgIf,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  isNavbarCollapsed = true;
  isLoggedIn = false;
  isAdmin = false;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.authService.isLoggedIn$.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });

    this.authService.isAdmin$.subscribe(admin => {
      this.isAdmin = admin;
    });
  }


  logout(): void {
    this.authService.logout();
  }
}
