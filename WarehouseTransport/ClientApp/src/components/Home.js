import React, { Component } from 'react';

export class Home extends Component {
    render() {
        return (
            <div>
        <h1>Warehouse Prototype</h1>
        <p>Basic delivery calculator between warehouses and destinations:</p>
            <ul>
                <li>Click <b>Route Calculator</b> in menu to view route for each driver.</li>
                <li>Algorithm based on Melbourne bay area. Melbourne has densities west and north and south east of CBD</li>
                <li>Delivery points distributed between both Warehouses based on above note.</li>
                <li>Drivers assigned to warehouse for now on delivery points for each Warehouse</li>
                <li>I removed duplicate positions as part of the algorithm for now as scope needs to refined.  By that I mean it can be handled as multiple deliveries or duplicate bookings and the approach is different.</li>
                <li>At the moment all the work is performed by the one service for each driver -&gt cleaning, sorting, warehouse choosing and so on this could be easily split up but it would depend on the requirements</li>
                <li>Algorithm does not deal to well with remote locations.  This would be an enhancemant by adding a weighting and distributing to driver evenly.  See driver 5 in this case.</li>
            </ul>
        <p>Display and technology:</p>
        <ul>
          <li>React with Google libraries</li>
          <li>Simple web api with routing on the backend.</li>
          <li>As this is a demo for now I haven't used a SQL backend for locations.  This could easily be ported into a entity model with dynamic locations lists depending on business requirements.</li>
        </ul>
        <p>This is demonstration application.  I've made assumptions about drivers being located near the warehouse. For now each drivers route is calculated separately, this could be improved to show all driver routes colour coded at the same time with filters to hide routes on request.</p>
      </div>
    );
  }
}
