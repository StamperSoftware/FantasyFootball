import { Component, inject, OnInit } from '@angular/core';
import { Player } from "@models";
import { PlayerService } from "../../../core/services/player.service";

@Component({
  selector: 'app-player-list',
  standalone: true,
  imports: [],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class PlayerListComponent implements OnInit {
  
  ngOnInit(): void {
      this.getPlayers();
  }

  private playerService = inject(PlayerService);
  
  players?:Player[];
  hasErrors = false;
  
  getPlayers(){
      this.playerService.getPlayers().subscribe({
        next: players => this.players = players,
        error : (err) => this.hasErrors = true
      })
  }
}
