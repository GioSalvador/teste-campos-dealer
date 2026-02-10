# Teste Técnico – API Loja do Sr. Campos

## Visão Geral
Este projeto faz parte de um teste técnico para vaga de Backend em C# .NET.  
A aplicação consiste em uma API responsável pelo gerenciamento de clientes, produtos e vendas, evoluindo uma solução base fornecida.

---

## Arquitetura e Tecnologias
- **Framework:** ASP.NET Web API (.NET Framework)
- **Linguagem:** C#
- **Persistência:** SQL Server + LINQ to SQL (DBML)
- **IDE:** Visual Studio
- **Testes de API:** Postman

A solução utiliza LINQ para acesso e manipulação dos dados, mantendo aderência à arquitetura original do projeto base.

---

## ClienteController
Funcionalidades implementadas:
- Consulta de cliente por ID
- Listagem de clientes
- Cadastro de cliente
- Atualização de cliente
- Exclusão de cliente

---

## ProdutoController
Funcionalidades implementadas:
- Consulta de produto por ID
- Listagem de produtos
- Cadastro de produto
- Atualização de produto
- Exclusão de produto

O controller já está preparado para futuras extensões relacionadas a histórico de preços.

---

## VendaController
Funcionalidades implementadas:
- Cadastro de venda vinculada a um cliente
- Registro de múltiplos itens por venda
- Persistência de itens em tabela específica (`VendaItem`)
- Consulta de vendas
- Exclusão de venda

O cadastro de venda utiliza um **ViewModel (`VendaVM`)**, permitindo:
- Separação entre contrato de API e entidades do banco
- Inclusão de múltiplos produtos por venda
- Preparação para cálculo de valores totais da venda

---

## Exemplos de Endpoints
- `GET /api/Cliente/GetById?idCliente=1`
- `GET /api/Produto/GetAll`
- `POST /api/Venda/Post`
- `DELETE /api/Venda/DeleteById?idVenda=1`

---

## Observações
- O foco inicial foi garantir a persistência correta dos dados e o funcionamento dos principais fluxos de negócio.
- Melhorias como padronização de status HTTP, cálculo de valores totais e endpoints adicionais serão abordadas em etapas futuras.
