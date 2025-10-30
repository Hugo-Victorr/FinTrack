import Keycloak from "keycloak-js";

export const keycloak = new Keycloak({
  clientId: "frontend-client",
  url: "http://localhost:8080/",
  realm: "fintrack-homol",
});