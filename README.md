# Band Tracker

#### A simple application that tracks which bands have played where, 12/2016

#### By **Brain Pritt**

## Description

This application allows the user to enter multiple venues and multiple bands.  It will keep track of the bands that have played at a particular venue, and which venues that a band has played.

## Setup/Installation Requirements

* This application relies on SSMS, Nancy, and other parts of the .NET framework
* _Database must be initialized, to do so from scratch you must create it using sqlcmd:_
  * In SQLCMD:
    * CREATE DATABASE band_tracker
    * GO
    * USE band_tracker
    * GO
    * CREATE TABLE bands (id INT IDENTITY(1,1), band VARCHAR(255), contact VARCHAR(255));
    * GO
    * CREATE TABLE venues (id INT IDENTITY(1,1), venue VARCHAR(255), contact VARCHAR(255))
    * GO
    * CREATE TABLE bands_venues (id INT IDENTITY(1,1), venue_id INT, band_id INT)
    * GO

* _To run application, machine must be running Windows with the latest .NET runtimes_
* _Clone this repository_
* _In terminal, navigate to project directory_
* _run > dnu restore_
* _run > dnx kestrel_
* _In browser window got to: localhost:5004/_


## This application conforms to the following specifications:
* 1 THe application starts with two empty databases.
  * Input: _null_
  * output: _null_
* 2 The application saves venues to the database.
  * Input: Annabells
  * Output: Annabells
* 3 The application will find a specific venue in the database
  * Input: Gund Arena
  * Output: Gund Arena
* 4 The application will allow user to update venue information
  * Input: Gillys Lounge --> Gillys Disco
  * Output: Gillys Disco
* 5 The application will allow user to delete a specific venue
  * Input: Akron Agora --> _delete_
  * Output: _null_
* 6 THe application will allow user to delete all venues
  * Input: Akron Agora, Cleveland Agora, Annabells --> _Delete All_
  * Output: _null_
* 7 The application saves band to database
  * Input: Feds
  * Output: Feds
* 8 The application will find a specific band in the database
  * Input: Light of the Loon
  * Output: LIght of the Loon
* 9 The application will allow user to assign a venue to a Band
  * Input: {No Hope} --> {Cleveland Agora}
  * Output: No Hope: Cleveland Agora
* 10 The application will list all venues in which a band has played
  * Input: Regina Spector
  * Output: Jar Arena, The Flats
* 11 The application will list all bands that have played at a specific venue
  * Input: The Crystal Ballroom
  * Output: Krishna Das, David Stringer

## Known Bugs

At time of commit, there were no known bugs

## Support and contact details

_For comments, questions and bug reports, visit project page:_
* https://github.com/brianpritt/cSharpHairSalon

My personal GitHub page
* https://github.com/brianpritt

## Technologies Used

This application relies on Microsoft .NET.  Other technologies include the Nancy Framework, and Razor view Engine.  HTML, CSS, and JavaScript are used as well.


### License

*Licensed under GPLv3*

Copyright (c) 2016 **Brian Pritt**
