var app = require('express')();
var http = require('http');
var socketIo = require('socket.io');
const bodyParser = require('body-parser');

var server = http.createServer(app);
var io = socketIo.listen(server);
// app.use(bodyParser.json({ limit: '4000mb' }));
// app.use(bodyParser.urlencoded({
//   extended: true,
//   limit: '4000mb'
// }))
app.get('/', function (req, res) {
  res.sendFile(__dirname + '/index.html');
});

var userId = 0;
io.on('connection', function (socket) {
  socket.userId = userId++;
  console.log('a user connected, user id: ' + socket.userId);

  socket.on('mess', function (msg) {
    console.log('message from user#' + socket.userId + ": " + msg);
    io.emit('mess', {
      id: socket.userId,
      msg: msg
    });
  });
});
var PORT = 3001;
server.listen(PORT, function () {
  console.log('listening on *: ' + PORT);
});