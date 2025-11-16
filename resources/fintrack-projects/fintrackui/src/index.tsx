import { createRoot } from "react-dom/client";

import { ReactKeycloakProvider } from "@react-keycloak/web";

import App from "./App";
import { keycloak } from "./providers/auth-provider/utils/keycloak";

const container = document.getElementById("root") as HTMLElement;
const root = createRoot(container);

root.render(
  <ReactKeycloakProvider authClient={keycloak}>
    <App />
  </ReactKeycloakProvider>
);
