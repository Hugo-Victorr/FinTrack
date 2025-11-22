#!/bin/bash

# Script para preparar a EC2 para deploy automatizado
# Uso: ./setup-ec2.sh

set -e

YELLOW='\033[1;33m'
GREEN='\033[0;32m'
NC='\033[0m'

echo -e "${YELLOW}================================${NC}"
echo -e "${YELLOW}Setup EC2 para FinTrack Deploy${NC}"
echo -e "${YELLOW}================================${NC}"

# 1. Atualizar sistema
echo -e "${GREEN}1. Atualizando sistema...${NC}"
sudo yum update -y

# 2. Instalar Docker
echo -e "${GREEN}2. Instalando Docker...${NC}"
sudo yum install -y docker
sudo usermod -aG docker ec2-user

# 3. Instalar Docker Compose
echo -e "${GREEN}3. Instalando Docker Compose...${NC}"
sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" \
    -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# 4. Instalar Git (para clonar repo)
echo -e "${GREEN}4. Instalando Git...${NC}"
sudo yum install -y git

# 5. Instalar curl (para health checks)
echo -e "${GREEN}5. Instalando curl...${NC}"
sudo yum install -y curl

# 6. Iniciar Docker
echo -e "${GREEN}6. Iniciando Docker...${NC}"
sudo systemctl start docker
sudo systemctl enable docker

# 7. Criar diretório de deployment
echo -e "${GREEN}7. Criando diretório de deployment...${NC}"
mkdir -p ~/fintrack-deployment
mkdir -p ~/fintrack-deployment/logs

# 8. Criar arquivo .env template
echo -e "${GREEN}8. Criando arquivo .env template...${NC}"
cat > ~/fintrack-deployment/.env << 'EOF'
# Docker Registry Configuration
DOCKER_REGISTRY=docker.io
DOCKER_USERNAME=seu-usuario
DOCKER_PASSWORD=seu-token

# Database Configuration
DB_NAME=fintrackDb
DB_USER=fintrack
DB_PASSWORD=seu-password-seguro
OPENFINANCE_DB=openfinanceDb

# Application Configuration
ASPNETCORE_ENVIRONMENT=Production
IMAGE_TAG=latest

# Image URLs (auto-preenchido)
FINTRACK_EXPENSES_IMAGE=${DOCKER_REGISTRY}/${DOCKER_USERNAME}/fintrack-expenses:${IMAGE_TAG}
FINTRACK_EDUCATION_IMAGE=${DOCKER_REGISTRY}/${DOCKER_USERNAME}/fintrack-education:${IMAGE_TAG}
OPENFINANCE_API_IMAGE=${DOCKER_REGISTRY}/${DOCKER_USERNAME}/openfinance-api:${IMAGE_TAG}
EOF

echo -e "${GREEN}? Setup concluído!${NC}"
echo ""
echo -e "${YELLOW}Próximos passos:${NC}"
echo "1. Editar ~/.env com suas credenciais:"
echo "   nano ~/fintrack-deployment/.env"
echo ""
echo "2. Fazer logout e login para aplicar permissões do Docker:"
echo "   exit"
echo "   ssh -i seu-key.pem ec2-user@seu-ec2-ip"
echo ""
echo "3. Verificar instalação:"
echo "   docker --version"
echo "   docker-compose --version"
echo ""
echo "4. Configurar GitHub Secrets no repositório"
echo ""
