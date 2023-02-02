import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})

export class FooterComponent implements OnInit {

  public year: number = 0;

  ngOnInit(): void {
    this.year = new Date().getFullYear();
  }

}

