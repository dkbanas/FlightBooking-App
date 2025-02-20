import {Component, TemplateRef, ViewChild} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {NgClass, NgIf} from '@angular/common';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../../services/auth.service';
import {LoginResponse} from '../../../models/LoginResponse';
import {ToastrService} from 'ngx-toastr';
import {UserAuthRequest} from '../../../models/UserAuthRequest';

@Component({
  selector: 'app-login-modal',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './login-modal.component.html',
  styleUrl: './login-modal.component.scss'
})
export class LoginModalComponent {
  @ViewChild('loginModal') loginModal!: TemplateRef<any>;
  loginForm: FormGroup;
  passwordVisible = false;
  errorMessage: string | null = null;

  constructor(private modalService: NgbModal,private fb: FormBuilder,private authService:AuthService,private toastr: ToastrService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  login() {
    if (this.loginForm.valid) {
      const request:UserAuthRequest = this.loginForm.value;
      this.authService.login(request).subscribe({
        next: (response: LoginResponse) => {
          this.toastr.success('Login successful!', 'Success');
          console.log('Login successful:', response);
          this.modalService.dismissAll();
        },
        error: (error) => {
          console.error('Login failed:', error);
          this.errorMessage = error.error?.message || 'Login failed. Please try again.';
          this.toastr.error("Something went wrong! Please try again", 'Error');
        }
      });
    }
  }

  get email() {
    return this.loginForm.get('email')!;
  }

  get password() {
    return this.loginForm.get('password')!;
  }

  openModal() {
    const modalRef = this.modalService.open(this.loginModal, { ariaLabelledBy: 'loginModalLabel' });
    modalRef.dismissed.subscribe(() => {
      this.loginForm.reset();
      this.errorMessage = null;
    });
  }

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

}
