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
    var formData = new FormData();
    formData.append('firstName', data.firstName);
    formData.append('lastName', data.lastName);
    formData.append('password', data.password);
    formData.append('userName', data.userName);
    return this._http.post<CustomResponse>(`${environment.urlIdp}UserAdministrator/CreateUserIdp`,formData)
  }
}
