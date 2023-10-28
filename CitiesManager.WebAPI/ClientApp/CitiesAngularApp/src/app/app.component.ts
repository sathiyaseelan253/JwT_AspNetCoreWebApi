import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public username: string;
  constructor(public accountService: AccountService, private router : Router) {
    this.username = accountService.currentUserName;
    console.log("Username: ", this.username);
  }
  onLogout() {
    this.accountService.getLogOutLogin().subscribe({
      next: (res) => {
        this.accountService.currentUserName = null;
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        this.router.navigate(['/login']);
      },
        error: (err) => {
          console.log(err);
      },
      complete: () => {

      }
    })
  }
}
