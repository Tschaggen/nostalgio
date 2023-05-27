import React from 'react';
import Hotbar from './Hotbar';
import Post from './Post';

class Timeline extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            feed:null  
        };  

    }

    loadFeed() {
        fetch(document.location.protocol + '//' + document.location.host+'/api/Post/GetTimeline')
        .then((res) => {
            return res.json;
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
            posts.push(<Post username={element.username} text= {element.text}></Post>);
        });

        return( 
        <div className='timeline-wrapper' id='timeline-wrapper'>
            <Hotbar setScreen={this.props.setScreen} />
            {posts}
        </div>);
    }
  }

  export default Timeline;