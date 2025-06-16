# Lumen-API ⚡


Este repositório contém o backend da plataforma **Lumen**, desenvolvido em ASP.NET Core. Ele fornece a API que dá suporte às funcionalidades da aplicação, como autenticação, cadastro de usuários, doações e geração de relatórios para as ONGs cadastradas.

---

## 📌 Funcionalidades

- ✅ Autenticação com JWT
- 🏠 Cadastro e gerenciamento de ONGs
- 💸 Realização de doações
- 📊 Geração de relatórios
- 📄 Integração com Swagger para documentação da API

---

## 👥 Integrantes

- Amanda Queiroz Sobral  
- Carlos Eduardo Domingues Hobmeier  
- João Vitor de Freitas  
- Théo Lucas Nicoleli  
- Tiago Bisolo Prestes  

---

## 🛠️ Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/)
- [ASP.NET Core Web API](https://learn.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/ef/)
- [Swagger / Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [MySQL](https://www.mysql.com/)
- [JWT](https://jwt.io/)
- [AutoMapper](https://automapper.org/)

---

## 📦 Como Rodar o Projeto Localmente

### ✅ Pré-requisitos

- [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/) com extensão C#
ou
- [JetBrains Rider]()
- Git instalado na máquina

---

### 🚀 Instruções de Execução

1. **Clone o repositório**

```bash
git git@github.com:theonicoleli/Lumen-API.git
cd Lumen-API
```

2. **Configure o banco de dados**

Crie um banco de dados MySQL com o nome desejado (ex: lumen_db)

Atualize a connection string no arquivo appsettings.json:

```
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=lumen_db;user=root;password=SENHA_AQUI"
}
```

3. **Restaure os pacotes**
```
dotnet restore
```

4. **Execute as migrations (se aplicável)**

Se já tiver banco existente execute as migrations, do contrário não é necessário.

```
dotnet ef database update
```

Se não tiver dotnet-ef instalado:
```
dotnet tool install --global dotnet-ef
```

5. **Rode a aplicação**

```
dotnet run --project Lumen-API
```

6. **A API estará disponível em:**

https://localhost:7142/swagger/index.html
O número da porta (7142) pode variar. O terminal mostrará o endereço exato após o comando npm run dev
