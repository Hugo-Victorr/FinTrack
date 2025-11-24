import { keycloak } from "../auth-provider/utils/keycloak";

export type Resource = "dashboard" | "course" | "coursecategory" | "expensecategory" | "expense" | "wallet";
export type Action = "list" | "show" | "edit" | "delete" | "create";

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
  dashboard: {
    list: ["user"],
  },
  course: {
    list: ["user", "admin", "manager"],
    show: ["user","admin", "manager"],
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
  expensecategory: {
    list: ["user"],
    create: ["user"],
    edit: ["user"],
    delete: ["user"],
  },
  expense: {
    list: ["user"],
    create: ["user"],
  },
  wallet: {
    list: ["user"],
    create: ["user"],
    edit: ["user"],
    delete: ["user"],
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
