import React, { useEffect, useState } from "react";

const WebSocketComponent: React.FC = () => {
    const [message, setMessage] = useState<string>("");
    const [socket, setSocket] = useState<WebSocket | null>(null);

    useEffect(() => {
        const ws = new WebSocket("wss://locahost:7020/ws");
        ws.onopen = () => {
            console.log("Connected to WebSocket");
        };

        ws.onmessage = (event: MessageEvent) => {
            setMessage(event.data); // Handle incoming message
        };

        ws.onclose = () => {
            console.log("Disconnected from WebSocket");
        };

        setSocket(ws);

        return () => {
            ws.close();
        }
    }, []);

    const sendMessage = () => {
        if (socket) {
            socket.send("Hello from React with TypeScript!");
        }
    };

  return (
    <div>
      <p>Message from server: {message}</p>
      <button onClick={sendMessage}>Send Message</button>
    </div>
  );
};

export default WebSocketComponent;
