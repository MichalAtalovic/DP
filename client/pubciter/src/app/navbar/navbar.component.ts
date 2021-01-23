import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  public showDelimitor = true;
  constructor (){}

  ngOnInit(): void {
  }

  onSubmit(event: Event) {
  }

  toggle(e: any) {
    this.showDelimitor = !this.showDelimitor;
  }

}
