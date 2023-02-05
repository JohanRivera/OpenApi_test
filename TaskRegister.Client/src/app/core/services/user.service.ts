import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { CustomResponse } from '../models/custom-response';
import { UserRegister } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _http: HttpClient) { }

  CreateUser(data: UserRegister)
  {
    return this._http.post<CustomResponse>(`${environment.urlIdp}UserAdministrator/CreateUserIdp`,data)
  }
}
