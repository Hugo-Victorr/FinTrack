# ?? Comandos Úteis - FinTrack CI/CD

## ?? GitHub Actions

### Trigger Manual do Pipeline
```bash
# Deploy para main
gh workflow run build-and-deploy.yml --ref main

# Deploy para develop
gh workflow run build-and-deploy.yml --ref develop

# Quick deploy (sem rebuild)
gh workflow run quick-deploy.yml --ref main -f environment=main
```

### Ver Status
```bash
# Listar últimas execuções
gh run list --workflow build-and-deploy.yml

# Ver detalhes de uma execução
gh run view <run-id>

# Ver logs de uma execução
gh run view <run-id> --log

# Cancelar execução
gh run cancel <run-id>

# Reexecutar
gh run rerun <run-id>
```

---

## ?? Docker Local

### Build Local (Teste antes de push)

```bash
# Build FinTrack.Expenses
cd resources/fintrack-projects/fintrackdotnet
docker build -f ../../dockerfiles/fintrack.dockerfile -t fintrack-expenses:local .

# Build FinTrack.Education
docker build -f ../../dockerfiles/fintrack-education.dockerfile -t fintrack-education:local .

# Build OpenFinance
cd ../../../resources/fintrack-projects/openfinancedotnet
docker build -f ../../dockerfiles/openfinance.dockerfile -t openfinance-api:local .

# Listar imagens locais
docker images | grep fintrack
docker images | grep openfinance
```

### Docker Compose Local

```bash
# Build e start tudo
docker-compose -f docker-compose-full.yml up -d

# Parar tudo
docker-compose down

# Remover volumes (limpar dados)
docker-compose down -v

# Ver logs
docker-compose logs -f
docker-compose logs -f fintrack-expenses
docker-compose logs -f fintrack-education
docker-compose logs -f openfinance-api
docker-compose logs -f postgres

# Executar comando em container
docker-compose exec fintrack-expenses /bin/bash
docker-compose exec postgres psql -U fintrack -d fintrackDb

# Reiniciar um serviço
docker-compose restart fintrack-expenses

# Atualizar uma imagem
docker-compose pull fintrack-expenses
docker-compose up -d fintrack-expenses
```

---

## ??? SSH & EC2

### Conectar EC2

```bash
# SSH básico
ssh -i seu-key.pem ec2-user@seu-ec2-ip

# SSH com forward de porta (acessar localmente)
ssh -i seu-key.pem -L 8001:localhost:8001 ec2-user@seu-ec2-ip

# Copiar arquivo para EC2
scp -i seu-key.pem seu-arquivo ec2-user@seu-ec2-ip:~/

# Copiar arquivo da EC2
scp -i seu-key.pem ec2-user@seu-ec2-ip:~/arquivo seu-arquivo
```

### Gerenciar Deployment na EC2

```bash
# Na EC2
cd ~/fintrack-deployment

# Ver status dos containers
docker-compose ps

# Ver logs em tempo real
docker-compose logs -f

# Ver logs específico
docker-compose logs -f fintrack-expenses

# Parar todos
docker-compose down

# Restart todos
docker-compose restart

# Atualizar imagens e restart
docker-compose pull
docker-compose up -d

# Verificar saúde
curl http://localhost:8001/health
curl http://localhost:8002/health
curl http://localhost:8003/health

# Ver uso de recursos
docker stats

# Limpar espaço em disco
docker system prune -a
docker volume prune
```

---

## ?? Docker Hub

### Login e Push Manual

```bash
# Login
docker login -u seu-usuario

# Tag local com seu usuário
docker tag fintrack-expenses:local seu-usuario/fintrack-expenses:v1.0.0

# Push
docker push seu-usuario/fintrack-expenses:v1.0.0

# Logout
docker logout
```

### Gerenciar Tokens

```bash
# Criar token (via web https://hub.docker.com/settings/security)
# Ou via CLI (se tiver docker-cli-plugin-credstorage)
docker token create --read fintrack-token

# Listar tokens (só web por enquanto)
# Deletar tokens (só web por enquanto)
```

---

## ?? Monitoramento

### Health Checks

```bash
# Verificar todas as APIs
for port in 8001 8002 8003; do
  curl -s http://localhost:$port/health && echo " ? Port $port OK" || echo " ? Port $port DOWN"
done

# Teste mais detalhado
curl -v http://localhost:8001/health
curl -i http://localhost:8001/health

# Com timeout
curl --max-time 5 http://localhost:8001/health

# Salvar response em arquivo
curl -o response.json http://localhost:8001/health
```

### Logs e Debugging

```bash
# Ver logs desde início
docker logs fintrack-expenses

# Últimas 100 linhas
docker logs --tail 100 fintrack-expenses

# Com timestamps
docker logs -t fintrack-expenses

# Seguir (tail -f)
docker logs -f fintrack-expenses

# Desde determinado tempo
docker logs --since 10m fintrack-expenses

# Executar shell no container
docker exec -it fintrack-expenses /bin/bash

# Executar comando
docker exec fintrack-expenses dotnet --version

# Ver informações do container
docker inspect fintrack-expenses
docker inspect fintrack-expenses | grep -i health
```

