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
  timeToAdd: string
  timeToUpdate: string

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
    this.timeToUpdate = this.selectedTime;
  }

  currentScheduleChanged(){
    this.timeToUpdate = this.selectedTime;
  }

  getStringTimetableFromArray(){
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
      let finalTimetable = allTimesTogether.replace(/(\.\,)/g, ".");
      return finalTimetable;
  }

  addTime(){
    if(this.timeToAdd){
      if(this.currentScheduleTimetable.indexOf(this.timeToAdd) === -1){
        this.currentScheduleTimetable.push(this.timeToAdd);
        this.currentScheduleTimetable.sort();      
        this.currentSchedule.timetable = this.getStringTimetableFromArray();
        this.scheduleService.put(this.currentSchedule.id, this.currentSchedule).subscribe(
          (confirm) => {
            this.getData();
            this.timeToAdd = undefined;
          },
          (err) => {
            console.log(err);
          }
        );
      }
    }
  }

  removeTime(){
    if(this.selectedTime){
      this.currentScheduleTimetable = this.currentScheduleTimetable.filter((time) => {
        return time !== this.selectedTime;
      });
      this.currentScheduleTimetable.sort();
      this.currentSchedule.timetable = this.getStringTimetableFromArray();
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

  updateTime(){
    if(this.timeToUpdate){
      if(this.currentScheduleTimetable.indexOf(this.timeToUpdate) === -1){
        let idx = this.currentScheduleTimetable.indexOf(this.selectedTime);
        this.currentScheduleTimetable[idx] = this.timeToUpdate;
        this.currentScheduleTimetable.sort();
        this.currentSchedule.timetable = this.getStringTimetableFromArray();
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
}
