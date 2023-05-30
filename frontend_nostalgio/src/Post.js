import React from 'react';
import './post.css';
import Comment from './Comment';

class Post extends React.Component {
    
    
    render() {

      let comments = [];

      this.props.comments.forEach(e => {
        comments.push(<Comment text={e.text} created={e.created} username={e.username}></Comment>)
      });

      return( 
        <div className='post' id={'post-'+this.props.id}>

            <div className='post-user-wrapper'>
              <div className='post-user'>{this.props.username}</div>
            </div>

            <div className='post-text'>{this.props.text}</div>

            <div className='post-interaction-wrapper'>
              
              <form className='none'
              onSubmit={(e) => {
                e.preventDefault();
                let text = e.target.elements.comment.value;

                const params = {
                  text : text,
                  postId : this.props.id
                }
          
                const options = {
                  method: 'POST',
                  body: JSON.stringify( params ) 
                }
          
                fetch(document.location.protocol + '//' + document.location.hostname+':5000/api/Post/Comment', options);
              }}>
                <input name="comment" type='text' className='from-write'></input>
                <button className='submit'>Send</button>
              </form>

              <button className='post-btn post-write-btn'
              onClick={(e) => {
                let commentWrapper = document.querySelector('#post-'+this.props.id+' form');
                if(commentWrapper.classList.contains('none')) {
                  commentWrapper.classList.remove('none');                    
                }
                else {
                  commentWrapper.classList.add('none');
                }
              }}>
                <svg xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="M180-180h44l443-443-44-44-443 443v44Zm614-486L666-794l42-42q17-17 42-17t42 17l44 44q17 17 17 42t-17 42l-42 42Zm-42 42L248-120H120v-128l504-504 128 128Zm-107-21-22-22 44 44-22-22Z"/></svg>
              </button>

              <button className='post-btn post-comment-btn'
                onClick={(e) => {
                  let commentWrapper = document.querySelector('#post-'+this.props.id+' .comment-wrapper');
                  if(commentWrapper.classList.contains('none')) {
                    commentWrapper.classList.remove('none');                    
                  }
                  else {
                    commentWrapper.classList.add('none');
                  }
                }}>
              <svg xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="M240-399h313v-60H240v60Zm0-130h480v-60H240v60Zm0-130h480v-60H240v60ZM80-80v-740q0-24 18-42t42-18h680q24 0 42 18t18 42v520q0 24-18 42t-42 18H240L80-80Zm134-220h606v-520H140v600l74-80Zm-74 0v-520 520Z"/></svg>
              </button>

              <button className='post-btn post-like-btn'
              onClick={(e) => {

                const params = {
                  postId : this.props.id
                }
          
                const options = {
                  method: 'POST',
                  body: JSON.stringify( params ) 
                } 


                fetch(document.location.protocol + '//' + document.location.hostname+':5000/api/Post/Like',options)

              }}>
                <svg xmlns="http://www.w3.org/2000/svg" height="48" viewBox="0 -960 960 960" width="48"><path d="m480-121-41-37q-105.768-97.121-174.884-167.561Q195-396 154-451.5T96.5-552Q80-597 80-643q0-90.155 60.5-150.577Q201-854 290-854q57 0 105.5 27t84.5 78q42-54 89-79.5T670-854q89 0 149.5 60.423Q880-733.155 880-643q0 46-16.5 91T806-451.5Q765-396 695.884-325.561 626.768-255.121 521-158l-41 37Zm0-79q101.236-92.995 166.618-159.498Q712-426 750.5-476t54-89.135q15.5-39.136 15.5-77.72Q820-709 778-751.5T670.225-794q-51.524 0-95.375 31.5Q531-731 504-674h-49q-26-56-69.85-88-43.851-32-95.375-32Q224-794 182-751.5t-42 108.816Q140-604 155.5-564.5t54 90Q248-424 314-358t166 158Zm0-297Z"/></svg>
              </button>

            </div>

            <div className='comment-wrapper none'>

              {comments}

            </div>

        </div>);
    }
  }

  export default Post;