Ninja Delivery Notes
______________________________________________________________________________________________________

Basic delivery calculator between warehouses and destinations:

Summary:
_________
Click Route Calculator in menu to view route for each driver.
Algorithm based on Melbourne bay area. Melbourne has densities west and north and south east of CBD
Delivery points distributed between both Warehouses based on above note.
Drivers assigned to warehouse for now on delivery points for each Warehouse
Travelling salesman algorithm for routes to give a rough optimisation. This could certainly be improved by running distances via a road calulator.


Display and technology:
_______________________
React with Gooogle libraries
Simple web api with routing on the backend.
As this is a demo for now I haven't used a SQL backend for locations. This could easily be ported into a entity model with dynamic locations lists depending on business requirements.
This is demonstration application. I've made assumptions about drivers being located near the warehouse. For now each drivers route is calculated separately, this could be improved to show all driver routes colour coded at the same time with filters to hide routes on request.