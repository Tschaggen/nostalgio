import React from 'react';
import Hotbar from './Hotbar';
import Post from './Post';
import './timeline.css'

class Timeline extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            feed: null  
        };  

    }

    loadFeed() {

    
        const options = {
            method: 'GET',
            headers: {
                "Authorization": "Bearer "+this.props.jwtToken,  
                "Content-Type": "application/json",
            },
        };

        fetch(document.location.protocol + '//' + document.location.hostname+':5000/api/Post/GetTimeline',options)
        .then((res) => {
            return res.json();
        }).then( res => {
            this.setState({feed : res});
        });
    }

    componentDidMount() {
        this.loadFeed();
    }
    
    render() {

        if(this.state.feed == null) {
            return( 
                <div className='timeline-wrapper' id='timeline-wrapper'>
                  <Hotbar setScreen={this.props.setScreen} />
                  <div className='timeline-load' id='timeline-load'>Loading</div>
                </div>);
        }

        let posts = [];

        this.state.feed.forEach(element => {
            posts.push(<Post 
                username={element.postedByUsername}
                text={element.description}
                postedAt={element.postedAt}
                likes={element.likes}
                comments={element.comments}
                image={element.image}
                id={element.postId}
                jwtToken={this.props.jwtToken}
                setScreen={this.props.setScreen}></Post>);
        });

        return( 
        <div className='timeline-wrapper' id='timeline-wrapper'>
            <Hotbar setScreen={this.props.setScreen} jwtToken={this.props.jwtToken}/>
            <div className='post-wrapper'>
                {posts}
            </div>
        </div>);
    }
  }

  export default Timeline;