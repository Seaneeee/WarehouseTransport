import React from "react";
import {
    withGoogleMap,
    GoogleMap,
    withScriptjs,
    Marker,
    DirectionsRenderer
} from "react-google-maps";


class MapDirectionsRenderer extends React.Component {
    state = {
        directions: null,
        error: null,
        places: [],
        loaded: false,
        driver:0
    };

    async getDirections() {
        const { travelMode, driver } = this.props;
        const response = await fetch('api/directions/' + driver);
        const data = await response.json();
        let places = data;
        const waypoints = places.map(p => ({
            location: { lat: p.latitude, lng: p.longitude },
            stopover: true
        }))
        const origin = waypoints.shift().location;
        const destination = waypoints.pop().location;
    
        const directionsService = new window.google.maps.DirectionsService();
        directionsService.route(
            {
                origin: origin,
                destination: destination,
                travelMode: travelMode,
                waypoints: waypoints
            },
            (result, status) => {
                if (status === window.google.maps.DirectionsStatus.OK) {
                    this.setState({
                        directions: result
                    });
                } else {
                    this.setState({ error: result });
                }
            }
        );
        this.setState({ loaded: true, driver: driver });
    }

    componentDidMount() {
    }

    render() {
        let newDriver = this.props.driver;
        let loadedDriver = this.state.loaded;
        if (newDriver !== this.state.driver) {
            loadedDriver = false;
            this.getDirections();
        }

        if (this.state.error) {
            return <h1>{this.state.error}</h1>;
        }
        if (!loadedDriver) {
            return <h1 className="loadingRoute">Loading route</h1>;
        }
        return (this.state.directions && <DirectionsRenderer directions={this.state.directions} />)
    }
}

const Map = withScriptjs(
    withGoogleMap(props => (
        <GoogleMap
            defaultCenter={props.defaultCenter}
            defaultZoom={props.defaultZoom}
        >
            {props.markers.map((marker, index) => {
                const position = { lat: marker.latitude, lng: marker.longitude };
                return <Marker key={index} position={position} />;
            })}
            <MapDirectionsRenderer
                places={props.markers}
                travelMode={window.google.maps.TravelMode.DRIVING}
                driver={props.driver}
            />
        </GoogleMap>
    ))
);

const googleMapsApiKey = "AIzaSyD1uCZ65ceA_IbL-_cGa4ATNola0934TbE";

class Transport extends React.Component {
    constructor(props) {
        super(props);
        this.state = { driver: "1" };
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {
        this.setState({ driver: event.target.value });
    }

    render() {

        const places = [];

        const {
            loadingElement,
            containerElement,
            mapElement,
            defaultCenter,
            defaultZoom
        } = this.props;

        return (
            <div>
                <div className="driverSelection">
                    <span><b>Select driver</b> to view route</span>
                    <select value={this.state.driver} onChange={this.handleChange}>
                        <option value="1">Driver 1</option>
                        <option value="2">Driver 2</option>
                        <option value="3">Driver 3</option>
                        <option value="4">Driver 4</option>
                        <option value="5">Driver 5</option>
                    </select>
                </div>
                <Map
                    googleMapURL={
                        'https://maps.googleapis.com/maps/api/js?key=' +
                        googleMapsApiKey +
                        '&libraries=geometry,drawing,places'
                    }
                    markers={places}
                    driver={this.state.driver}
                    loadingElement={loadingElement || <div style={{ height: "100%" }} />}
                    containerElement={containerElement || <div style={{ height: "80vh" }} />}
                    mapElement={mapElement || <div style={{ height: "100%" }} />}
                    defaultCenter={defaultCenter || { lat: -37.762553, lng: 144.954236 }}
                    defaultZoom={defaultZoom || 11}
                />
            </div>
        );
    }
};

export default Transport;
