https://personnelmanager.northeurope.cloudapp.azure.com/download/index.html

Database:

Abandoned /Databases/Staff.db

Currently has ability to create empty database in .../User/AppData/PManager/ by template (Change appsettings.json "ConnectionType": "LOCAL").
Or use API access to remote db insted

# Topic  
Personnel Manager. A program for managing user schedules and planning events (work schedules, salary dates, activities) in a calendar format.

# Goal  
A desktop application developed using WPF technology, targeted for use in a local network.

# General Functionality  
+ Differentiation of user roles within the application (Admin, Manager, User).  
+ System for individual event schedules and work reports.  

## User Roles  

### User / Manager  
+ View event schedules (work dates/times, salary days).  
+ Work reports (attendance marking).  

### Manager  
+ Setting schedules for Users.  

### Admin  
+ User creation (login and password generation, simulation of account issuance). Users can later update their profiles with additional information.  

## UI Elements  
Control elements include a GUI with a menu-based system of various windows (all windows exist as single instances and are not open simultaneously).  

+ **Login/Registration window**  
+ **Menu window**  
+ **MessageBox** for action confirmations  

### Event Schedule  
+ Part of the menu sections.  
+ Includes a calendar displaying work schedules and events in an accessible format.  

### Login Window  
+ **Login input field**  
+ **Password input field**  
+ **Login button** – navigates to the menu window.  
+ **Registration button (Sign Up)** – switches input fields to:  
    + Full Name (in "First Last" format).  
    + Login.  
    + Generate button to the right of the login input field – generates a login based on the entered name (Format: Surn_aa11, where **Surn** is the first 4 letters of the last name transliterated into English, **aa** are 2 random letters, **11** are 2 random digits).  
    + Password.  
    + Generate button to the right of the password input field – generates a random password.  
    + Password confirmation (autofills upon generation).  
    + Phone.  
    + Email address.  
    + Sign Up button – after registration, users must log in to their account.  
    + Back to Log In button.  

### Main Window  
+ **Menu sections:**  
    + **Schedule:**  
        + Buttons to view the schedule by day/week/month.  
    + **Personnel** – view employees and options for administrators:  
        + Add Event.  
        + Edit Staff – allows changing phone, email, password, and role.  
    + **Add User section** – exclusive to administrators for registering new users with the following details:  
        + Full Name (in "First Last" format).  
        + Login.  
        + Generate button to the right of the login input field – generates a login based on the entered name (Format: Surn_aa11, where **Surn** is the first 4 letters of the last name transliterated into English, **aa** are 2 random letters, **11** are 2 random digits).  
        + Password.  
        + Generate button to the right of the password input field – generates a random password.  
        + Password confirmation (autofills upon generation).  
        + Phone.  
        + Email address.  
        + Sign Up button – after registration, users must log in to their account.  
        + Back to Log In button.  
    + **My Profile** – view and update some information:  
        + Change Phone (Change button to the right of the Phone field).  
        + Change Email (Change button to the right of the Email field).  
        + Change Password button – opens a window with:  
            + Old Password input field.  
            + New Password input field.  
            + Change Password button.  
+ **Log Out button**.
