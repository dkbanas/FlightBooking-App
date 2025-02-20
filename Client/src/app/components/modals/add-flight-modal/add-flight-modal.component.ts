import {Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild} from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AirportService} from '../../../services/airport.service';
import {debounceTime, Subject, switchMap, tap} from 'rxjs';
import {FlightService} from '../../../services/flight.service';
import {FlightRequest} from '../../../models/FlightRequest';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-add-flight-modal',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    FormsModule,
    NgForOf
  ],
  templateUrl: './add-flight-modal.component.html',
  styleUrl: './add-flight-modal.component.scss'
})
export class AddFlightModalComponent{
  @ViewChild('newFlightModal') newFlightModal!: TemplateRef<any>;
  @Output() flightAdded = new EventEmitter<void>();
  newFlightForm: FormGroup;
  departureAirports: any[] = [];
  arrivalAirports: any[] = [];
  departureLocationDisplay = '';
  arrivalLocationDisplay = '';



  constructor(private modalService: NgbModal,private fb: FormBuilder,private airportService:AirportService,private flightService:FlightService,private toastr: ToastrService) {
    this.newFlightForm = this.fb.group({
      flightNumber: ['', Validators.required],
      departureLocationId: ['', Validators.required],
      arrivalLocationId: ['', Validators.required],
      departureTime: ['', Validators.required],
      arrivalTime: ['', Validators.required],
      price: [0, Validators.required],
      airline: ['', Validators.required],
    });
  }

  ngOnInit() {
    this.setupLocationSearch('departure');
    this.setupLocationSearch('arrival');
  }

  setupLocationSearch(locationType: 'departure' | 'arrival') {
    this.newFlightForm.get(`${locationType}LocationId`)?.valueChanges.pipe(
      debounceTime(300),
      switchMap(query => {
        if (query && query.length >= 3) {
          return this.airportService.searchAirports(query);
        } else {
          this.clearAirportResults(locationType);
          return [];
        }
      })
    ).subscribe(airports => {
      this.updateAirports(locationType, airports);
    });
  }

  updateAirports(locationType: 'departure' | 'arrival', airports: any[]) {
    if (locationType === 'departure') {
      this.departureAirports = airports;
    } else {
      this.arrivalAirports = airports;
    }
  }

  clearAirportResults(locationType: 'departure' | 'arrival') {
    if (locationType === 'departure') {
      this.departureAirports = [];
    } else {
      this.arrivalAirports = [];
    }
  }
  onLocationInput(locationType: 'departure' | 'arrival') {
    const locationControl = this.newFlightForm.get(`${locationType}LocationId`)?.get('airport');
    const query = locationControl?.value || '';

    if (query.length >= 3) {
      this.airportService.searchAirports(query).subscribe((airports) => {
        if (locationType === 'departure') {
          this.departureAirports = airports;
        } else {
          this.arrivalAirports = airports;
        }
      });
    } else {
      if (locationType === 'departure') {
        this.departureAirports = [];
      } else {
        this.arrivalAirports = [];
      }
    }
  }

  selectAirport(airport: any, locationType: 'departure' | 'arrival') {
    const locationId = airport.id;

    this.newFlightForm.get(`${locationType}LocationId`)?.setValue(locationId);
    if (locationType === 'departure') {
      this.departureLocationDisplay = `${airport.name} - ${airport.city} - ${airport.country}`;
    } else {
      this.arrivalLocationDisplay = `${airport.name} - ${airport.city} - ${airport.country}`;
    }

    // Clear the airport suggestions
    this.clearAirportResults(locationType);
  }

  create() {
    if (this.newFlightForm.valid) {
      console.log('Form Data:', this.newFlightForm.value); // Debug the form data here
      const formData: FlightRequest = this.newFlightForm.value;
      this.flightService.createFlight(formData).subscribe({
        next: () => {
          this.flightAdded.emit();
          this.modalService.dismissAll();
          this.toastr.success('Flight added successfully!', 'Success');
        },
        error: (err) => {
          console.error('Error creating flight:', err);
          this.toastr.error('Failed to add flight.', 'Error');
        }
      });
    }
  }

  openModal() {
    const modalRef = this.modalService.open(this.newFlightModal, { ariaLabelledBy: 'newAirportModalLabel' });
    modalRef.dismissed.subscribe(() => {
      this.newFlightForm.reset();
    });
  }



  //Getters
  get flightNumber() {
    return this.newFlightForm.get('flightNumber')!;
  }

  get departureLocationId() {
    return this.newFlightForm.get('departureLocationId')!;
  }

  get arrivalLocationId() {
    return this.newFlightForm.get('arrivalLocationId')!;
  }

  get departureTime() {
    return this.newFlightForm.get('departureTime')!;
  }

  get arrivalTime() {
    return this.newFlightForm.get('arrivalTime')!;
  }

  get price() {
    return this.newFlightForm.get('price')!;
  }

  get airline() {
    return this.newFlightForm.get('airline')!;
  }

}
