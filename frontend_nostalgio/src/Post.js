import React from 'react';

class Post extends React.Component {
    
    
    render() {
      return( 
        <div className='post'>
            <div className='post-user'>{this.props.username}</div>
            <div className='post-text'>{this.props.text}</div>
        </div>);
    }
  }

  export default Post;