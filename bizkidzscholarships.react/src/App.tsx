import React, { useState, Component, } from 'react'
import { BrowserRouter as Router, Route  } from 'react-router-dom'
import './App.css'

class App extends Component {
    constructor(props: any) {
        super(props);

        this.state = {}
    }

    render() {
        return (
        <>
            <Router />
        </>);
    }

}

export default App
