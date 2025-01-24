const WebSocket = require('ws');

const server = new WebSocket.Server({ port: 8080 });
console.log("Servidor WebSocket escuchando en ws://localhost:8080");

let clients = [];

server.on('connection', (socket) => {
    console.log("Cliente conectado");

    // Agregar el cliente a la lista
    clients.push(socket);

    // Manejar mensajes recibidos
    socket.on('message', (message) => {
        console.log("Mensaje recibido:", message);

        // Reenviar el mensaje a todos los clientes
        clients.forEach(client => {
            if (client !== socket && client.readyState === WebSocket.OPEN) {
                client.send(message);
            }
        });
    });

    // Manejar desconexión
    socket.on('close', () => {
        console.log("Cliente desconectado");
        clients = clients.filter(client => client !== socket);
    });
});
