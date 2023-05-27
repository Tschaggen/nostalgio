import React from 'react';
import './login.css'

class Login extends React.Component {
    
    login(username,password) {

      const params = {
        username : username,
        password : password
      }

      const options = {
        method: 'POST',
        body: JSON.stringify( params ) 
      }


      fetch(document.location.protocol + '//' + document.location.host+'/api/Auth/Login', options)
        .then((res) => {
            return res.json;
        }).then( res => {
            this.props.setLogin(res.jwtToken);
        });
    }

    register(username,password) {

      const params = {
        username : username,
        password : password
      }

      const options = {
        method: 'POST',
        body: JSON.stringify( params ) 
      }

      fetch(document.location.protocol + '//' + document.location.host+'/api/Auth/Register', options)
        .then((res) => {
            return res.json;
        }).then( res => {
            this.props.setLogin(res.jwtToken);
        });
    }

    render() {
      return (
      <div className='login-container'>
        <button className='swap-to-login-form'>Login</button>
        <button className='swap-to-register-form'>Register</button>

        <form className='login-form' id='login-form' onSubmit={(e) => {
          e.preventDefault();
          this.login(e.target.elements.username.value,e.target.elements.password.value,);
        }}>

          <label htmlFor="username">First name:</label>
          <input type="text" id="username" name="username"/>
          <label htmlFor="password">Last name:</label>
          <input type="password" id="password" name="password"/>

          <button className='submit' >Login</button>

        </form>

        <form className='register-form' id='register-form' onSubmit={(e) => {
          e.preventDefault()
          this.register(e.target.elements.username.value,e.target.elements.password.value,)
        }}>

          <label htmlFor="username">First name:</label>
          <input type="text" id="username" name="username"/>
          <label htmlFor="password">Last name:</label>
          <input type="password" id="password" name="password"/>

          <button className='submit'>Register</button>

        </form>

      </div>);
    }
  }

  export default Login;