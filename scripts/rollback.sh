#!/bin/bash

# Script para fazer rollback para versão anterior em caso de falha

set -e

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

DEPLOYMENT_DIR="${1:-.}"
BACKUPS_DIR="$DEPLOYMENT_DIR/.backups"

log() { echo -e "${GREEN}[$(date '+%Y-%m-%d %H:%M:%S')]${NC} $1"; }
error() { echo -e "${RED}[ERRO]${NC} $1" >&2; exit 1; }

log "Iniciando rollback..."

if [ ! -d "$BACKUPS_DIR" ]; then
    error "Nenhum backup encontrado em $BACKUPS_DIR"
fi

# Listar backups disponíveis
echo -e "${YELLOW}Backups disponíveis:${NC}"
ls -1t "$BACKUPS_DIR" | nl

read -p "Digite o número do backup para restaurar: " backup_num

BACKUP_FILE=$(ls -1t "$BACKUPS_DIR" | sed -n "${backup_num}p")

if [ -z "$BACKUP_FILE" ]; then
    error "Backup inválido selecionado"
fi

log "Restaurando backup: $BACKUP_FILE"

# Parar containers
log "Parando containers..."
docker-compose down || true

# Restaurar docker-compose.yml
log "Restaurando docker-compose.yml..."
cp "$BACKUPS_DIR/$BACKUP_FILE" "$DEPLOYMENT_DIR/docker-compose.yml"

# Iniciar com versão anterior
log "Iniciando containers com versão anterior..."
docker-compose up -d

log "? Rollback concluído com sucesso!"
docker-compose ps
