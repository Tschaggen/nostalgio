import React from 'react';

class Comment extends React.Component {
    
    
    render() {
      return( 
        <div className='comment'>
            <div className='comment-user'>
                {this.props.username}
            </div>
            <div className='comment-text'>
                {this.props.text}
            </div>
        </div>);
    }
  }

  export default Comment;