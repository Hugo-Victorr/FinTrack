#!/bin/bash

# Script de Deploy das APIs FinTrack para EC2
# Uso: ./deploy.sh [main|develop|staging]

set -e

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configurações
ENVIRONMENT="${1:-main}"
DOCKER_REGISTRY="${DOCKER_REGISTRY:-docker.io}"
DOCKER_USERNAME="${DOCKER_USERNAME:-seu-usuario}"
DEPLOYMENT_DIR="/home/ec2-user/fintrack-deployment"
LOG_FILE="${DEPLOYMENT_DIR}/deploy.log"

# Validação
if [[ ! "$ENVIRONMENT" =~ ^(main|develop|staging)$ ]]; then
    echo -e "${RED}[ERRO] Ambiente inválido. Use: main, develop ou staging${NC}"
    exit 1
fi

log() {
    echo -e "${GREEN}[$(date '+%Y-%m-%d %H:%M:%S')]${NC} $1" | tee -a "$LOG_FILE"
}

error() {
    echo -e "${RED}[ERRO]${NC} $1" | tee -a "$LOG_FILE"
    exit 1
}

warning() {
    echo -e "${YELLOW}[AVISO]${NC} $1" | tee -a "$LOG_FILE"
}

log "================================"
log "Iniciando deploy para: $ENVIRONMENT"
log "================================"

# Criar diretório de deployment
mkdir -p "$DEPLOYMENT_DIR"
cd "$DEPLOYMENT_DIR"

log "1. Definindo tags de imagem..."
IMAGE_TAG=$(date +%Y%m%d-%H%M%S)
IMAGES=(
    "fintrack-expenses:${IMAGE_TAG}"
    "fintrack-education:${IMAGE_TAG}"
    "openfinance-api:${IMAGE_TAG}"
)

log "2. Fazendo login no Docker Hub..."
echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin 2>/dev/null || \
    error "Falha ao fazer login no Docker Hub"

log "3. Fazendo pull das imagens Docker..."
for image in "${IMAGES[@]}"; do
    log "   - Puxando $DOCKER_REGISTRY/$DOCKER_USERNAME/$image"
    docker pull "$DOCKER_REGISTRY/$DOCKER_USERNAME/$image" || \
        error "Falha ao fazer pull de $image"
done

log "4. Parando containers antigos..."
docker-compose down 2>/dev/null || true

log "5. Iniciando novos containers..."
export DOCKER_REGISTRY
export DOCKER_USERNAME
export IMAGE_TAG
export ENVIRONMENT
docker-compose up -d || error "Falha ao iniciar containers"

log "6. Aguardando containers ficarem healthy..."
sleep 10

log "7. Verificando status dos containers..."
docker-compose ps

log "8. Limpando recursos não utilizados..."
docker system prune -f --filter "until=24h" > /dev/null 2>&1 || true

log "9. Fazendo logout do Docker..."
docker logout > /dev/null 2>&1 || true

log "================================"
log "? Deploy concluído com sucesso!"
log "================================"
log "Imagens implantadas:"
for image in "${IMAGES[@]}"; do
    log "  - $image"
done
log ""
log "Verificar logs: tail -f $LOG_FILE"
log "Verificar containers: docker-compose ps"
log "Verificar saúde: curl http://localhost:8001/health"
