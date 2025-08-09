import { Component, inject, Input } from '@angular/core';
import { AthleteService } from "../../../core/services/athlete.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { WeeklyStats } from "@models";
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-update-athlete-stats',
  standalone: true,
    imports: [
        FormsModule
    ],
  templateUrl: './update-athlete-stats.component.html',
  styleUrl: './update-athlete-stats.component.scss'
})
export class UpdateAthleteStatsComponent {

    private activeModal = inject(NgbActiveModal);
    private athleteService = inject(AthleteService);

    @Input() data : WeeklyStats = {
        athleteId: 0,
        week: 0,
        season: 0,
        receptions: 0,
        receivingYards: 0,
        receivingTouchdowns: 0,
        passingYards: 0,
        passingTouchdowns: 0,
        rushingYards: 0,
        rushingTouchdowns: 0,
        score:0,
    };
    
    updateAthleteStats() {
        if (!this.data.athleteId) return;

        this.athleteService.updateAthleteStats(+this.data.athleteId, this.data).subscribe({
            next: () => this.activeModal.close(),
        });
    }
}
