import React from 'react';
import Hotbar from './Hotbar';

class MyProfile extends React.Component {
    
    
    render() {
      return( 
      <div className='profile-wrapper' id='my-profile-wrapper'>
        <Hotbar setScreen={this.props.setScreen} />
        myprofile
      </div>);
    }
  }

  export default MyProfile;