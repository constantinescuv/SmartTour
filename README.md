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
Apr 26, 2020:

Front-end:
- tour page designed as an accordion list with the following functionalities: - user can expand any checkpoint to find data like : address, trip advisor rating, and photos
                                                                              - user can cancel or finish tour and return to the home page

Back-end:
- work on sorting algorithm: - gathered and processed information from both the front-end(provided by the user) and the external api
                             - created function that calculates distance between 2 sets of coordinates of type latitude, longitude (using Haversine formula)
                             - use the previous function to eliminate any of the locations recieved from the Trip Advisor api that don't respect the user's distance range choice
                             - then data is prepared to match the input of a Travelling Salesman Problem (TSP)
                             - to avoid long waiting times, the list will be divided in 2 roughly equal lists of locations: a main list which will be the input for the TSP implementation and a backup list which, if needed, will distribute it's locations in a way that gives the user the opportunity to have other options if some of the generated checkpoints are not considered worth visiting by him.
                             - the main list gets a certain number of random places from the list returned by the external api, this makes the algorithm nondeterministic which is good because if a person generates 2 or more tours from the same location (which will be very possible) the resulting tour will not be the same; the probability of each place to be chosen is not uniform however, places with higher ratings will have a higher chance to be selected.
                             - a distance matrix between the places in the main list is created as it will be needed as input for TSP algorithm
                             - TSP algorithm gets the distance matrix, the start point, the maximum number of checkpoints, the maximum time chosen by the user and if it should be a circuit or not (if it is circuit, the way back to the start point is not counted as being part of the tour anyway, it just adds the start point as the last checkpoint in the tour)
                             - TSP algorithm finds the shortest path with the most checkpoints with the limits provided as input; any unselected checkpoints will be appended to the backup list
                             - a pretty good tour is returned as a response on the front-end and displayed to the user

##############
#TODO next: 
- change the allocated time for each subcategory of places (currently 45 minutes for any category but it should varry, ex: 15 minutes for sightseeing small objectives, 45 minutes for park walks, etc)
- even though higher rated locations are more likely to be found in the main list of objectives, the TSP algorithm does not take rating into consideration when calculating the best tour, this should change
- split the backup list in "n" lists where "n" is the length of the main list, and for each objective in the main list (or the generated tour) there will be a list of other objectives that the inital objective can be switched with and not mess up the tour so they must be close to eachother, maybe have the same subtype, etc 