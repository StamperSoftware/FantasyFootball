import { Component, OnInit, inject } from '@angular/core';
import { AthleteService } from "../../../core/services/athlete.service";
import { Athlete, Position, WeeklyStats } from "@models";
import { ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faEdit } from "@fortawesome/free-solid-svg-icons";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { UpdateAthleteStatsComponent } from "../update-athlete-stats/update-athlete-stats.component";

@Component({
  selector: 'app-athlete-detail',
  standalone: true,
    imports: [
        ReactiveFormsModule,
        FaIconComponent
    ],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class AthleteDetailComponent implements OnInit {
    
  ngOnInit(): void {
    this.getAthlete();
  }

  protected readonly Position = Position;
  
  private modalService = inject(NgbModal);
  private athleteService = inject(AthleteService);
  private route = inject(ActivatedRoute);
  
  athleteId = this.route.snapshot.paramMap.get("id");
  athlete?:Athlete;
  
  getAthlete(){
    
    if(!this.athleteId) return;
    
    this.athleteService.getAthleteWithStats(+this.athleteId).subscribe({
      next: athlete => this.athlete = athlete,
    }); 
  }

    openUpdateStatsModal(stats:WeeklyStats){
       
      if (!this.athleteId) return;
      
      const modal = this.modalService.open(UpdateAthleteStatsComponent);
      modal.componentInstance.data = {...stats};
      modal.result.then(()=>this.getAthlete());
    }
    
    protected readonly faEdit = faEdit;
}
