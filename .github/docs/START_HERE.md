# ?? COMECE AQUI - Pipeline FinTrack em 30 min

**Você tem um novo pipeline CI/CD totalmente pronto. Vamos colocá-lo funcionando hoje!**

---

## ?? Tempo Total: 30 minutos

- ?? 5 min: Configurar GitHub Secrets
- ?? 10 min: Setup EC2  
- ?? 10 min: Primeiro deploy automático
- ?? 5 min: Validar que está funcionando

---

## ?? AGORA: Configurar GitHub Secrets (5 min)

### 1. Vá para seu repositório GitHub
```
https://github.com/seu-usuario/fintrack
```

### 2. Acesse Settings ? Secrets
```
Settings ? Secrets and variables ? Actions
```

### 3. Crie 6 Secrets

**Secret 1: DOCKER_USERNAME**
```
Value: seu-usuario-docker
Exemplo: hugo-victorr
```

**Secret 2: DOCKER_PASSWORD**
```
Vá para https://hub.docker.com/settings/security
Create New Token
Copie o token completo (ex: dckr_pat_abc123...)
```

**Secret 3: EC2_HOST**
```
Value: IP da sua EC2
Exemplo: 54.123.456.789
```

**Secret 4: EC2_USER**
```
Value: ec2-user (ou ubuntu se Ubuntu AMI)
```

**Secret 5: EC2_SSH_KEY**
```
Abra seu arquivo .pem:
cat ~/Downloads/sua-chave.pem

Copie TUDO (de -----BEGIN até -----END)
Cole como valor do secret
```

**Secret 6: EC2_KEY_NAME**
```
Value: nome da sua chave (sem .pem)
Exemplo: fintrack-key
```

? **Pronto! Secrets configurados!**

---

## ?? AGORA: Setup EC2 (10 min)

### 1. SSH na EC2
```bash
ssh -i ~/Downloads/sua-chave.pem ec2-user@seu-ec2-ip
```

### 2. Executar Script de Setup
```bash
# Cola este comando inteiro:
curl -fsSL https://raw.githubusercontent.com/Hugo-Victorr/FinTrack/main/scripts/setup-ec2.sh | bash
```

Aguarde até aparecer `? Setup concluído!`

### 3. Fazer Logout e Login
```bash
exit
ssh -i ~/Downloads/sua-chave.pem ec2-user@seu-ec2-ip
```

### 4. Editar Arquivo .env
```bash
cd ~/fintrack-deployment
nano .env

# Editar estes valores:
# DOCKER_USERNAME=seu-usuario-docker
# DOCKER_PASSWORD=seu-token-docker
# DB_PASSWORD=uma-senha-segura-aqui

# Salvar: Ctrl+O, Enter, Ctrl+X
```

? **Pronto! EC2 configurada!**

---

## ?? AGORA: Fazer Primeiro Deploy (10 min)

### 1. No seu computador
```bash
cd ~/seu-repositorio/FinTrack
```

### 2. Fazer um pequeno commit
```bash
git add .
git commit -m "feat: pipeline ci/cd configurado"
git push origin main
```

### 3. Aguardar Deploy Automático
```bash
# Ver status no GitHub:
gh run list

# Ver logs:
gh run view <RUN_ID> --log

# Ou no browser:
# https://github.com/seu-usuario/fintrack/actions
```

? **Seus 3 APIs estão sendo buildados!** (Tempo: ~10-15 min)

---

## ?? AGORA: Validar que Está Funcionando (5 min)

### Na EC2
```bash
ssh -i ~/Downloads/sua-chave.pem ec2-user@seu-ec2-ip

# Ver containers rodando
docker-compose ps

# Resultado esperado:
# ? postgres (Up)
# ? fintrack-expenses (Up)
# ? fintrack-education (Up)
# ? openfinance-api (Up)
```

### Testar as APIs
```bash
curl http://localhost:8001/health    # Expenses
curl http://localhost:8002/health    # Education
curl http://localhost:8003/health    # OpenFinance

# Esperado: {"status":"Healthy"} ou status 200
```

### Acessar de fora (seu computador)
```bash
curl http://seu-ec2-ip:8001/health
curl http://seu-ec2-ip:8002/health
curl http://seu-ec2-ip:8003/health
```

? **PARABÉNS! Seu pipeline está funcionando!** ??

---

## ?? Próximos Passos (Opcional)

### Entender melhor o pipeline
```
Ler QUICKSTART.md     (5 min)
Ler ARCHITECTURE.md   (10 min)
```

### Usar comandos úteis
```bash
# Ver logs em tempo real
docker-compose logs -f

# Ver específico
docker-compose logs -f fintrack-expenses

# Fazer deploy manual (sem rebuild)
gh workflow run quick-deploy.yml -f environment=main

# Reverter versão anterior
ssh ec2-user@ip
cd ~/fintrack-deployment
./rollback.sh
```

### Compartilhar com o time
```
1. Envie QUICKSTART.md para o time
2. Marque uma reunião para explicar
3. Deixe que todos façam um push para treinar
```

---

## ?? Se Algo Não Funcionou

### Build falha?
```bash
# Testar localmente
cd resources/fintrack-projects/fintrackdotnet
docker build -f ../../dockerfiles/fintrack.dockerfile .

# Ver erro completo
docker build -f ../../dockerfiles/fintrack.dockerfile . 2>&1 | tail -100
```

### SSH falha?
```bash
# Verificar IP
ssh -i seu-key.pem ec2-user@seu-ec2-ip

# Debug
ssh -v -i seu-key.pem ec2-user@seu-ec2-ip

# Verificar security group da EC2 (porta 22 aberta?)
```

### Containers não startam?
```bash
# Na EC2
docker-compose logs
docker-compose down -v
docker-compose up -d
```

### Para mais ajuda
```
Ler DEPLOYMENT_GUIDE.md
Ler COMMANDS.md
Ler CI_CD_DASHBOARD.txt
```

---

## ?? Resultado Final

Após estes 30 minutos, você terá:

? Pipeline automático configurado  
? GitHub Actions buildando suas 3 APIs  
? Imagens sendo pushadas para Docker Hub  
? Deploy automático na EC2  
? 3 APIs rodando em produção  
? Health checks monitorando tudo  

---

## ?? Próximas Vezes

Para futuros deploys, é **muito simples**:

```bash
# Fazer mudanças no código
# Commit e push
git push origin main

# GitHub Actions faz tudo automaticamente!
# Aguarde ~15 min

# Deploy completo! ??
```

---

## ?? Documentação Completa

Quando quiser aprender mais, leia:

1. **QUICKSTART.md** - Visão geral
2. **SETUP_CHECKLIST.md** - Detalhado  
3. **DEPLOYMENT_GUIDE.md** - Produção
4. **ARCHITECTURE.md** - Como funciona
5. **COMMANDS.md** - Referência

---

## ?? Dicas Finais

- **Commitr frequentemente** - Cada push = novo deploy
- **Monitorar logs** - `docker-compose logs -f`
- **Testar local antes** - `docker-compose -f docker-compose-full.yml up`
- **Manter .env seguro** - Nunca fazer commit
- **Usar GitHub Secrets** - Para todas as credenciais

---

**Você consegue! Comece AGORA! ??**

Qualquer dúvida: Ler documentação ou verificar troubleshooting em DEPLOYMENT_GUIDE.md

---

**Tempo decorrido:** Você deveria estar lendo isto após ~30 min de trabalho! ?
