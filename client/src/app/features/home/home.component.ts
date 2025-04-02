import { Component, inject, OnInit } from '@angular/core';
import { HomeService } from "../../core/services/home.service";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  ngOnInit(): void {
    this.homeService.isConnected().subscribe(response => this.isConnected = response);
  }
  isConnected?:any;
  homeService = inject(HomeService);
}
