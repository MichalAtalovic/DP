import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthorService } from '../services/author.service';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, BaseChartDirective, Label } from 'ng2-charts';
import { PublicationService } from '../services/publication.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  @ViewChild(BaseChartDirective) public chartComponent: any;

  public panelData = { header: 'Dashboard', iconPath: 'assets/dashboard_fade.png' }
  public author: any;
  public graphFontSizeValue: any;

  public lineChartData: ChartDataSets[] = [];
  public lineChartLabels: Label[] = [];
  public lineChartOptions: (ChartOptions & { annotation: any }) = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      xAxes: [{
        ticks: {
          maxRotation: 90
        }
      }],
      yAxes: [
        {
          id: 'y-axis-0',
          position: 'left',
          ticks: { }
        }
      ]
    },
    annotation: null
  };
  public lineChartColors: Color[] = [
    {
      backgroundColor: 'rgba(0,0,255,0.5)',
      borderColor: 'blue',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ];
  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';

  @ViewChild(BaseChartDirective, { static: true }) chart!: BaseChartDirective;

  constructor(
    public authorService: AuthorService,
    public publicationService: PublicationService
  ) { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    Promise.all([
      this.authorService.getAuthors(),
      this.publicationService.getPublications()
    ]).then(values => {
      if (values[0] && (values[0] as any[]).length > 0) {
        this.author = (values[0] as any[])[0];
        this.graphFontSizeValue = this.author.settings.graphFontSize;
        this.fontSizeChanged(this.graphFontSizeValue);
      }

      if (values[1] && (values[1] as any[]).length > 0) {
        console.log(values[1] as any[]);

        const yearData = {} as any;
        (values[1] as any[])?.forEach(x => {
          (x.publicationCitationList as any[])?.forEach(q => {
            if (yearData[q.citation.publicationYear] != null) {
              yearData[q.citation.publicationYear] += 1;
            }
            else {
              yearData[q.citation.publicationYear] = 1;
            }
          });
        });

        const currentYear = new Date().getFullYear();
        const years: number[] = [];

        for (const property in yearData) {
          if (!isNaN(property as any))
            years.push(Number(property));
        }

        const minYear = Math.min(...years);

        for (let i = minYear; i <= currentYear; i++) {
          if (yearData[i.toString()] == null) {
            yearData[i.toString()] = 0;
          }
        }

        const data = [] as any;
        for (let i = minYear; i <= currentYear; i++) {
          this.lineChartLabels.push(i.toString());
          data.push(yearData[i.toString()]);
        }

        this.lineChartData = [{ data, label: 'Incidention' }];
      }
    });
  }

  changeChartType(args: any) {
    this.lineChartType = args.value;
  }

  fontSizeChanged(value: any) {
    (this.lineChartOptions as any).scales.xAxes[0].ticks.fontSize = value;
    (this.lineChartOptions as any).scales.yAxes[0].ticks.fontSize = value;

    setTimeout(() => {
      this.chartComponent.refresh();
    }, 10);
  }

}
