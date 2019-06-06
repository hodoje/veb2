import { DayOfTheWeek } from './../../models/day-of-the-week.model';
import { DayOfTheWeekHttpService } from './../../services/day-of-the-week-http.service';
import { TransportationLinesHttpService } from './../../services/transportation-lines-http.service';
import { Schedule } from './../../models/schedule.model';
import { SchedulesHttpService } from '../../services/schedules-http.service';
import { Component, OnInit } from '@angular/core';
import { TransportationLineTypesHttpService } from 'src/app/services/transportation-line-types-http.service';
import { TransportationLine } from 'src/app/models/transportationline.model';
import { TransportationLineType } from 'src/app/models/transportationlinetype.model';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {
  days: DayOfTheWeek[]
  currentDay: DayOfTheWeek
  allLineTypes: TransportationLineType[]
  currentLineType: TransportationLineType
  allLines: TransportationLine[]
  currentLines: TransportationLine[]
  currentLine: TransportationLine
  allSchedules: Schedule[]
  currentSchedules: Schedule[]
  currentSchedule: Schedule
  isScheduleShown: Boolean
  currentScheduleTimetable: string[]

  constructor(private scheduleService: SchedulesHttpService,
              private transportationLinesService: TransportationLinesHttpService,
              private transportationLineTypesService: TransportationLineTypesHttpService,
              private daysService: DayOfTheWeekHttpService) 
  {
    this.days = [];
    this.allLineTypes = [];
    this.allLines = [];
    this.allSchedules = [];
    this.currentLines = [];
    this.currentSchedules = [];
    this.isScheduleShown = false;
    this.currentScheduleTimetable = [];
  }

  ngOnInit() {
    this.getData();
  }

  getData(){
    this.getDaysData();
  }

  getDaysData(){
    this.daysService.getAll().subscribe(
      (data) => {
        this.days = data;
        this.currentDay = this.days[0];
      },
      (err) => {
        console.log(err);
      },
      () => {
        this.getTransportationLineTypesData();
      }
    );
  }

  getTransportationLineTypesData(){
    this.transportationLineTypesService.getAll().subscribe(
      (data: TransportationLineType[]) => {
        this.allLineTypes = data;
        this.currentLineType = this.allLineTypes[0];
      },
      (err) => {
        console.log(err);
      },
      () => {
        this.getTransportationLinesData();
      }
    );
  }

  getTransportationLinesData(){
    this.transportationLinesService.getAll().subscribe(
      (data: TransportationLine[]) => {
        this.allLines = data;
        this.currentLines = this.allLines.filter(l => l.transportationLineType.name === this.currentLineType.name);
        this.currentLine = this.currentLines[0];
      },
      (err) => {
        console.log(err);
      },
      () => {
        this.getSchedulesData();
      }
    );
  }

  getSchedulesData(){
    this.scheduleService.getAll().subscribe(
      (data: Schedule[]) =>{
        this.allSchedules = data;
        //console.log(this.allSchedules);
      },
      (err) =>{
        console.log(err);
      }
    );
  }

  currentLineTypeChanged(){
    this.currentLines.length = 0;
    this.currentLines = this.allLines.filter(l => l.transportationLineType.name === this.currentLineType.name);
  }

  showSchedule(){
    this.currentSchedules.length = 0;
    this.currentScheduleTimetable.length = 0;
    this.currentSchedules = this.allSchedules.filter(s => s.lineNum === this.currentLine.lineNum);
    this.currentSchedule = this.currentSchedules.find(s => s.dayOfTheWeek === this.currentDay.name);
    this.currentScheduleTimetable = this.currentSchedule.timetable.split(".");
    this.currentScheduleTimetable = this.currentScheduleTimetable.map(row => {
      return row.replace(",", " ");
    });
    this.isScheduleShown = true;
  }
}