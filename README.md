# Teste Técnico – API Loja do Sr. Campos

## Objetivo do Projeto
Este projeto consiste na evolução de uma API REST para gerenciamento de clientes, produtos e vendas da “Loja do Sr. Campos”.

---

## Arquitetura e Tecnologias
- **Framework:** ASP.NET Web API (.NET Framework)
- **Linguagem:** C#
- **Persistência:** SQL Server + LINQ to SQL (DBML)
- **IDE:** Visual Studio
- **Testes de API:** Postman

A solução utiliza LINQ para acesso e manipulação dos dados, mantendo aderência à arquitetura original do projeto base.

---

## Arquitetura do Sistema
A aplicação foi desenvolvida utilizando arquitetura baseada em:
- Controllers (camada de exposição HTTP)
- DTOs (transferência de dados)
- LINQ to SQL (acesso a dados)
- Banco relacional SQL Server
- 
Modelo: 
Controller → DataContext → Banco de Dados
Estrutura do projeto no Visual Studio:

Backend:

<img width="255" height="460" alt="Image" src="https://github.com/user-attachments/assets/e2c4d66a-14d6-4dca-b200-6b2a71fd5a14" />

Frontend:

<img width="203" height="312" alt="Image" src="https://github.com/user-attachments/assets/cf2cd1b8-6b63-4ff6-b94b-5d2a6ed78b0a" />

---

## Estrutura do Banco de Dados
<img width="966" height="476" alt="Image" src="https://github.com/user-attachments/assets/5071e8ae-194a-4a57-89ed-d725960993f0" />

---

## Endpoints Implementados

Cliente:
- GET /api/cliente
- GET /api/cliente/{id}
- POST /api/cliente
- PUT  /api/cliente/{id}
- DELETE /api/cliente/{id}
- 
Produto:
- GET /api/produto
- GET /api/produto/{id}
- POST /api/produto
- PUT  /api/produto/{id}
- DELETE /api/produto/{id}
- 
Venda:
- GET /api/venda
- GET /api/venda/{id}
- GET /api/venda/cliente/{id}
- POST /api/venda
- PUT  /api/venda/{id}
- DELETE /api/venda/{id}
- GET /api/venda/ranking

[Link para a collection do Postman aqui](https://giosalvador-142763.postman.co/workspace/Giovani-Salvador's-Workspace~7dfb4bd5-28c7-4ac1-8ec2-839a62292a10/collection/51946502-98488018-6c19-4ee6-923f-5ae6a21b4b33?action=share&creator=51946502&active-environment=51946502-ba789232-cd74-4b4c-9c1a-d1d38fbfe994)

---

## Decisões Técnicas e Melhorias Implementadas

### Padronização de Respostas HTTP

Antes:

- Strings simples em erro
- Respostas inconsistentes
  
Depois:

Uso correto de:
- 200 OK
- 201 Created
- 400 BadRequest
- 404 NotFound
- 500 InternalServerError
  
Retornos padronizados em JSON:

<img width="442" height="131" alt="Image" src="https://github.com/user-attachments/assets/863d8d88-0dba-4c4f-90fe-b522ba3c5335" />

### Desativação de Lazy Loading

Foi utilizado:

```bash
db.DeferredLoadingEnabled = false;
```

Objetivo:

- Evitar carregamento desnecessário de relacionamentos
- Melhorar performance
- Evitar problemas de serialização

### Implementação de DTO para Venda

Motivo:

- Evitar exposição direta da entidade
- Permitir envio estruturado com lista de itens
- Maior controle da lógica de negócio
- 
### Regra de Negócio: Venda Imutável

Decisão importante:

Após registrada, a venda não pode ser alterada.

Motivo:

- Consistência financeira
- Boas práticas de sistemas transacionais
- Evitar inconsistência histórica

PUT retorna:
```bash
400 - Venda não pode ser alterada após registro.
```

### Venda com múltiplos itens

Uma venda deve conter pelo menos um item.

<img width="260" height="389" alt="Image" src="https://github.com/user-attachments/assets/0289adb3-fcf4-48a3-b834-7a8c023ddd36" />

### Rastreabilidade de Preço

Sempre que o preço de um produto é alterado:

- Atualiza precoAtual
- Insere registro em ProdutoPrecoHistorico

Isso garante auditoria e rastreabilidade completa.

### Implementação do Ranking

Nenhum valor financeiro é confiado ao frontend.
A API:

- Busca precoAtual no banco
- Busca precoAtual no banco
- Calcula vlrTotalItem
- Soma totalVenda

Isso evita manipulação indevida de valores.

### Implementação do Ranking

Critério:

- Ordenação por vlrTotalVenda DESC
- Top 10 registros

Implementação:

<img width="347" height="44" alt="Image" src="https://github.com/user-attachments/assets/ac98d1cb-6367-4950-ac95-03534db88380" />

Retorno:

<img width="485" height="732" alt="Image" src="https://github.com/user-attachments/assets/704c8610-3877-4876-8007-3c009feb74f0" />

## Tratamento de Erros

Foram implementados:

- Validação de body nulo
- Validação de campos obrigatórios
- Validação de existência de cliente
- Validação de existência de produto
- Try/catch em operações críticas
- Retorno padronizado em JSON

## Testes Realizados

Testes realizados via Postman:

- Cenários positivos
- Cenários negativos
- Validação de regras de negócio
- Testes de integridade referencial

<img width="374" height="365" alt="Image" src="https://github.com/user-attachments/assets/3c364a90-cb31-4ade-9c5e-c1b8758c66b2" />

## Controle de Versão

- Uso de Git
- Organização por branches
- Commits descritivos
- Separação de refatoração e documentação

## Conclusão

A API foi estruturada com foco em organização, padronização de respostas HTTP, regras de negócio bem definidas e boas práticas de desenvolvimento.
A arquitetura permite evolução futura para implementação de autenticação, paginação, cache e separação em camadas de serviço.

