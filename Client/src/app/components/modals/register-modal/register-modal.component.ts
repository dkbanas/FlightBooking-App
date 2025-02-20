import {Component, EventEmitter, Output, TemplateRef, ViewChild} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {NgClass, NgIf} from "@angular/common";
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AuthService} from '../../../services/auth.service';
import {UserAuthRequest} from '../../../models/UserAuthRequest';

@Component({
  selector: 'app-register-modal',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './register-modal.component.html',
  styleUrl: './register-modal.component.scss'
})
export class RegisterModalComponent {
  @ViewChild('registerModal') registerModal!: TemplateRef<any>;
  registerForm: FormGroup;
  passwordVisible = false;
  confirmPasswordVisible = false;
  errorMessage: string | null = null;

  constructor(private modalService: NgbModal,private fb: FormBuilder,private authService: AuthService) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', [Validators.required]]
    },{ validators: this.matchValidator('password', 'confirmPassword') });


  }

  register() {
    if (this.registerForm.valid) {
      const request: UserAuthRequest = this.registerForm.value;
      this.authService.register(request).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          this.modalService.dismissAll();
        },
        error: (error) => {
          console.error('Registration failed:', error);
          this.errorMessage = error.error?.message || 'Login failed. Please try again.';
        }
      });
    }
  }

  get email() {
    return this.registerForm.get('email')!;
  }

  get password() {
    return this.registerForm.get('password')!;
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword')!;
  }

  openModal() {
    const modalRef = this.modalService.open(this.registerModal, { ariaLabelledBy: 'registerModalLabel' });
    modalRef.dismissed.subscribe(() => {
      this.registerForm.reset();
      this.errorMessage = null;
    });
  }

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

  toggleConfirmPasswordVisibility() {
    this.confirmPasswordVisible = !this.confirmPasswordVisible;
  }

  matchValidator(controlName: string, matchingControlName: string): ValidatorFn {
    return (abstractControl: AbstractControl) => {
      const control = abstractControl.get(controlName);
      const matchingControl = abstractControl.get(matchingControlName);

      if (matchingControl?.errors && !matchingControl.errors['confirmedValidator']) {
        return null;
      }

      if (control?.value !== matchingControl?.value) {
        const error = { confirmedValidator: true };
        matchingControl?.setErrors(error);
        return error;
      } else {
        matchingControl?.setErrors(null);
        return null;
      }
    };
  }

}
