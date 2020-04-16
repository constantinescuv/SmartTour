"# SmartTour" 

Commits with detailed explanation

#############
Apr 7, 2020:

Front-end:
- created new ionic angular project
- added a bottom navigation bar with the 3 main pages (explore, start tour, profile)
- integrated google maps with geolocation for the "start tour" page, so far just puts a marker on user's current location

#############
Apr 8, 2020:

Front-end:
- more work on "start tour" page, added "configure tour" button which opens up a page with fields and inputs dynamically rendered from a json file ("/assets/tourConfig.json")

##############
Apr 9, 2020:

Front-end:
- designed a private profile page in the "profile" tab with information like name, profile picture, visited places, completed tours and a tweet-like activity feed containing every completed tour with a short description, date and picture

##############
Apr 13, 2020:

Front-end: 
- deleted some unnecessary files
- created new folders for shared components, pages, etc and restructured what i coded so far
- created 3 new pages: the welcome, login and register pages and set the base route of the app to the welcome page which will contain a short "how to use" of the app once it is finished, then takes the user to the login page where he can either register or login

Back-end:
-created sql database for user accounts
-created authentication service with working register and login and connected it to the front-end

##############
Apr 14, 2020:

Front-end:
- modified the login function to add response (of type user) from back-end to "localStorage" (if login is succesfull)
- made explore/login/register pages route to home page if user is set in localStorage
- made the other pages route to the login page if user is not set in localStorage

##############
Apr 15, 2020:

Front-end:
- added form to change profile info like first name, last name, profile picture, reset completed tours, reset visited places
- user will not have to change or rewrite all the profile information just to change his profile picture for example

Back-end:
- changed user database structure to store profile information aswell
- changed register function to add users in the database with default values for non-required fields (0 for completed tours and visited places and a default profile-picture)
- added change profile functionality which updates the user in the database

##############
Apr 16, 2020:

Front-end:
- added coordinates and user input from the geolocation function and "configure tour" to the local storage and created a button which posts this data to the back-end
- saves the response to localStorage

Back-end:
- uses the data recieved from front (just the coordinates so far) and calls the Trip Advisor external API to get points of interest from around those coordinates
- processes the data recieved to keep only the needed information and make it easier to access
- sends a response to the front-end with an object of type TourModel which contains a list of points of interest

##############
#TODO next: 
- find a way to nicely display the Tour to the user and make it editable
- start gathering all the information needed to sort the points of interest in the sorting algorithm
- create the sorting algorithm