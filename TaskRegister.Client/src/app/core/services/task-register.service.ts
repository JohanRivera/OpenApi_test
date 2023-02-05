import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { CustomResponse } from '../models/custom-response';
import { DeleteTimeslipDto, SearchTimeslipDto, TaskRegisterDto } from '../models/task-register.model';

@Injectable({
  providedIn: 'root'
})
export class TaskRegisterService {
  controller = 'TaskRegister'

  constructor(private _http: HttpClient) { }

  CreateTimeslip(data: TaskRegisterDto){
    return this._http.post<CustomResponse>(`${environment.urlApi}${this.controller}/CreateTimeslip`,data);
  }

  UpdateTimeslip(data: TaskRegisterDto){
    return this._http.post<CustomResponse>(`${environment.urlApi}${this.controller}/UpdateTimeslip`,data);
  }

  GetTimeslips(data: SearchTimeslipDto){
    return this._http.post<CustomResponse>(`${environment.urlApi}${this.controller}/GetListTimeslips`,data);
  }

  DeleteTimeslip(data: DeleteTimeslipDto){
    return this._http.post<CustomResponse>(`${environment.urlApi}${this.controller}/DeleteTimeslip`,data);
  }
}
