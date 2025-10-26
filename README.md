# FinTrack
Aplicação para gestão de gastos e educação financeira

🩵 FASE 1 — Ambiente local

 Instalar Terraform e AWS CLI.

 Criar arquivo .env central para variáveis locais (DB, Keycloak, etc.).

🔹 Estrutura inicial do repositório
/infrastructure
/backend
/frontend
/docker-compose.yml

 backend/ → microserviços ASP.NET (ex: users, billing, orders).

 frontend/ → app Refine com Keycloak/Google Auth.

 infrastructure/ → scripts Terraform e definições de Docker local.

🧠 FASE 2 — Autenticação local (Keycloak + Google)
🔹 Subir Keycloak localmente

 Adicionar Keycloak no docker-compose.yml:

keycloak:
  image: quay.io/keycloak/keycloak:26.0
  command: start-dev
  environment:
    - KEYCLOAK_ADMIN=admin
    - KEYCLOAK_ADMIN_PASSWORD=admin
  ports:
    - 8080:8080

 Acessar <http://localhost:8080/admin> e criar realm myapp.

🔹 Configurar clients

 Criar frontend-client (public) → <http://localhost:5173/>*.

 Criar gateway-client (confidential).

 Criar roles básicas (user, admin).

🔹 Adicionar Google Auth no Keycloak

 Registrar app no Google Cloud Console
.

 Criar credenciais OAuth2 (tipo Web application).

 Adicionar <http://localhost:8080/>* como redirect URI.

 No Keycloak → Identity Providers → adicionar “Google”.

 Inserir client_id e client_secret do Google.

 Testar login via Google.

⚙️ FASE 3 — Backend (ASP.NET microservices)
🔹 Criar template de microserviço

 Criar projeto base com:

ASP.NET 8 minimal API.

Serilog.

Swagger.

Health checks.

 Adicionar Dockerfile para cada serviço.

🔹 Middleware para claims (sem validar JWT)

 Criar middleware que lê headers x-user-id, x-user-roles, x-user-email.

 Popular ClaimsPrincipal no HttpContext.User.

 Usar [Authorize(Roles="admin")] normalmente.

🔹 Banco de dados local

 Adicionar Postgres no docker-compose.yml.

 Criar migrations e seeds iniciais.

 Conectar microserviços via string de conexão do .env.

🔹 Serviço de usuários

 Implementar integração com API do Keycloak para CRUD de usuários.

 Armazenar metadados locais (tenant, plano, etc.).

 Implementar endpoint /me que retorna dados do usuário autenticado.

🧩 FASE 4 — Frontend (Refine + Keycloak + Google)
🔹 Setup inicial

 Criar app Refine (npm create refine-app).

 Instalar @react-keycloak/web.

 Configurar o provider:

const keycloak = new Keycloak({
  url: "<http://localhost:8080/>",
  realm: "myapp",
  clientId: "frontend-client",
});

 Implementar login/logout via Keycloak (Google já aparece automaticamente).

 Configurar fetch para enviar Authorization: Bearer token.

🔹 Testar localmente

 Subir tudo com docker-compose up.

 Fazer login com Keycloak/Google.

 Testar chamada à API local (localhost:5000) com token válido.

🌐 FASE 5 — Infraestrutura AWS (via Terraform)
🔹 Estrutura Terraform

 Criar infrastructure/main.tf, variables.tf, outputs.tf.

 Definir backend remoto (ex: S3 + DynamoDB).

 Criar módulos Terraform:

/modules
  /vpc
  /ecs
  /rds
  /api_gateway
  /keycloak

🔹 Provisionar recursos

 Criar VPC, sub-redes públicas/privadas.

 Criar RDS Postgres.

 Criar ECS cluster ou EKS cluster.

 Criar Load Balancer interno.

 Criar API Gateway público.

 Criar S3 bucket para assets/logs.

🔹 Configurar Keycloak na AWS

 Implantar Keycloak em ECS (task + service).

 Definir volume persistente para banco (ou usar RDS dedicado).

 Configurar domínio HTTPS (Route53 + ACM).

 Atualizar realm myapp com URLs públicas.

🔹 API Gateway (JWT Authorizer)

 Criar JWT Authorizer usando Keycloak:

issuer: <https://auth.meudominio.com/realms/myapp>

audience: gateway-client

 Mapear claims → headers:

x-user-id: context.authorizer.claims.sub
x-user-email: context.authorizer.claims.email
x-user-roles: context.authorizer.claims.realm_access.roles

 Habilitar “remove all client headers” antes de forward.

🔹 Deploy dos microserviços

 Gerar imagens Docker e enviar para ECR.

 Criar serviços ECS conectados à VPC privada.

 Configurar ALB interno para o Gateway apontar.

🚀 FASE 6 — CI/CD
🔹 GitHub Actions / GitLab CI

 Workflow de build e push para ECR.

 Workflow de terraform apply automatizado (infra).

 Deploy ECS após merge na main.

🧩 FASE 7 — Observabilidade e refinamento

 Adicionar logging centralizado (CloudWatch ou Loki).

 Adicionar health endpoints e monitoramento no Terraform (CloudWatch alarms).

 Implementar tracing (OpenTelemetry).

 Revisar CORS e tempo de expiração dos tokens.

 Habilitar HTTPS everywhere.

✅ FASE 8 — Go-Live

 Registrar domínio (myapp.com).

 Configurar ACM + Route53.

 Atualizar Keycloak com URLs públicas.

 Testar login via Google e fluxo completo (Refine → Gateway → microserviço).

 Fazer smoke tests e métricas iniciais.

📦 Resultado final esperado
[Frontend Refine + Keycloak (com Google Login)]
        ↓
[AWS API Gateway]
  → Valida JWT
  → Injeta claims (user-id, roles)
        ↓
[ECS microserviços ASP.NET]
  → Lê claims do header
  → Aplica regras de negócio
        ↓
[RDS / S3 / outros serviços]
