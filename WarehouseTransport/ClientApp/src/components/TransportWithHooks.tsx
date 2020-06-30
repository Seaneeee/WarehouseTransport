import React from "react";
import {
    withGoogleMap,
    GoogleMap,
    withScriptjs,
    Marker,
    DirectionsRenderer
} from "react-google-maps";

function MapDirectionsRenderer(props) {

    const [state, setState] = React.useState({
        directions: null,
        error: null,
        places: [],
        loaded: false,
        driver: 0
    })

    async function getDirections() {
        const { travelMode, driver } = props;
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
                    setState({
                        ...state,
                        directions: result
                    })

                } else {
                    setState({
                        ...state,
                        error: result
                    })
                }
            }
        );
        setState({
            ...state,
            loaded: true, driver: driver
        })
    }

    let newDriver = props.driver;
    let loadedDriver = state.loaded;
    React.useEffect(() => {
        if (newDriver !== state.driver) {
            getDirections();
        }
    }, [newDriver]);


    if (state.error && state.driver) {
        return <h1>{state.error}</h1>;
    }
    if (!loadedDriver && state.driver!==0) {
        return <h1 className="loadingRoute">Loading route</h1>;
    }
    return (state.directions && <DirectionsRenderer directions={state.directions} />)
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

const TransportHooks = (props) => {
    const [driver, setDriver] = React.useState("1");

    const handleChange = (e) => {
        setDriver(e.target.value)
    }
    const places = [];

    const {
        loadingElement,
        containerElement,
        mapElement,
        defaultCenter,
        defaultZoom
    } = props;

    return (
        <div>
            <div>
                <i>Typescript version using Hooks</i>
            </div>
            <div className="driverSelection">
                <span><b>Select driver</b> to view route</span>
                <select value={driver} onChange={(e) => handleChange(e)}>
                    <option value="1">Driver A</option>
                    <option value="2">Driver B</option>
                    <option value="3">Driver C</option>
                    <option value="4">Driver D</option>
                    <option value="5">Driver E</option>
                </select>
            </div>
            <Map
                googleMapURL={
                    'https://maps.googleapis.com/maps/api/js?key=' +
                    googleMapsApiKey +
                    '&libraries=geometry,drawing,places'
                }
                markers={places}
                driver={driver}
                loadingElement={loadingElement || <div style={{ height: "100%" }} />}
                containerElement={containerElement || <div style={{ height: "80vh" }} />}
                mapElement={mapElement || <div style={{ height: "100%" }} />}
                defaultCenter={defaultCenter || { lat: -37.762553, lng: 144.954236 }}
                defaultZoom={defaultZoom || 11}
            />
        </div>
    );
};

export default TransportHooks;
