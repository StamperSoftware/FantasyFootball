import { Component, inject, OnInit } from '@angular/core';
import { AthleteService } from "../../../core/services/athlete.service";
import { Athlete, getPositionName, Position } from "../../../models";

@Component({
  selector: 'app-athlete-list',
  standalone: true,
  imports: [],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class AthleteListComponent implements OnInit {
  ngOnInit(): void {
    this.getAthletes();
  }
  
  
  private athleteService = inject(AthleteService);
  athletes:Athlete[] = [];
  hasErrors = false;
  
  
  
  getAthletes(){
    this.athleteService.getAthletes().subscribe({
      next:(response) => this.athletes = response,
      error: err => this.hasErrors = true
    })
  }


  protected readonly getPositionName = getPositionName;
  protected readonly Position = Position;
}
