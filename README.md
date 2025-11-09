* [ ] FASE 1 — Ambiente local

  * [ ] Instalar Terraform e AWS CLI
  * [ ] Criar arquivo .env central
  * [x] Criar estrutura inicial do repositório
  * [ ] Configurar docker-compose.yml

* [ ] FASE 2 — Autenticação local (Keycloak + Google)

  * [ ] Adicionar Keycloak no docker-compose.yml
  * [ ] Criar realm `myapp`
  * [ ] Criar clients `frontend-client` e `gateway-client`
  * [ ] Criar roles básicas (user, admin)
  * [ ] Registrar app no Google Cloud Console
  * [ ] Criar credenciais OAuth2
  * [ ] Adicionar redirect URI no Keycloak
  * [ ] Adicionar Identity Provider Google
  * [ ] Testar login via Google

* [ ] FASE 3 — Backend (ASP.NET microservices)

  * [ ] Criar template de microserviço
  * [ ] Adicionar Serilog, Swagger e Health checks
  * [ ] Criar Dockerfile para cada serviço
  * [ ] Criar middleware para claims
  * [ ] Adicionar Postgres no docker-compose.yml
  * [ ] Criar migrations e seeds
  * [ ] Conectar microserviços ao banco
  * [ ] Implementar serviço de usuários
  * [ ] Integrar com API do Keycloak
  * [ ] Implementar endpoint `/me`

* [ ] FASE 4 — Frontend (Refine + Keycloak + Google)

  * [ ] Criar app Refine
  * [ ] Instalar @react-keycloak/web
  * [ ] Configurar provider Keycloak
  * [ ] Implementar login/logout
  * [ ] Configurar fetch com Authorization header
  * [ ] Testar integração com backend

* [ ] FASE 5 — Infraestrutura AWS (via Terraform)

  * [ ] Criar arquivos main.tf, variables.tf, outputs.tf
  * [ ] Definir backend remoto (S3 + DynamoDB)
  * [ ] Criar módulos Terraform (vpc, ecs, rds, api_gateway, keycloak)
  * [ ] Provisionar VPC e sub-redes
  * [ ] Criar RDS Postgres
  * [ ] Criar ECS ou EKS cluster
  * [ ] Criar Load Balancer interno
  * [ ] Criar API Gateway público
  * [ ] Criar S3 buckets
  * [ ] Implantar Keycloak em ECS
  * [ ] Configurar domínio HTTPS (Route53 + ACM)
  * [ ] Atualizar realm myapp
  * [ ] Criar JWT Authorizer no API Gateway
  * [ ] Mapear claims para headers
  * [ ] Deploy dos microserviços no ECS

* [ ] FASE 6 — CI/CD

  * [ ] Configurar GitHub Actions / GitLab CI
  * [ ] Criar workflow de build e push para ECR
  * [ ] Criar workflow de terraform apply automatizado
  * [ ] Configurar deploy automático no ECS

* [ ] FASE 7 — Observabilidade e refinamento

  * [ ] Adicionar logging centralizado
  * [ ] Adicionar health endpoints e monitoramento
  * [ ] Implementar tracing com OpenTelemetry
  * [ ] Revisar CORS e expiração dos tokens
  * [ ] Habilitar HTTPS everywhere

* [ ] FASE 8 — Go-Live

  * [ ] Registrar domínio (myapp.com)
  * [ ] Configurar ACM + Route53
  * [ ] Atualizar Keycloak com URLs públicas
  * [ ] Testar login via Google
  * [ ] Testar fluxo completo (Refine → Gateway → microserviço)
  * [ ] Fazer smoke tests e métricas iniciais
