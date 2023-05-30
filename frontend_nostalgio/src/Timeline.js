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
        fetch(document.location.protocol + '//' + document.location.host+'/api/Post/GetTimeline')
        .then((res) => {
            return res.json;
        }).then( res => {
            this.setState({feed : [
                {
                    postId : 'sauifhagsuifhaiosf',
                    postedAt: '2023-05-25T17:48:21.2124',
                    postedByUsername : 'Cock' ,
                    description : 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.' ,
                    likes : 15,
                    comments : [{
                        username : 'Schwnaz',
                        created : '2023-06-25T17:48:21.2124',
                        text : 'Obama ist cool'
                    }
                    ],
                    image : 'adfsafsaf'

                }
            ]});
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
                id={element.postId}></Post>);
        });

        return( 
        <div className='timeline-wrapper' id='timeline-wrapper'>
            <Hotbar setScreen={this.props.setScreen} />
            <div className='post-wrapper'>
                {posts}
            </div>
        </div>);
    }
  }

  export default Timeline;