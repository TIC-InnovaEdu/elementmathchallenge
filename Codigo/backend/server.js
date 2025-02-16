const express = require('express');
const keys = require('./config/keys.js');
const app = express();
const bodyParser = require('body-parser');

// Setting up DB
const mongoose = require('mongoose');
mongoose.connect(keys.mongoURI);

app.use(bodyParser.urlencoded({extended:false}));

//Setup database models
require('./model/account');
require('./model/questions');

//Setup the routes
require('./routes/authenticationRoutes')(app);
require('./routes/authRoutes')(app);


app.listen(keys.port,() => {
    console.log("Listening on "+ keys.port);
});