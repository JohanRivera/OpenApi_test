import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as $ from "jquery";
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public userName: string = '';
  
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    $('#sidebarCollapse').on('click', function () {
      $('#sidebar').toggleClass('active');
      $(this).toggleClass('active');
    });
    $('#sidebarCollapse').click();
    this.getNameUserLogIn()
  }

  getNameUserLogIn() {
    this.authService.userData.subscribe(response => {
      this.userName = response.userData["given_name"]
    });
  }

  logout() 
  {
    this.authService.signOut();
    this.router.navigate(['/login']);
  }
}
