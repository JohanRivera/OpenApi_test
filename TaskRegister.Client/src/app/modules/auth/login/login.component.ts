import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserRegister } from 'src/app/core/models/user.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserService } from 'src/app/core/services/user.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{

  public user: UserRegister = { userName: '', password: '', firstName: '', lastName: ''};
  public fieldTextType: boolean = false;

  constructor(private authService: AuthService, 
    private router: Router,
    private userService: UserService) {}

  ngOnInit(): void {}

  // Metodo que valida el form del login
  onSubmit(form: NgForm)
  {
    if (form.invalid) {
      return;
    }
    
    this.register(this.user); // Llamado de consumo de método de logeo
  }

  // Metodo para autenticar al usuario
  register(data: UserRegister)
  {
    this.userService.CreateUser(data).subscribe(response => {
      if(response.isSuccessful)
        this.authService.doLogin();
      
      else
        Swal.fire({
          icon: 'info',
          text: response.message,
        })
    },(error) =>{
      Swal.fire({
        icon: 'error',
        text: 'Ha ocurrido un error en la creación de usuario.',
      })
    });
  }

  login(){
    return this.authService.doLogin();
  }
}
