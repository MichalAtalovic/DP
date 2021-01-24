import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-publication-card',
  templateUrl: './publication-card.component.html',
  styleUrls: ['./publication-card.component.css']
})
export class PublicationCardComponent implements OnInit {

  @Input() public publication: any;
  constructor() { }

  ngOnInit(): void {
  }

}
