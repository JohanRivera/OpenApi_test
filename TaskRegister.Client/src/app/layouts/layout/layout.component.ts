import { Component, OnInit } from '@angular/core';
import { ResponsiveService } from 'src/app/core/services/responsive.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {
  constructor(private responsiveService: ResponsiveService){}

  ngOnInit(): void {
    this.onResize();
  }

  onResize() {
    this.responsiveService.checkWidth();
  }
}
