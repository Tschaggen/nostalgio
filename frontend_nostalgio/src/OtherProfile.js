import React from 'react';
import Hotbar from './Hotbar';

class OtherProfile extends React.Component {
    
    
    render() {
      return( 
      <div className='profile-wrapper' id='other-profile-wrapper'>
        <Hotbar setScreen={this.props.setScreen} />
        otherprofile
      </div>);
    }
  }

  export default OtherProfile;