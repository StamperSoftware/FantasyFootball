import { Component, inject, Input, OnInit } from '@angular/core';
import { AthleteService } from "../../../core/services/athlete.service";
import { Athlete, Position } from "@models";
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
  @Input() data? : {athletes?:Athlete[]};
  getAthletes() {
    
    if (this.data?.athletes) {
      this.athletes = this.data.athletes;
      return;
    }
    
    this.athleteService.getAthletes().subscribe({
      next: athletes => this.athletes = athletes,
      error : err => this.hasErrors = true,
    });  
  }
  
  selectAthlete(id:number) {
    this.activeModal.close(this.athletes.find(a => a.id == id));
  }

    protected readonly Position = Position;
}
