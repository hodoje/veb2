import { Component, OnInit } from '@angular/core';
import { DayOfTheWeek } from 'src/app/models/day-of-the-week.model';
import { TransportationLineType } from 'src/app/models/transportationlinetype.model';
import { TransportationLine } from 'src/app/models/transportationline.model';
import { Schedule } from 'src/app/models/schedule.model';
import { SchedulesHttpService } from 'src/app/services/schedules-http.service';
import { TransportationLinesHttpService } from 'src/app/services/transportation-lines-http.service';
import { TransportationLineTypesHttpService } from 'src/app/services/transportation-line-types-http.service';
import { DayOfTheWeekHttpService } from 'src/app/services/day-of-the-week-http.service';
import { Time } from '@angular/common';

@Component({
  selector: 'app-schedules-modification',
  templateUrl: './schedules-modification.component.html',
  styleUrls: ['./schedules-modification.component.scss']
})
export class SchedulesModificationComponent implements OnInit {
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
  currentScheduleTimetable: string[]
  selectedTime: string
  timeToAdd: Time

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
        this.currentLineChanged();
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

  currentLineChanged(){
    this.currentSchedules.length = 0;
    this.currentScheduleTimetable.length = 0;
    this.currentSchedules = this.allSchedules.filter(s => s.lineNum === this.currentLine.lineNum);
    let separators = [",", ".", " "];
    this.currentSchedule = this.currentSchedules.find(s => s.dayOfTheWeek === this.currentDay.name);
    this.currentScheduleTimetable = this.currentSchedule.timetable.split(new RegExp('[' + separators.join('|') + ']', 'g'));
    this.selectedTime = this.currentScheduleTimetable[0];
  }

  addTime(newTime){
    if(newTime){
      this.currentScheduleTimetable.push(newTime);
      this.currentScheduleTimetable.sort();
      // Algorithm for setting a '.' on last time in an hour
      // taking the end time so when we can check if we should put a '.' or not
      let lastTime = this.currentScheduleTimetable[this.currentScheduleTimetable.length - 1];
      let lastDoubleDigitHour = lastTime.split(":");
      let lastHour = lastDoubleDigitHour[0];
      for(var i = 0; i < this.currentScheduleTimetable.length; i++){
        let time = this.currentScheduleTimetable[i];
        let doubleDigitHour = time.split(":");
        let hour = doubleDigitHour[0];
        if(hour != lastHour){ 
          let nextTime = this.currentScheduleTimetable[i + 1];
          let nextDoubleDigitHour = nextTime.split(":");
          let nextHour = nextDoubleDigitHour[0];
          if(nextHour > hour){
            this.currentScheduleTimetable[i] = `${time}.`;
          }
        }
      }
      // At this moment end times in an hour have '.' at the end
      let allTimesTogether = this.currentScheduleTimetable.join();
      // At this moment allTimesTogether is a string where between each item is a ','
      // only on end times we have '.,', that's why we split there
      let finalTimetable = allTimesTogether.replace(".,", ".");
      // Update on backend
      this.currentSchedule.timetable = finalTimetable;
      this.scheduleService.put(this.currentSchedule.id, this.currentSchedule).subscribe(
        (confirm) => {
          this.getData();
        },
        (err) => {
          console.log(err);
        }
      );
    }
  }
}
