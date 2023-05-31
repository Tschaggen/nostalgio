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

    loadFeed = () => {

    
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
                  <Hotbar setScreen={this.props.setScreen} reloadFeed={this.loadFeed} />
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
                reloadFeed={this.loadFeed}></Post>);
        });

        return( 
        <div className='timeline-wrapper' id='timeline-wrapper'>
            <Hotbar setScreen={this.props.setScreen} jwtToken={this.props.jwtToken} reloadFeed={this.loadFeed}/>
            <div id='postForm' className='none'>
                <form onSubmit={(e) => {
                    e.preventDefault();

                    var reader = new FileReader();
                    var token = this.props.jwtToken;
                    var loadf = this.loadFeed;
                    reader.readAsDataURL(e.target.elements.img.files[0]);
                    reader.onload = function () {
                        let img = reader.result;
                        let text = e.target.elements.text.value;

                        console.log(img);

                        const params = {
                            description : text,
                            image: img
                        }
          
                        const options = {
                            method: 'POST',
                            headers: {
                                "Authorization": "Bearer "+token,
                                "Content-Type": "application/json"  
                            },
                            body: JSON.stringify( params ) 
                        }
          
                        fetch(document.location.protocol + '//' + document.location.hostname+':5000/api/Post', options);

                        loadf();
                        loadf();
                    };

                }}>
                    <textarea type='text' name='text' className='make-post-text'></textarea>
                    <div className='wrapper'>
                        <input type='file' name='img'></input>
                        <button className='submit'>post</button>
                    </div>
                </form>
            </div>
            <div className='post-wrapper'>
                {posts}
            </div>
        </div>);
    }
  }

  export default Timeline;