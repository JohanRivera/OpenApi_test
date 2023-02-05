import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { CustomResponse } from '../models/custom-response';
import { CreateProject, ProjectDto } from '../models/projects.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  constructor(private _http: HttpClient) { }

  CreateProject(data: CreateProject){
    return this._http.post<CustomResponse>(`${environment.urlApi}Projects/CreateProject`,data);
  }

  UpdateProject(data: ProjectDto){
    return this._http.post<CustomResponse>(`${environment.urlApi}Projects/UpdateProject`,data);
  }

  GetAllProjects(){
    return this._http.get<CustomResponse>(`${environment.urlApi}Projects/GetListProjects`);
  }
}
