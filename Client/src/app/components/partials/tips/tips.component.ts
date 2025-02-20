import { Component } from '@angular/core';
import {NgForOf} from '@angular/common';

@Component({
  selector: 'app-tips',
  standalone: true,
  imports: [
    NgForOf
  ],
  templateUrl: './tips.component.html',
  styleUrl: './tips.component.scss'
})
export class TipsComponent {
  componentTitle = 'Tips for Finding Cheap Flights';
  componentDescription = 'Due to the high cost of airline tickets, every traveler wants to know how to fly cheaply. By following these tips, you can save hundreds of dollars. So, how do you find cheap flights?';

  tips = [
    {
      title: 'Plan Ahead',
      icon: '📅', // Calendar emoji
      description: 'Start your search several months before your trip. Booking in advance increases your chances of getting a lower price. Airlines know that travelers with fixed dates are willing to pay more, so early booking can be key to saving money.'
    },
    {
      title: 'Avoid Weekends',
      icon: '🚫', // Prohibited emoji
      description: 'Avoid booking flights on weekends, especially Sundays. That’s when most people search for tickets, and airlines raise prices. It’s better to search during the week, especially in the morning.'
    },
    {
      title: 'Be Flexible with Dates',
      icon: '🔄', // Refresh emoji
      description: 'If possible, be flexible with your travel dates. Often, there are great deals available a day before or after your planned departure.'
    },
    {
      title: 'Follow Budget Airlines',
      icon: '✈️', // Airplane emoji
      description: 'Budget airlines often offer lower prices than traditional carriers. Keep an eye on their promotions.'
    }
  ];
}
