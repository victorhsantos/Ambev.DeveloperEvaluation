# Ambev Developer Evaluation

## Descrição do Projeto

Este projeto é uma aplicação de avaliação de desenvolvedores, estruturada com Clean Architecture e orientada a eventos. O objetivo é fornecer uma plataforma robusta para gerenciar avaliações de desenvolvedores, utilizando tecnologias modernas e boas práticas de desenvolvimento.

## Estrutura do Projeto

O projeto segue os princípios da Clean Architecture, dividindo a aplicação em camadas bem definidas:

- **Domain**: Contém as entidades, eventos de domínio, interfaces de repositório e validações. Esta camada é independente de frameworks e bibliotecas externas.
- **Application**: Implementa os casos de uso da aplicação, utilizando MediatR para mediar as interações entre os componentes.
- **Infrastructure**: Contém a implementação dos repositórios, contexto do Entity Framework Core e outras dependências externas.
- **CrossCutting**: Inclui serviços e utilitários que são utilizados em várias partes da aplicação.
- **Adapters**: Implementa adaptadores para integração com serviços externos, como RabbitMQ.
- **API**: Contém a API Web, configurada com ASP.NET Core, incluindo controladores e configuração do Swagger.

### Clean Architecture

A Clean Architecture garante que a lógica de negócio e as regras de aplicação sejam independentes de frameworks, UI, banco de dados ou qualquer outra dependência externa. Isso facilita a manutenção, testes e evolução do sistema.

### Domain Events

O projeto utiliza Domain Events para notificar outras partes do sistema sobre mudanças importantes no estado das entidades de domínio. Isso promove um design desacoplado e facilita a implementação de funcionalidades reativas.

## Tecnologias Utilizadas

- .NET 8
- MediatR
- Entity Framework Core
- FluentValidation
- AutoMapper
- Swagger
- xUnit (para testes)
- RabbitMQ
- MassTransit
- Serilog
- PostgreSQL
- MongoDB
- Redis

## Como Configurar o Projeto

### Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/ambev-developer-evaluation.git cd ambev-developer-evaluation
````

### Requisitos

- .NET 8 SDK
- Docker (para rodar os serviços de banco de dados, cache e mensageria)

### Configuração de Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto com as seguintes variáveis:
```
ASPNETCORE_ENVIRONMENT=Development ConnectionStrings__DefaultConnection=Host=localhost;Database=developer_evaluation;Username=developer;Password=ev@luAt10n
```

### Rodar as Migrations
```
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM
```

## Como Executar a Aplicação

### Via Terminal
```
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

### Via Visual Studio

1. Abra a solução `Ambev.DeveloperEvaluation.sln` no Visual Studio.
2. Selecione o projeto `Ambev.DeveloperEvaluation.WebApi` como projeto de inicialização.
3. Pressione `F5` para iniciar a aplicação.

### Endpoints Principais

Os principais endpoints da aplicação estão definidos no [SalesController.cs](#salescontroller.cs-context):

- `POST /api/sales` - Cria uma nova venda
- `PUT /api/sales` - Atualiza uma venda existente
- `GET /api/sales` - Recupera vendas com base nos filtros fornecidos
- `DELETE /api/sales` - Exclui uma venda com base no número da venda

### Documentação Swagger

Acesse a documentação Swagger em `http://localhost:8080/swagger`.

## Como Executar os Testes

### Testes Unitários e de Integração
```
dotnet test
```

Os resultados dos testes serão exibidos no terminal. Para mais detalhes, utilize a janela de Test Explorer no Visual Studio.

## Contribuições

Contribuições são bem-vindas! Siga as orientações abaixo:

1. Faça um fork do repositório.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`).
4. Envie para o repositório (`git push origin feature/nova-feature`).
5. Abra um Pull Request.

Certifique-se de seguir as convenções de commit e manter a organização das issues.

---

Este README foi gerado para fornecer uma visão geral clara e concisa do projeto, facilitando a configuração, execução e contribuição para o mesmo.
