import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  canActivate(){
      let role = localStorage.role;
    return localStorage.getItem('jwt') && role === 'Admin';
  }
}