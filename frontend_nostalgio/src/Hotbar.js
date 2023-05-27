import React from 'react';

class Hotbar extends React.Component {
    
    
    render() {
      return( 
        <div className='hotbar' id='hotbar'>
            <div className='logo'>Nostalgio</div>
            <a href='#' onClick={() => {this.props.setScreen('myprofile'); return false;}} className='my-profile-button'>MyProfile</a>
            <a href='#' onClick={() => {this.props.setScreen('timeline'); return false;}} className='my-profile-button'>Timeline</a>
        </div>);
    }
  }

  export default Hotbar;