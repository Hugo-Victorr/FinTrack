# FinTrack 💰

> Plataforma completa de gestão financeira pessoal e educação financeira

FinTrack é uma aplicação web moderna para rastreamento de despesas, gestão de carteiras, análise financeira e educação financeira. Desenvolvida com arquitetura de microserviços, oferece uma experiência completa para controle e aprendizado sobre finanças pessoais.

![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow)
![License](https://img.shields.io/badge/license-MIT-blue)

## 📋 Índice

- [Características](#-características)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
- [Executando o Projeto](#-executando-o-projeto)
- [Documentação](#-documentação)
- [Roadmap](#-roadmap)
- [Contribuindo](#-contribuindo)
- [Licença](#-licença)

## ✨ Características

### Gestão Financeira
- 📊 **Rastreamento de Despesas**: Registro detalhado de gastos com categorização
- 💳 **Gestão de Carteiras**: Múltiplas carteiras com diferentes tipos e moedas
- 📈 **Dashboard Analítico**: Visualização de KPIs e métricas financeiras
- 🏷️ **Categorização**: Organização de despesas por categorias personalizáveis

### Educação Financeira
- 🎓 **Cursos Online**: Sistema completo de cursos sobre finanças pessoais
- 📚 **Módulos e Lições**: Estrutura hierárquica de conteúdo educacional
- ✅ **Acompanhamento de Progresso**: Tracking de conclusão de cursos e lições
- 🎯 **Planos de Aprendizado**: Caminhos personalizados de estudo

### Infraestrutura
- 🔐 **Autenticação Segura**: Integração com Keycloak e login social (Google)
- 🔒 **Autorização por Roles**: Sistema de permissões (user, manager, admin)
- ☁️ **Cloud-Ready**: Preparado para deploy na AWS com ECS/EKS
- 🐳 **Containerizado**: Docker e Docker Compose para desenvolvimento local

## 🏗️ Arquitetura

O FinTrack utiliza uma arquitetura de microserviços com os seguintes componentes:

```
┌─────────────────┐
│   Frontend      │  React + Refine + TypeScript
│   (fintrackui)  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  API Gateway    │  AWS API Gateway + JWT Authorizer
└────────┬────────┘
         │
    ┌────┴────┬─────────────┬─────────────┐
    ▼         ▼             ▼             ▼
┌─────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐
│Expenses │ │Education │ │Analytics │ │ Identity │
│ Service │ │ Service  │ │ Service  │ │ Keycloak │
└────┬────┘ └────┬─────┘ └────┬─────┘ └────┬─────┘
     │           │             │             │
     └───────────┴─────────────┴─────────────┘
                         │
                         ▼
              ┌──────────────────┐
              │   PostgreSQL     │
              │   (RDS/ECS)      │
              └──────────────────┘
```

### Microserviços Backend

- **FinTrack.Expenses**: Gestão de despesas, carteiras e categorias
- **FinTrack.Education**: Sistema de cursos e progresso educacional
- **FinTrack.Analytics**: Dashboard e análise de KPIs financeiros
- **FinTrack.Database**: Camada de acesso a dados (Entity Framework Core)
- **FinTrack.Model**: Entidades e DTOs compartilhados
- **FinTrack.Infrastructure**: Serviços e utilitários compartilhados

## 🛠️ Tecnologias

### Frontend
- **React 19** - Biblioteca UI
- **TypeScript** - Tipagem estática
- **Refine** - Framework para aplicações B2B/admin
- **Ant Design** - Componentes UI
- **Keycloak.js** - Autenticação
- **Vite** - Build tool
- **React Router** - Roteamento

### Backend
- **.NET 7+** - Framework de aplicação
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados
- **Keycloak** - Gerenciamento de identidade
- **Swagger/OpenAPI** - Documentação de API

### Infraestrutura
- **Docker** - Containerização
- **Docker Compose** - Orquestração local
- **AWS ECS/EKS** - Orquestração em produção
- **AWS RDS** - Banco de dados gerenciado
- **AWS API Gateway** - Gateway de APIs
- **AWS S3** - Armazenamento de objetos
- **AWS Route53 + ACM** - DNS e certificados SSL

## 📁 Estrutura do Projeto

```
fintrack/
├── assets/                          # Recursos estáticos
│   └── logo-neon.png
├── docs/                            # Documentação
│   └── cloud-resources.drawio
├── resources/
│   ├── fintrack-projects/
│   │   ├── fintrackdotnet/          # Backend .NET
│   │   │   ├── FinTrack.Expenses/   # Microserviço de despesas
│   │   │   ├── FinTrack.Education/  # Microserviço educacional
│   │   │   ├── FinTrack.Analytics/  # Microserviço de analytics
│   │   │   ├── FinTrack.Database/   # Camada de dados
│   │   │   ├── FinTrack.Model/      # Entidades e DTOs
│   │   │   ├── FinTrack.Infrastructure/ # Utilitários
│   │   │   └── docker-compose/      # Docker Compose
│   │   ├── fintrackui/              # Frontend React
│   │   │   ├── src/
│   │   │   │   ├── components/      # Componentes React
│   │   │   │   ├── pages/           # Páginas
│   │   │   │   ├── providers/       # Providers (auth, data)
│   │   │   │   └── routes/          # Configuração de rotas
│   │   │   └── package.json
│   │   └── fintrackidentity/        # Configuração Keycloak
│   │       └── docker-compose.yml
│   └── ...
├── LICENSE
└── README.md
```

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **.NET SDK 7+** - [Download](https://dotnet.microsoft.com/download)
- **Node.js 18+** e npm - [Download](https://nodejs.org/)
- **Docker** e **Docker Compose** - [Download](https://www.docker.com/get-started)
- **PostgreSQL 15+** (opcional, pode usar Docker)
- **Git** - [Download](https://git-scm.com/)

## 🚀 Instalação

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/fintrack.git
cd fintrack
```

### 2. Configure o banco de dados

Inicie o PostgreSQL usando Docker Compose:

```bash
cd resources/fintrack-projects/fintrackdotnet/docker-compose
docker-compose up -d postgres
```

### 3. Configure as variáveis de ambiente

Crie arquivos `.env` ou configure as variáveis necessárias:

**Backend** (`FinTrack.*/appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "FintrackDb": "Host=localhost;Port=5433;Database=fintrackDb;Username=fintrack;Password=fintrack"
  }
}
```

**Frontend**: Configure a URL da API e do Keycloak conforme necessário.

## 💻 Executando o Projeto

### Backend (Microserviços)

Para executar cada microserviço:

```bash
# Expenses Service
cd resources/fintrack-projects/fintrackdotnet/FinTrack.Expenses
dotnet run

# Education Service
cd resources/fintrack-projects/fintrackdotnet/FinTrack.Education
dotnet run

# Analytics Service
cd resources/fintrack-projects/fintrackdotnet/FinTrack.Analytics
dotnet run
```

Ou execute todos via Docker Compose (quando configurado).

### Frontend

```bash
cd resources/fintrack-projects/fintrackui
npm install
npm run dev
```

O frontend estará disponível em `http://localhost:5173` (porta padrão do Vite).

### Keycloak (Autenticação)

```bash
cd resources/fintrack-projects/fintrackidentity
docker-compose up -d
```

Acesse o console do Keycloak em `http://localhost:8080`.

## 📚 Documentação

### API Documentation

Cada microserviço expõe documentação Swagger quando executado em modo Development:
- Expenses: `http://localhost:5001/swagger`
- Education: `http://localhost:5002/swagger`
- Analytics: `http://localhost:5003/swagger`

### Entidades Principais

- **Expense**: Despesa financeira
- **Wallet**: Carteira/Conta bancária
- **ExpenseCategory**: Categoria de despesa
- **Course**: Curso de educação financeira
- **CourseModule**: Módulo do curso
- **CourseLesson**: Lição do módulo
- **CourseProgress**: Progresso do usuário no curso


## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor:

1. Faça um Fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👥 Autor

Desenvolvido com 💚 pela equipe FinTrack

---

⭐ Se este projeto foi útil para você, considere dar uma estrela!