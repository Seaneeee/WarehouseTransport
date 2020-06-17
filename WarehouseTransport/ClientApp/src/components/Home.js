import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;
    //console.log(process.env.PUBLIC_URL);
    render() {
    return (
      <div>
        <h1>Ninja Delivery Notes</h1>
        <p>Basic delivery calculator between warehouses and destinations:</p>
            <ul>
                <li>Click <b>Route Calculator</b> in menu to view route for each driver.</li>
                <li>Algorithm based on Melbourne bay area. Melbourne has densities west and north and south east of CBD</li>
                <li>Delivery points distributed between both Warehouses based on above note.</li>
                <li>Drivers assigned to warehouse for now on delivery points for each Warehouse</li>
                <li>Travelling salesman algorithm for routes to give a rough optimisation.  This could certainly be improved by running distances via a road calulator.</li>
            </ul>
        <p>Display and technology:</p>
        <ul>
          <li>React with Gooogle libraries</li>
          <li>Simple web api with routing on the backend.</li>
          <li>As this is a demo for now I haven't used a SQL backend for locations.  This could easily be ported into a entity model with dynamic locations lists depending on business requirements.</li>
        </ul>
        <p>This is demonstration application.  I've made assumptions about drivers being located near the warehouse. For now each drivers route is calculated separately, this could be improved to show all driver routes colour coded at the same time with filters to hide routes on request.</p>
      </div>
    );
  }
}
