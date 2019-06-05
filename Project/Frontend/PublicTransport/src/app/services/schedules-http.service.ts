import { BaseHttpService } from './base-http.service';
import { Injectable } from '@angular/core';
import { Schedule } from '../models/schedule.model';

@Injectable()
export class SchedulesHttpService extends BaseHttpService<Schedule>{
  specifiedUrl = "schedules"
}