---

## ?? Rollback & Recovery

### Rollback Manual

```bash
# Na EC2

# Ver versões anteriores
ls -1t ~/.backups/

# Copiar backup anterior
cp ~/.backups/docker-compose-backup-20240115-143022.yml docker-compose.yml

# Restart com versão anterior
docker-compose down
docker-compose up -d

# Verificar
docker-compose ps
```

### Via GitHub Actions

```bash
# Usar quick-deploy para versão anterior
gh workflow run quick-deploy.yml -f environment=develop

# Ver e selecionar qual backup restaurar
ssh -i seu-key.pem ec2-user@seu-ec2-ip
cd ~/fintrack-deployment
./rollback.sh
```

---

## ?? Troubleshooting

### Build falha

```bash
# Testar build localmente
docker build -f dockerfiles/fintrack.dockerfile -t test:local . --progress=plain

# Ver erro completo
docker build -f dockerfiles/fintrack.dockerfile -t test:local . 2>&1 | tail -50

# Debug interativo
docker build -f dockerfiles/fintrack.dockerfile -t test:local --target build .
docker run -it test:local /bin/bash
```

### API não responde

```bash
# Verificar se container está rodando
docker ps | grep fintrack

# Ver logs
docker logs fintrack-expenses

# Verificar porta
netstat -tlnp | grep 8001
ss -tlnp | grep 8001

# Testar conexão
curl -v http://localhost:8001/health

# Ver processo dentro do container
docker top fintrack-expenses

# Restart container
docker restart fintrack-expenses
```

### Banco de dados

```bash
# Conectar ao PostgreSQL
docker exec -it postgres psql -U fintrack -d fintrackDb

# Listar bancos
\l

# Verificar conexão
SELECT 1;

# Ver logs do postgres
docker logs postgres

# Testar conexão de fora
docker exec -it postgres pg_isready -U fintrack

# Resetar dados
docker-compose down -v
docker-compose up -d postgres
```

### Network/Conectividade

```bash
# Verificar rede Docker
docker network ls
docker network inspect docker_default

# Testar conectividade entre containers
docker exec fintrack-expenses ping postgres

# Ver IPs dos containers
docker inspect -f '{{.Name}} {{.NetworkSettings.IPAddress}}' $(docker ps -q)

# Testar DNS
docker exec fintrack-expenses nslookup postgres
```

---

## ?? Performance

### Monitorar Recursos

```bash
# CPU, memória em tempo real
docker stats

# Histórico (com --no-stream)
docker stats --no-stream

# Específico
docker stats fintrack-expenses

# Ver imagem size
docker images --filter reference=fintrack-expenses

# Ver uso de disco
docker system df

# Ver layers da imagem
docker history fintrack-expenses
```

### Otimizar

```bash
# Remover imagens não usadas
docker image prune

# Remover containers parados
docker container prune

# Remover volumes órfãos
docker volume prune

# Limpeza completa (cuidado!)
docker system prune -a

# Build com cache
docker build --build-arg BUILDKIT_INLINE_CACHE=1 .
```

---

## ?? Git & CI/CD

### Branches

```bash
# Ver branches locais
git branch

# Ver todos (local + remote)
git branch -a

# Criar e mudar para nova branch
git checkout -b feature/nova-feature

# Commit e push
git add .
git commit -m "feat: nova feature"
git push origin feature/nova-feature

# Criar PR e aguardar Actions
# Após PR mergeado, Actions faz deploy automaticamente
```

### Workflow

```bash
# Estrutura recomendada

# 1. Feature branch
git checkout -b feature/minha-feature

# 2. Fazer alterações

# 3. Testar localmente
docker-compose -f docker-compose-full.yml up

# 4. Commit
git add .
git commit -m "feat: descrição"

# 5. Push
git push origin feature/minha-feature

# 6. GitHub: criar Pull Request

# 7. Actions roda build (sem deploy)

# 8. Code review

# 9. Merge para main

# 10. Actions roda build + deploy

# 11. Monitorar logs
gh run list --workflow build-and-deploy.yml
gh run view <id> --log
```

---

## ?? Checklist Pré-Deploy

```bash
# Local
[ ] git pull origin main
[ ] docker-compose build
[ ] docker-compose up -d
[ ] curl http://localhost:8001/health
[ ] Testes locais OK

# Commit
[ ] git add .
[ ] git commit -m "descrição"
[ ] git push origin feature-branch

# GitHub
[ ] Criar PR
[ ] Actions passa (verde)
[ ] Code review aprovado
[ ] Merge para main

# EC2
[ ] Monitorar deploy
[ ] Verificar containers
[ ] Health checks passando
[ ] Logs sem erro
```

---

**Sempre tenha em mãos! ??**
