import React from 'react';
import './login.css'

class Login extends React.Component {
    
    login(username,password) {

      const params = {
        Username : username,
        Password : password
      }

      const options = {
        method: 'POST',
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify( params ) 
      }

      fetch(document.location.protocol + '//' + document.location.hostname+':5000/api/Auth/Login', options)
        .then((res) => {
          if(res.status == 401 || res.status == 404 || res.status == 400 || res.status == 500) {
            return false;
          }
            return res.json();
        }).then( res => {
          if(res !== false) {
            this.props.setLogin(res.jwtToken);
          }
          else {
            let container = document.querySelector('.login-container');

            let element = document.createElement('div');
            element.innerText = 'Something went wrong during the login procces';
            element.classList.add('error')

            container.prepend(element);
          }
        });
    }

    register(username,password) {

      const params = {
        user : {
          username : username
        },
        password : password
      }

      const options = {
        method: 'POST',
        body: JSON.stringify( params ) 
      }

      fetch(document.location.protocol + '//' + document.location.hostname+':5000/api/Auth/Register', options)
        .then((res) => {
            return res.json();
        }).then( res => {
            res = {
              jwtToken : 1
            }
            this.props.setLogin(res.jwtToken);
        });
    }

    render() {
      return (
      <div className='login-container'>
        
        <div className='login-button-wrapper'>
          <button onClick={()=> {
              let loginBtn = document.getElementById('swap-to-login-form');
              let registerBtn = document.getElementById('swap-to-register-form');
              let registerForm = document.getElementById('register-form');
              let loginForm = document.getElementById('login-form');

              registerBtn.classList.remove('active');
              loginBtn.classList.add('active');
              registerForm.classList.add('none');
              loginForm.classList.remove('none')
          }} id='swap-to-login-form' className='active'>Login</button>

          <button onClick={()=> {
              let registerBtn = document.getElementById('swap-to-register-form');
              let loginBtn = document.getElementById('swap-to-login-form');
              let registerForm = document.getElementById('register-form');
              let loginForm = document.getElementById('login-form');

              loginBtn.classList.remove('active');
              registerBtn.classList.add('active');
              registerForm.classList.remove('none');
              loginForm.classList.add('none')
          }}id='swap-to-register-form'>Register</button>
        </div>

        <form className='login-form' id='login-form' onSubmit={(e) => {
          e.preventDefault();
          this.login(e.target.elements.username.value,e.target.elements.password.value,);
        }}>

          <div className='from-input-row'>
            <label className="username-label form-label" htmlFor="username">Username</label>
            <input className="username-input form-input-text" type="text" id="username" name="username"/>
          </div>

          <div className='from-input-row'>
            <label className="password-label form-label" htmlFor="password">Password</label>
            <input className="password-input form-input-text" type="password" id="password" name="password"/>
          </div>

          <div className='submit-wrapper'>
            <button className='submit' >Login</button>
          </div>

        </form>

        <form className='register-form none' id='register-form' onSubmit={(e) => {
          e.preventDefault()
          this.register(e.target.elements.username.value,e.target.elements.password.value,)
        }}>

          <div className='from-input-row'>
            <label className="username-label form-label" htmlFor="username">Username</label>
            <input className="username-input form-input-text" type="text" id="username" name="username"/>
          </div>

          <div className='from-input-row'>
            <label className="password-label form-label" htmlFor="password">Password</label>
            <input className="password-input form-input-text" type="password" id="password" name="password"/>
          </div>

          <div className='submit-wrapper'>
            <button className='submit'>Register</button>
          </div>

        </form>

      </div>);
    }
  }

  export default Login;