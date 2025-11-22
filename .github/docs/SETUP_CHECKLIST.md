# ? Checklist: Setup Completo do Pipeline

## ?? ANTES DE COMEÇAR

- [ ] EC2 disponível e rodando (Ubuntu 22.04 ou Amazon Linux 2)
- [ ] Acesso SSH funcionando (`ssh -i key.pem ec2-user@ip`)
- [ ] GitHub repository acesso com `gh` CLI ou GitHub website
- [ ] Docker Hub conta criada

---

## ?? PASSO 1: Preparar Docker Hub (5 min)

### 1.1 Criar Token
- [ ] Acesse https://hub.docker.com/settings/security
- [ ] Clique em "New Access Token"
- [ ] Nome: `fintrack-pipeline`
- [ ] Copie o token gerado (salve em local seguro!)

### 1.2 Testar Login Localmente
```bash
docker login -u seu-usuario
# Colar token quando pedido
```

---

## ?? PASSO 2: Configurar GitHub Secrets (5 min)

### 2.1 Acesse Settings do Repositório
```
https://github.com/seu-usuario/fintrack/settings/secrets/actions
```

### 2.2 Criar 6 Secrets (Add repository secret)

| Nome | Valor | Exemplo |
|------|-------|---------|
| `DOCKER_USERNAME` | Seu usuário Docker | `seu-usuario` |
| `DOCKER_PASSWORD` | Token do Docker Hub | `dckr_pat_abc123...` |
| `EC2_HOST` | IP da EC2 | `54.123.456.789` |
| `EC2_USER` | User SSH | `ec2-user` |
| `EC2_SSH_KEY` | **Conteúdo completo do .pem** | `-----BEGIN PRIVATE KEY-----...` |
| `EC2_KEY_NAME` | Nome da chave | `fintrack-key` |

#### ?? Como Copiar EC2_SSH_KEY
```bash
# Terminal local
cat ~/Downloads/seu-key.pem

# Copiar TUDO (-----BEGIN até -----END)
# Colar como secret no GitHub
```

### ? Verificar Secrets
```bash
gh secret list
```

---

## ?? PASSO 3: Setup EC2 (15 min)

### 3.1 SSH na EC2
```bash
ssh -i seu-key.pem ec2-user@seu-ec2-ip
```

### 3.2 Executar Script de Setup
```bash
# Copiar e colar todo este bloco:

curl -fsSL https://raw.githubusercontent.com/Hugo-Victorr/FinTrack/main/scripts/setup-ec2.sh | bash

# Se der erro, execute manualmente:
# sudo yum update -y
# sudo yum install -y docker git curl
# sudo usermod -aG docker ec2-user
# sudo systemctl start docker
# sudo systemctl enable docker
# 
# sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" \
#     -o /usr/local/bin/docker-compose
# sudo chmod +x /usr/local/bin/docker-compose
# 
# mkdir -p ~/fintrack-deployment
```

### 3.3 Verificar Instalação
```bash
docker --version      # Docker version 20.10+
docker-compose --version  # Docker Compose version 2.0+
docker run hello-world
```

### 3.4 Fazer Logout e Login
```bash
exit
ssh -i seu-key.pem ec2-user@seu-ec2-ip
```

### 3.5 Criar .env
```bash
cd ~/fintrack-deployment

# Copiar arquivo exemplo
# Ou criar manualmente:
cat > .env << 'EOF'
DOCKER_REGISTRY=docker.io
DOCKER_USERNAME=seu-usuario
DOCKER_PASSWORD=seu-token
DB_NAME=fintrackDb
DB_USER=fintrack
DB_PASSWORD=sua-senha-aqui
ASPNETCORE_ENVIRONMENT=Production
IMAGE_TAG=latest
EOF

# Verificar
cat .env
```

### 3.6 Copiar docker-compose
```bash
# Download do repositório
wget -O docker-compose.yml \
  https://raw.githubusercontent.com/Hugo-Victorr/FinTrack/main/docker-compose-prod.yml

# Ou SCP local
# scp -i seu-key.pem docker-compose-prod.yml ec2-user@seu-ec2-ip:~/fintrack-deployment/docker-compose.yml
```

---

## ?? PASSO 4: Verificar Arquivos no Repository (5 min)

### 4.1 Workflows
- [ ] `.github/workflows/build-and-deploy.yml` existe?
- [ ] `.github/workflows/quick-deploy.yml` existe?

### 4.2 Dockerfiles
- [ ] `dockerfiles/fintrack.dockerfile` existe?
- [ ] `dockerfiles/fintrack-education.dockerfile` existe?
- [ ] `dockerfiles/openfinance.dockerfile` existe?

### 4.3 Docker Compose
- [ ] `docker-compose-prod.yml` existe?
- [ ] `docker-compose-full.yml` existe?

### 4.4 Scripts
- [ ] `scripts/setup-ec2.sh` existe?
- [ ] `scripts/deploy.sh` existe?

### 4.5 Documentação
- [ ] `QUICKSTART.md` existe?
- [ ] `DEPLOYMENT_GUIDE.md` existe?
- [ ] `ARCHITECTURE.md` existe?
- [ ] `COMMANDS.md` existe?

