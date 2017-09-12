'use strict';
var port = process.env.PORT || 3000;
var express = require('express')
var app = express()

app.use('/', express.static('src'))
app.listen(port, () => console.log('Listening in port: ' + port))