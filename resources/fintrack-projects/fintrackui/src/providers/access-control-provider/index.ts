import { AccessControlProvider } from "@refinedev/core";
import { keycloak } from "../auth-provider/utils/keycloak";
import { Action, can, Resource } from "./permissions";
import { KeycloakTokenParsed } from "keycloak-js";

export const accessControlProvider: AccessControlProvider = {  
  can: async ({ resource, action }) => {
    const token = keycloak.tokenParsed as KeycloakTokenParsed;
    const userRoles: string[] = token?.realm_access?.roles || [];
    const allowed = can(userRoles, action as Action, resource as Resource);
    return { can: allowed, reason: "You are not allowed to access this resource" };
  },
  options: {
    buttons: {
      enableAccessControl: true,
      hideIfUnauthorized: true,
    },
  }
};
