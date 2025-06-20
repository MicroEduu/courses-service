#  Guia de Configuração e Execução do Projeto

Este documento apresenta um guia completo para configurar e executar a aplicação em sua máquina local de forma rápida e eficiente.

Este micro representa o CRUD de cursos do sistema, além de conter uma requisição para o microserviço de Auth Service para retornar o token de autenticação e os dados do usuário (todas as rotas precisam de autenticação, exceto a de login).

##  Pré-requisitos

Certifique-se de ter as seguintes ferramentas instaladas em seu sistema:

| Ferramenta | Versão Mínima | Link de Download |
|------------|---------------|------------------|
| **.NET SDK** | 8.0+ | [Download .NET](https://dotnet.microsoft.com/download) |
| **Git** | Latest | [Download Git](https://git-scm.com/downloads) |

> ** Dica:** Verifique as versões instaladas executando `dotnet --version` e `git --version`

##  Início Rápido

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/MicroEduu/courses-service
cd courses-service
cd CoursesService
```

### 2️⃣ Configurar a Solução

Se necessário, crie e configure o arquivo de solução:

```bash
# Criar arquivo de solução (se não existir)
dotnet new sln

# Adicionar projeto à solução
dotnet sln CoursesService.sln add CoursesService.csproj
```

### 3️⃣ Instalar Ferramentas Necessárias

Instale a ferramenta global do Entity Framework:

```bash
# Instalação inicial
dotnet tool install --global dotnet-ef

# Ou atualizar se já estiver instalada
dotnet tool update --global dotnet-ef
```

### 4️⃣ Configurar Banco de Dados

Execute as migrações para preparar o banco SQLite:

```bash
dotnet ef database update
```

### 5️⃣ Executar a Aplicação

```bash
dotnet run
```

🎉 **Aplicação iniciada com sucesso!**

A aplicação estará disponível em:
- 🌐 **HTTP:** http://localhost:5072/swagger/index.html

# 📌 Rotas da API

## 🔓 Rotas Públicas (sem token)

| Método | Endpoint           | Descrição                                  |
|--------|--------------------|--------------------------------------------|
| GET    | /api/Auth/token    | Obtém um token de autenticação do usuário. |

> Esta rota serve apenas como um endpoint para acionar a rota de autenticação do microserviço de autenticação.
> 
> É necessário que o microserviço de autenticação esteja em execução para que esta rota funcione corretamente.

---

## 🔒 Rotas Protegidas (com token)

### 🔐 Autenticação

| Método | Endpoint              | Descrição                                                  |
|--------|------------------------|------------------------------------------------------------|
| GET    | /api/Auth/user-info    | Retorna informações do usuário com base no token enviado.  |

---

### 🎓 Cursos (`/api/Course`)

| Método | Endpoint             | Descrição                                                       | Permissão                                  |
|--------|-----------------------|------------------------------------------------------------------|--------------------------------------------|
| GET    | /api/Course           | Lista todos os cursos cadastrados.                              | Todos os usuários                          |
| POST   | /api/Course           | Cadastra um novo curso.                                         | Apenas **admins** e **professores**       |
| GET    | /api/Course/{id}      | Busca os detalhes de um curso específico.                       | Todos os usuários                          |
| PATCH  | /api/Course/{id}      | Edita um curso específico.                                      | **Admins** ou **professores responsáveis** |
| DELETE | /api/Course/{id}      | Remove um curso específico.                                     | **Admins** ou **professores responsáveis** |


