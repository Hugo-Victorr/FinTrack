# FinTrack

## TODO

* [ ] FASE 1 — Ambiente local

  * [ ] Instalar Terraform e AWS CLI
  * [ ] Criar arquivo .env central
  * [x] Criar estrutura inicial do repositório
  * [x] Configurar docker-compose.yml

* [ ] FASE 2 — Autenticação (Keycloak + Google)

  * [x] Adicionar Keycloak no docker-compose.yml
  * [x] Criar realm `fintrack`
  * [x] Criar clients `frontend-client` e `gateway-client`
  * [ ] Criar roles básicas (user, manager, admin)
  * [ ] Adicionar theme
  * [ ] Integração com e-mails (AWS SES)
  * [ ] Configurar event listener (sincronia usuários com aplicação)
  * [ ] Adicionar Identity Provider Google
  * [ ] Exportar configuração

* [ ] FASE 3 — Backend (ASP.NET microservices)

  * [x] Criar template de microserviço
  * [ ] Controle de acesso com [Authorize]
  * [ ] Adicionar Serilog, Swagger (+docs) e Health checks
  * [ ] Criar Dockerfile para cada serviço
  * [x] Criar middleware para claims
  * [x] Adicionar Postgres no docker-compose.yml
  * [x] Criar migrations e seeds
  * [x] Conectar microserviços ao banco
  * [ ] Integrar com API do Keycloak
  * [ ] Implementar endpoint `/me`
  * [ ] Feature: gestão de usuários
  * [ ] Feature: cursos + progresso
  * [ ] Feature: movimentações financeiras
  * [ ] Feature: cadastro de objetivos
  * [ ] Feature: integração com mock api Open Finance (scraping job)
  * [ ] Feature: exportação de dados/relatórios
  * [ ] Feature: central de ações (?)
  * [ ] Feature: integração com investimentos (scraping job ações) + dashboard
  * [ ] Feature: notificações com SQS + Telegram / E-mail

* [ ] FASE 4 — Frontend (Refine + Keycloak + Google)

  * [x] Criar app Refine
  * [x] Instalar @react-keycloak/web
  * [x] Configurar provider Keycloak
  * [x] Implementar login/logout
  * [x] Configurar fetch com Authorization header
  * [x] Testar integração com backend
  * [ ] Criar dashboard
  * [ ] Implementar tela de preferências

* [ ] FASE 5 — Infraestrutura AWS (via Terraform)

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
  * [ ] Configurar deploy automático no ECS

* [ ] FASE 7 — Observabilidade e refinamento

  * [ ] Adicionar logging centralizado
  * [ ] Adicionar health endpoints e monitoramento
  * [ ] Implementar tracing com OpenTelemetry
  * [ ] Revisar CORS e expiração dos tokens
  * [ ] Habilitar HTTPS everywhere
  * [ ] Habilitar criptografia everywhere

* [ ] FASE 8 — Go-Live

  * [ ] Registrar domínio (myapp.com)
  * [ ] Configurar ACM + Route53
  * [ ] Atualizar Keycloak com URLs públicas
  * [ ] Testar login via Google
  * [ ] Testar fluxo completo (Refine → Gateway → microserviço)
  * [ ] Fazer smoke tests e métricas iniciais

* [ ] FASE 9 - Integração Langchain

  * [ ] Implementar Q&A
  * [ ] Implementar ações via REST API