### ? Se falta algum arquivo
```bash
# Git pull dos arquivos criados
git pull origin main

# Ou re-sincronizar
git fetch origin
git reset --hard origin/main
```

---

## ?? PASSO 5: TESTE INICIAL (10 min)

### 5.1 Teste Local (Sem deploy ainda)
```bash
# No seu computador
docker-compose -f docker-compose-full.yml up -d

# Esperar ~30s
sleep 30

# Testar
curl http://localhost:8001/health
curl http://localhost:8002/health
curl http://localhost:8003/health

# Ver logs
docker-compose logs
```

### 5.2 Teste Pipeline Manual
```bash
# Trigger workflow manualmente
gh workflow run build-and-deploy.yml --ref main

# Ver status
gh run list

# Ver logs
gh run view <RUN_ID> --log

# Aguardar completo (~10-15 min para 3 APIs)
```

### 5.3 Verificar Deploy na EC2
```bash
# SSH na EC2
ssh -i seu-key.pem ec2-user@seu-ec2-ip

# Na EC2
cd ~/fintrack-deployment

# Ver status
docker-compose ps

# Ver logs
docker-compose logs -f

# Aguardar health checks (30s+)

# Testar
curl http://localhost:8001/health
curl http://localhost:8002/health
curl http://localhost:8003/health

# ? Se tudo verde: SUCESSO!
```

---

## ?? PASSO 6: Verificação Final (5 min)

### 6.1 APIs Respondendo?
- [ ] `curl http://seu-ec2-ip:8001/health` ? 200 OK
- [ ] `curl http://seu-ec2-ip:8002/health` ? 200 OK
- [ ] `curl http://seu-ec2-ip:8003/health` ? 200 OK

### 6.2 Containers Rodando?
```bash
docker-compose ps
# Resultado esperado: 4 containers UP
# - postgres
# - fintrack-expenses
# - fintrack-education
# - openfinance-api
```

### 6.3 Banco de Dados OK?
```bash
docker logs postgres | tail -20
# Procurar por "ready to accept connections"
```

### 6.4 Logs Sem Erros?
```bash
docker-compose logs --tail 50
# Procurar por ERROR, FATAL, etc.
```

---

## ?? APÓS SETUP: Procedimento Normal

### Para Fazer Deploy
```bash
# Opção 1: Push code e deixar Actions fazer tudo
git push origin main

# Opção 2: Deploy manual rápido
gh workflow run quick-deploy.yml -f environment=main
```

### Para Monitorar
```bash
# Ver run em progresso
gh run list

# Ver logs
gh run view <ID> --log

# Na EC2
ssh -i seu-key.pem ec2-user@seu-ec2-ip
cd ~/fintrack-deployment
docker-compose logs -f
```

### Para Rollback
```bash
ssh -i seu-key.pem ec2-user@seu-ec2-ip
cd ~/fintrack-deployment

# Ver backups
ls -1 .backups/

# Restaurar
cp .backups/docker-compose-backup-XXXXXX.yml docker-compose.yml
docker-compose down
docker-compose up -d
```

---

## ?? TROUBLESHOOTING

### Erro: "docker: command not found"
```bash
# Na EC2, refazer
sudo yum install -y docker
sudo systemctl start docker
exit
ssh -i seu-key.pem ec2-user@seu-ec2-ip
```

### Erro: "Cannot connect to Docker daemon"
```bash
sudo systemctl start docker
sudo usermod -aG docker $USER
exit
ssh -i seu-key.pem ec2-user@seu-ec2-ip
```

### Erro: "Connection refused" no SSH
```bash
# Verificar security group da EC2
# Porta 22 deve estar aberta
# Verificar IP do seu computador

# Testar conectividade
ping seu-ec2-ip  # Deve responder

# Testar SSH
ssh -v -i seu-key.pem ec2-user@seu-ec2-ip
```

### Erro: "Pull access denied"
```bash
# Na EC2
docker login -u seu-usuario
# Colar token
```

### Erro: "Port 8001 already in use"
```bash
# Na EC2
docker-compose down
# Ou mudar porta no docker-compose.yml
```

### Erro: "No such file or directory: docker-compose.yml"
```bash
cd ~/fintrack-deployment
# Ter certeza que arquivo existe
ls -la

# Se não, copiar
wget -O docker-compose.yml https://raw.githubusercontent.com/.../docker-compose-prod.yml
```

---

## ?? SUPORTE

Se alguma coisa der errado:

1. **Verificar logs**: `docker-compose logs`
2. **Reiniciar**: `docker-compose restart`
3. **Nuclear reset**: `docker-compose down -v && docker-compose up -d`
4. **Ver documentação**: Ler `DEPLOYMENT_GUIDE.md`
5. **Executar comandos**: Seguir `COMMANDS.md`

---

## ? Parabéns! 

Seu pipeline está **100% funcional** e pronto para:
- ? Build automático de 3 APIs
- ? Deploy automático na EC2
- ? Health checks e monitoramento
- ? Rollback automático
- ? Logs centralizados

**Próximo passo: `git push` e veja a magic! ??**

---

**Última atualização: 2024**
**Dúvidas? Consulte `QUICKSTART.md`**
