import React from 'react';
import './hotbar.css';

class Hotbar extends React.Component {
    
    
    render() {
      return( 
        <div className='hotbar' id='hotbar'>
            <div className='logo'>Nostalgio</div>

            <form className='hotbar-add-form' onSubmit={(e=> {
              e.preventDefault();

              const options = {
                method: 'POST',
                headers: {
                  "Authorization": "Bearer "+this.props.jwtToken,
                  "Content-Type": "application/json"  
                },
                body: JSON.stringify( e.target.elements.user.value ) 
              } 


              fetch(document.location.protocol + '//' + document.location.hostname+':5000/api/Follow',options);

              this.props.reloadFeed();
              this.props.reloadFeed();

            })}>
              <input type='text' name='user' className='hotbar-add-text'></input>
              <button className='submit'>Add</button>
            </form>

            <button onClick={() => {this.props.setScreen('timeline'); return false;}} className='timeline-button'>
            <svg xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="M180-120q-24 0-42-18t-18-42v-600q0-24 18-42t42-18h462l198 198v462q0 24-18 42t-42 18H180Zm0-60h600v-428.571H609V-780H180v600Zm99-111h402v-60H279v60Zm0-318h201v-60H279v60Zm0 159h402v-60H279v60Zm-99-330v171.429V-780v600-600Z"/></svg>
            </button>
            <button onClick={() => {
                let postform = document.getElementById('postForm');

                if(postform != null ) {
                  if(postform.classList.contains('none')) {
                    postform.classList.remove('none');
                  }
                  else {
                    postform.classList.add('none');
                  }
                }
              }} className='timeline-button'>
            <svg xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="M450-200v-250H200v-60h250v-250h60v250h250v60H510v250h-60Z"/></svg>
            </button>
        </div>);
    }
  }

  export default Hotbar;