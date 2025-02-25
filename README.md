Description

The Dynamic Time-Table Generator is a web application built using .NET MVC that allows users to create a dynamic timetable based on user input. 
The system takes input from the user, stores the data in a database, and then generates a dynamic timetable based on the provided details.

Features

1) User input for the number of working days, subjects per day, and total subjects.

2) Storage of user input in a database.

3) A second step where users enter subjects and their respective total hours.

4) Automatic generation of a timetable based on subject hours.


Workflow:-

1. Timetable Creation Page (TimeTableCreation.cshtml)

   Located in Views -> TTMS.

   The page allows users to input:
        Number of working days (1-7)
        Number of subjects per day (<9)
        Total subjects

   After submission, these values are stored in the database.

   On successful saving, the system redirects the user to the next page.

2. Subject and Hour Submission Page

   The user is prompted to enter subject names and allocate hours for each subject.

   The total subject hours must match the calculated total hours for the week.

   Once all hours are assigned correctly, click on "Generate" button.

3. Timetable Generation

   Upon clicking the "Generate" button, a dynamic timetable is created.

   The table is structured as follows:
       Columns represent working days.
       Rows represent subjects per day.
       Subjects are distributed dynamically based on the entered hours.


Equation for Timetable Calculation:- 
    Total hours for the week = Number of Working Days × Number of Subjects per Day
    The system ensures that the total subject hours match the total hours for the week before generating the timetable.


Implementation Details:-

   Frontend: Razor Views (.cshtml), HTML, CSS, jQuery
   Backend: .NET MVC / .NET (C#)
   Database: SQL Server for storing input values


This README provides an overview of the project, explaining its purpose, structure, and functionality.

