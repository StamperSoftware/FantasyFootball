import { Component, inject } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { LeagueService } from "../../../core/services/league.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-create-league',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './create.component.html',
  styleUrl: './create.component.scss'
})
export class CreateLeagueComponent {
    
    private leagueService = inject(LeagueService);
    activeModal = inject(NgbActiveModal);
    name = "";
    createLeague(){
      this.leagueService.createLeague({name:this.name}).subscribe({
        next:(newLeague) => this.activeModal.close(newLeague),
      });
    }
}
