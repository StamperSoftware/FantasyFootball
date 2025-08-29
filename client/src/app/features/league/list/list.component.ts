import { Component, inject, OnInit } from '@angular/core';
import { LeagueService } from "../../../core/services/league.service";
import { League } from "@models";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faAdd, faTrash } from "@fortawesome/free-solid-svg-icons";
import { RouterLink } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { CreateLeagueComponent } from "../create/create.component";

@Component({
  selector: 'app-league-list',
  standalone: true,
  imports: [
    FaIconComponent,
    RouterLink
  ],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class LeagueListComponent implements OnInit{

  protected readonly faAdd = faAdd;
  private leagueService = inject(LeagueService);
  private modalService = inject(NgbModal);

  leagues?:League[];
  hasErrors:boolean = false;

  ngOnInit(): void {
    this.getLeagues();
  }

  openCreateLeagueModal() {
    this.modalService.open(CreateLeagueComponent).result.then(
        (newLeague) => this.getLeagues(),
        () => {},
    );
  }

  getLeagues(){
    this.leagueService.getLeagues().subscribe(
        {
          next: (response) => this.leagues = response.data,
          error: err => this.hasErrors = true
        }
    )
  }
  
  deleteLeague(leagueId:number){
    this.leagueService.deleteLeague(leagueId).subscribe({
      next: () => this.getLeagues(),
    });
  }
    protected readonly faTrash = faTrash;
}
