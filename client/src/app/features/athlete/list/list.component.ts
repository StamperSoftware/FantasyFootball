import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { AthleteService } from "../../../core/services/athlete.service";
import { Athlete, Position, Team } from "@models";
import { RouterLink } from "@angular/router";
import { PositionDropdownComponent } from "../../../ui/dropdowns/position/position-dropdown.component";
import { TeamComponent } from "../../../ui/dropdowns/team/team.component";
import { FloatingInputComponent } from "../../../components/floating-input/floating-input.component";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faX } from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-athlete-list',
  standalone: true,
  imports: [
    RouterLink,
    TeamComponent,
    FloatingInputComponent,
    PositionDropdownComponent,
    FaIconComponent
  ],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class AthleteListComponent {
  
  constructor () {
    effect(()=> {
      this.getAthletes();
    })
  }
  
  private athleteService = inject(AthleteService);
  athletes:Athlete[] = [];
  hasErrors = false;
  
  team = signal<number|'all'>('all');
  position = signal<Position|'all'>('all');
  search = signal("");
  
  updateSearch(e:Event){
    let target = e.target as HTMLInputElement;
    this.search.set(target.value);
  }
  
  getAthletes(){
    this.athleteService.getAthletes({teamId:this.team(), search:this.search(), position:this.position()}).subscribe({
      next: (response) => this.athletes = response,
      error: err => this.hasErrors = true
    })
  }
  
  resetFilters(){
    this.team.set("all")
    this.position.set("all")
    this.search.set("");
  }
  
  protected readonly Position = Position;
  protected readonly faX = faX;
}
