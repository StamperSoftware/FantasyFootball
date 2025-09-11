import { Component, inject, input, OnInit } from '@angular/core';
import { Player } from "@models";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { LeagueService, PlayerService } from "@services";

@Component({
  selector: 'app-select-player',
  standalone: true,
  imports: [],
  templateUrl: './select-player.component.html',
  styleUrl: './select-player.component.scss'
})
export class SelectPlayerComponent implements OnInit {
    ngOnInit(): void {
      this.getPlayers();
    }
    
    private playerService = inject(PlayerService)
    private leagueService = inject(LeagueService)
    private activeModal = inject(NgbActiveModal);
    players?:Player[];
    hasErrors = false;
    
    leagueId:number|undefined;
    
    getPlayers(){
        if (this.leagueId) {

            this.leagueService.getPlayersNotInLeague(this.leagueId).subscribe({
                next: response => this.players = response,
                error : err => this.hasErrors = true,
            })
        } else {
            this.playerService.getPlayers().subscribe({
                next: response => this.players = response,
                error : err => this.hasErrors = true,
            })
        }
        
    }
    
    handleSelectPlayer(id:number){
      this.activeModal.close(id);
    }
    
}
