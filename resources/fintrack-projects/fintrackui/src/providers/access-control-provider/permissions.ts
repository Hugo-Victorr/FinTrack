import { keycloak } from "../auth-provider/utils/keycloak";

export type Resource = "course" | "coursecategory";
export type Action = "list" | "edit" | "delete" | "create";

export type Role = "admin" | "manager" | "user";

type ResourcePermissionMap = {
  [R in Resource]: Partial<Record<Action, Role[]>>;
} & {
  "*": Partial<Record<Action | "*", Role[]>>;
};

export const permissionRules: ResourcePermissionMap = {
  "*": {
    "*": ["admin"], // global rule: admin can do everything
  },
  course: {
    create: ["admin", "manager"],
    edit: ["admin", "manager"],
    delete: ["admin", "manager"],
  },
  coursecategory: {
    list: ["admin", "manager"],
    create: ["admin", "manager"],
    edit: ["admin", "manager"],
    delete: ["admin", "manager"],
  },
};

export function can(
  userRoles: string[],
  action: Action,
  resource: Resource
): boolean {
  const globalRoles = permissionRules["*"]["*"];
  if (globalRoles?.some((r) => userRoles.includes(r))) return true;

  const allowedRoles =
    permissionRules[resource]?.[action] ||
    permissionRules["*"][action];

  // If no rule defined → allow
  if (!allowedRoles) return true;

  return allowedRoles.some((r) => userRoles.includes(r));
}

export function getCurrentUserRoles(): string[] | undefined {
  return keycloak.tokenParsed?.realm_access?.roles;
}
