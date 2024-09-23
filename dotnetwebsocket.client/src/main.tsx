import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import WebSocketComponent from "./WebSocket.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <WebSocketComponent></WebSocketComponent>
  </StrictMode>
);
