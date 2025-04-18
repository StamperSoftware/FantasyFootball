import { Component, inject, OnInit } from '@angular/core';
import { AthleteService } from "../../../core/services/athlete.service";
import { Athlete } from "../../../models";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-select-athlete-list',
  standalone: true,
  imports: [],
  templateUrl: './select-list.component.html',
  styleUrl: './select-list.component.scss'
})
export class SelectAthleteListComponent implements OnInit{
  ngOnInit(): void {
    this.getAthletes();
  }
  
  private athleteService = inject(AthleteService);
  private activeModal = inject(NgbActiveModal);
  
  athletes:Athlete[] = [];
  hasErrors = false;
  
  getAthletes() {
    this.athleteService.getAthletes().subscribe({
      next: athletes => this.athletes = athletes,
      error : err => this.hasErrors = true,
    });  
  }
  
  selectAthlete(id:number) {
    this.activeModal.close(id);
  }
}
