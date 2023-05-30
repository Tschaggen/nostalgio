import React from 'react';
import Login from "./Login";
import Timeline from "./Timeline";
import MyProfile from './MyProfile';
import OtherProfile from './OtherProfile';
import './index.css'

class Wrapper extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            login: null,
            screen: 'timeline'  
        };  

    }

    
    setLogin = (loginData) =>  {
        this.setState({login: loginData});
    }

    setScreen = (screenData) =>  {
        this.setState({screen: screenData});
    }
    
    render() {
        if(this.state.login === null) {
            return <Login setLogin={this.setLogin}/>
        }
        else {
            if(this.state.screen === 'timeline') {
                return <Timeline setScreen={this.setScreen}/>;
            }
            else if(this.state.screen === 'myprofile') {
                return <MyProfile setScreen={this.setScreen}/>;
            }
            else if(this.state.screen === 'otherprofile') {
                return <OtherProfile setScreen={this.setScreen}/>;
            }
        }
    }
  }

  export default Wrapper;