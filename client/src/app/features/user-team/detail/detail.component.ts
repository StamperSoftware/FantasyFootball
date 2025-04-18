import { Component, inject, OnInit } from '@angular/core';
import { UserTeamService } from "../../../core/services/user-team.service";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { UserTeam } from "../../../models";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faEdit, faPlus } from "@fortawesome/free-solid-svg-icons";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { EditUserTeamComponent } from "../edit/edit.component";
import { AthleteListComponent } from "../../athlete/list/list.component";
import { SelectAthleteListComponent } from "../../athlete/select-list/select-list.component";

@Component({
  selector: 'app-user-team-detail',
  standalone: true,
    imports: [
        FaIconComponent,
        RouterLink
    ],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class UserTeamDetailComponent implements OnInit {
    
    ngOnInit(): void {
      this.getTeam();
    }
    
    private teamService = inject(UserTeamService);
    private activatedRoute = inject(ActivatedRoute);
    private modalService = inject(NgbModal);
    
    protected readonly faEdit = faEdit;
    protected readonly faPlus = faPlus;
    
    team?:UserTeam;
    hasErrors = false;
    
    teamId = this.activatedRoute.snapshot.paramMap.get("id");
  
    getTeam(){
      if(!this.teamId) {
        this.hasErrors = true;
        return;
      }
      
      this.teamService.getTeam(+this.teamId).subscribe({
        next: response => this.team = response,
        error: err => this.hasErrors = true
      })
    }
    
    openEditModal(){
        const modal = this.modalService.open(EditUserTeamComponent);
        modal.componentInstance.data = {name:this.team?.name, id :this.teamId};
        modal.result.then(()=>this.getTeam());
    }

    openAddAthleteModal() {
        const modal = this.modalService.open(SelectAthleteListComponent);
        modal.result.then((id:number) => this.addAthleteToTeam(id));
    }
    
    addAthleteToTeam(athleteId:number){
        if (this.teamId == null) return;
        
        this.teamService.addAthleteToTeam(athleteId, +this.teamId).subscribe({
            next: () => this.getTeam(),
            error : () => this.hasErrors = true,
        });
    }
    
}
