import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import Transport from "./components/Transport";
import TransportHooks from "./components/TransportWithHooks";

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/transportHooks' component={TransportHooks} />
        <Route path='/transport' component={Transport} />
      </Layout>
    );
  }
}
