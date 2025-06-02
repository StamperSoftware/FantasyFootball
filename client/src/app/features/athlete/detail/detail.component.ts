import { Component, OnInit, inject } from '@angular/core';
import { AthleteService } from "../../../core/services/athlete.service";
import { Athlete, Position } from "../../../models";
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-athlete-detail',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class AthleteDetailComponent implements OnInit {
  ngOnInit(): void {
    this.getAthlete();
  }

  protected readonly Position = Position;
  
  private athleteService = inject(AthleteService);
  private route = inject(ActivatedRoute);
  private fb = inject(FormBuilder);
  
  athleteId = this.route.snapshot.paramMap.get("id");
  athlete?:Athlete;
  
  formGroup = this.fb.group({
    week:[1],
    season:[2025],
    stats : this.fb.group({
      receptions: [0],
      receivingYards: [0],
      receivingTouchdowns: [0],
      passingYards: [0],
      passingTouchdowns: [0],
      rushingYards: [0],
      rushingTouchdowns: [0],
    }),
  });
  
  
  getAthlete(){
    
    if(!this.athleteId) return;
    
    this.athleteService.getAthlete(+this.athleteId).subscribe({
      next: athlete => this.athlete = athlete,
    });  
  }
  
  updateAthleteStats() {
    
    if (!this.athleteId) return;
    
    this.athleteService.updateAthleteStats(+this.athleteId, this.formGroup.value).subscribe({
        next: () => console.log("stats updated"),
    });
  }

}
