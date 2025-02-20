import {Component, EventEmitter, Output, TemplateRef, ViewChild} from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AirportService} from '../../../services/airport.service';
import {AirportRequest} from '../../../models/AirportRequest';
import {AirportResponse} from '../../../models/AirportResponse';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-add-airport-modal',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    NgForOf
  ],
  templateUrl: './add-airport-modal.component.html',
  styleUrl: './add-airport-modal.component.scss'
})
export class AddAirportModalComponent {
  @ViewChild('newAirportModal') newAirportModal!: TemplateRef<any>;
  @Output() airportAdded = new EventEmitter<void>();
  newAirportForm: FormGroup;
  selectedFile: File | null = null;
  continents: string[] = ['Africa', 'Asia', 'Europe', 'North America', 'Australia', 'South America'];

  constructor(private modalService: NgbModal,private fb: FormBuilder,private airportService:AirportService,private toastr: ToastrService) {
    this.newAirportForm = this.fb.group({
      name: ['', Validators.required],
      code: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      continent: ['', Validators.required]
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }



  create() {
    if (this.newAirportForm.valid) {
      const airportRequest: AirportRequest = this.newAirportForm.value;

      if (this.selectedFile) {
        // Step 1: Upload the image
        this.airportService.uploadImage(this.selectedFile, airportRequest.code).subscribe({
          next: (response: any) => {
            airportRequest.cityPhotoUrl = response.imageUrl;

            this.createAirport(airportRequest);
          },
          error: (error) => {
            console.error('Image upload failed:', error);
          },
        });
      } else {
        this.createAirport(airportRequest);
      }
    }
  }

  private createAirport(airportRequest: AirportRequest) {
    this.airportService.createAirport(airportRequest).subscribe({
      next: (response: AirportResponse) => {
        console.log('Created airport successfully:', response);
        this.airportAdded.emit();
        this.modalService.dismissAll();
        this.toastr.success('Airport added successfully!', 'Success');
      },
      error: (error) => {
        console.error('Creating airport failed:', error);
        this.toastr.error('Failed to add airport.', 'Error');
      },
    });
  }

  openModal() {
    const modalRef = this.modalService.open(this.newAirportModal, { ariaLabelledBy: 'newAirportModalLabel' });
    modalRef.dismissed.subscribe(() => {
      this.newAirportForm.reset();
    });
  }

  get name() {
    return this.newAirportForm.get('name')!;
  }

  get code() {
    return this.newAirportForm.get('code')!;
  }

  get city(){
    return this.newAirportForm.get('city')!;
  }

  get country(){
    return this.newAirportForm.get('country')!;
  }

  get continent() {
    return this.newAirportForm.get('continent')!;
  }
}
