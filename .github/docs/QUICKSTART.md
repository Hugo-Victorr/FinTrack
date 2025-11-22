# ?? Pipeline CI/CD FinTrack - Guia Rápido

## O que foi criado?

Um **pipeline completo e profissional** que:

? **Build automático** das 3 APIs em paralelo  
? **Push para Docker Hub** apenas em branches principais  
? **Deploy automático na EC2** via SSH  
? **Health checks** para cada serviço  
? **Logs centralizados** e monitoramento  
? **Rollback automático** em caso de falha  

---

## ?? Checklist de Configuração (5 min)

### 1?? Crie um Token Docker Hub
```
https://hub.docker.com/settings/security ? Create Access Token
```

### 2?? Configure GitHub Secrets
```
Settings ? Secrets and variables ? Actions
```

Adicione:
- `DOCKER_USERNAME` = seu usuario docker
- `DOCKER_PASSWORD` = seu token docker
- `EC2_HOST` = IP da sua EC2 (ex: 54.xxx.xxx.xxx)
- `EC2_USER` = ec2-user (ou ubuntu)
- `EC2_SSH_KEY` = conteúdo da sua chave .pem
- `EC2_KEY_NAME` = nome da sua chave

### 3?? Setup da EC2 (10 min)
```bash
# SSH na EC2
ssh -i seu-key.pem ec2-user@seu-ec2-ip

# Colar este comando:
curl -fsSL https://raw.githubusercontent.com/seu-usuario/fintrack/main/scripts/setup-ec2.sh | bash

# Editar credenciais
nano ~/fintrack-deployment/.env

# Fazer logout e login
exit
ssh -i seu-key.pem ec2-user@seu-ec2-ip
```

### 4?? Fazer Push para Trigger o Pipeline
```bash
git push origin main  # ou develop
```

---

## ?? Fluxo de Deploy

### Automático (Recomendado)
```
git push ? GitHub Actions ? Docker Build ? Push Hub ? Deploy EC2
```

### Manual (Se necessário)
```bash
# Na EC2
cd ~/fintrack-deployment
docker-compose pull
docker-compose up -d
```

---

## ?? Arquivos Criados

```
.github/workflows/
??? build-and-deploy.yml    ? Pipeline principal
??? quick-deploy.yml        ? Deploy manual rápido

dockerfiles/
??? fintrack.dockerfile            ? FinTrack.Expenses
??? fintrack-education.dockerfile  ? FinTrack.Education
??? openfinance.dockerfile         ? OpenFinance.API

docker-compose-prod.yml      ? Produção
docker-compose-full.yml      ? Dev + Prod

scripts/
??? setup-ec2.sh   ? Configurar EC2 (rodar 1x)
??? deploy.sh      ? Deploy manual
??? rollback.sh    ? Voltar versão anterior

DEPLOYMENT_GUIDE.md  ? Documentação completa
```

---

## ?? Testar

### 1. Trigger Pipeline Manualmente
```bash
gh workflow run build-and-deploy.yml --ref main
```

### 2. Ver Status
```bash
# GitHub Actions
gh run list
gh run view <id> --log

# EC2
ssh -i seu-key.pem ec2-user@seu-ec2-ip
docker-compose ps
docker-compose logs -f
```

### 3. Testar APIs
```bash
curl http://seu-ec2-ip:8001/health   # Expenses
curl http://seu-ec2-ip:8002/health   # Education
curl http://seu-ec2-ip:8003/health   # OpenFinance
```

---

## ?? Troubleshooting Rápido

| Problema | Solução |
|----------|---------|
| Build falha | Verificar Dockerfile (`docker build -t test .`) |
| Imagem não faz push | Verificar `DOCKER_USERNAME`/`DOCKER_PASSWORD` |
| Deploy não inicia | `ssh ec2-user@ip` ? `docker-compose logs` |
| API não responde | `curl http://localhost:8001/health` |
| Banco não conecta | `docker logs postgres` ? verificar senha |

---

## ?? Próximos Passos (Opcional)

- [ ] Configurar HTTPS com certificado (Let's Encrypt)
- [ ] Usar AWS ECR em vez de Docker Hub
- [ ] Auto-deploy em múltiplas EC2
- [ ] Alerts no Slack/Discord
- [ ] Database migrations automáticas
- [ ] Backup automático do PostgreSQL

---

## ?? Dúvidas?

Consulte:
1. `DEPLOYMENT_GUIDE.md` - Documentação completa
2. `.github/workflows/build-and-deploy.yml` - Ver workflow
3. `docker-compose-full.yml` - Ver configuração

---

**Pronto! Seu pipeline está 100% operacional! ??**
