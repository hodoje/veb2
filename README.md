# veb2

**Assignment text**

**1. Assignment description**<br/>
Realize an application for digitalization of public transport. Application should simplify:
  - Evidenting public transport lines. Each line contains stations.
  - Evidenting tickets and passengers. Evident data necessary for evidenting tickets selling whereby there are different types of tickets. Evident usage of tickets used by passengers.
  - Evident schedules and vehicle locations. Predict changes to schedule, so as dynamical vehicle position tracking. Position tracking can be realize through a subsystem that delivers test data about positions.
  
There are four types of users o this system
  - Unregistered user
  - Passenger
  - Controller
  - Administrator
  
**2. System functionalities**<br/>
*2.1 Displayed information to unregistered users*<br/>
First page that a (unregistered) user sees is the home page where he can choose to display schedules, lines grid, vehicle map, pricelist or login/register page.
  - Schedule: a form is shown with possibilities for choosing urban or suburban schedule, day and line.
  - Line grid [one]: displays a grid of existing urban and/or suburban transport, where it is necessary to draw lines grid with existing stations on the map. By clicking on a station, user gets additional information about a station.
  - Current vehicles locations: displays all the vehicles of a chosen line and their current locations on the map.
  - Pricelist: displays a price for a chosen ticket type (hourly (bought ticket is valid in the hour for any line), daily, monthly, yearly) and chosen passenger type (student, retiree, regular).
  
*2.2 User registration - user login*<br/>
On registration/login page, a user can log in using his email address and password.

In case a user is not yet registered, and wants to use the advanced functions of the application, he needs to register first. Registration covers input of email address, password, name, lastname, birthday and address. Password is inputed in two fields so it would prevent making mistakes while choosing a new password.

While registering it is also necessary to define a passenger type (student, retiree, regular). If a user expresses himself as a student (school student or college student), it is necessary that he delivers the system a proof in form of a photograph of his index, while on the other hand, if he expresses himself as a retiree, it is necessary to deliver pension check. Users that don't express or express themselves as regular user do not deliver a document. While registering user is able to skip the part for document delivery and deliver it later on while using the application.

Note: it is necessary to provide a mechanism for autentication and authorization of user on server side.

*2.3 User profile*<br/>
Registered user is able to update his personal data on a profile page, so as to watch and re-delivers his documents.

*2.4 Process of buying a ticket*<br/>
A user is able to buy some of the following types of tickets:
  - Hourly - bought ticket lasts for an hour and can be used for any line
  - Daily - bought ticket lasts until the end of the current day and can be used for any line
  - Monthly - bought ticket lasts until the end of the current month
  - Yearly - bought ticket lasts until the end of the current year
  
// In this part of the text, there is a mistake, instead of *Controller* it should be **Admin**<br/>
User whos profile is not verified by **controller** or didn't deliver the corresponding document has no options of buying a ticket.

*2.5 Process of verifying passenger profile*<br/>
// In this part of the text, there is a mistake, instead of *Controller* it should be **Admin**<br/>
**Controller** has the possibility to check user data and documents and accept or reject a registration request. After accepting, user profile is activated and he can buy tickets normally.

Passenger on his profile has an indicator of profile verification status (Request processing, request accepted, request rejected). Send a mail as a notification.

*2.6 Process of ticket validation*
There is a separate page for ticket validation. Controller inputs an ID of a ticket and the system determines the validity of a ticket based on ticket type and the following rules:
  - For hourly ticket, based on check-in time and current time
  - For others, by defined rules
  
*2.7 Managing lines and stations (lines grid)*<br/>
Admin can manage lines that contain:
  - Line number
  - Stations

Admin can manage stations that contain:
  - Name
  - Address
  - Geo coordinates
  - Line numbers
  
*2.8 Managing the schedule*<br/>
Admin can manage the schedule that contains:
  - Day of the week
  - Dispatches
  - Line number
  
*2.9 Managing the pricelist*<br/>
Admin can manage the pricelist

**3. Implementation of the system**<br/>
Application should be realize as a client-server application. Server part of the application has to be realized using C# programming language (.NET WebApi). Client application has to be realized using Angular framework.

**4. Grading**<br/>
In the following text there are some mandatory requirements that need to be implemented for each grade. Each higher grade requires implementation of all the previous requirements covered under lower grades.
  - All grades: usage of Git is mandatory (loose translation (the rest is irrelevant)).
  - 6 - CRUD operations for unregistered user (can buy only hourly ticket) which includes the admin view. Usage of css styles (bootstrap or material design).
  - 7 - CRUD operations of registered user, managing the pricelist.
  - 8 - Controller (ticket validation), provide transactions on admin level
  - 9 - Displaying of lines on the map
  - 10 - Vehicle position tracking in real time (Web sockets).
