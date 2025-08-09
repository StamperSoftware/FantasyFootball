import { Component, inject, OnInit } from '@angular/core';
import { PlayerService } from "../../../core/services/player.service";
import { Player } from "@models";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

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
    private activeModal = inject(NgbActiveModal);
    players?:Player[];
    hasErrors = false;
    
    getPlayers(){
      this.playerService.getPlayers().subscribe({
        next: response => this.players = response,
        error : err => this.hasErrors = true,
      })
    }
    
    handleSelectPlayer(id:number){
      this.activeModal.close(id);
    }
    
}
