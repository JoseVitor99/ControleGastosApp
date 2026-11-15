# Controle de Gastos

Sistema para gerenciamento e controle de gastos residenciais, com gerenciamento de pessoas, categorias e transações, permitindo a visualização de totais e saldo geral.

---

## Objetivo

O projeto tem como finalidade organizar informações financeiras de forma simples, permitindo o registro de receitas e despesas associadas a pessoas e categorias.

---

## Arquitetura e Tecnologias

A aplicação foi dividida em duas partes principais:

### Backend (API)

* .NET (ASP.NET Core)
* Entity Framework Core
* SQLite

Responsável por:

* Regras de negócio
* Organização dos endpoints REST

### Frontend (Interface)

* React
* TypeScript
* PrimeReact / PrimeFlex

Responsável por:

* Interação com o usuário
* Consumo da API
* Apresentação dos registros

---

## Estrutura do Projeto

ControleGastosApp/

 -> ControleGastoApi/ -> Backend (API REST)
 
 -> ControleGastosUI/ -> Frontend (Interface)

### Organização do Backend

Controllers -> Rotas da API
Services    -> Regras de negócio
Data        -> Contexto do banco (Entity Framework)
Entidade    -> Espelho do Banco
DTO         -> Objetos de transferência de dados

---

## Funcionalidades

### Pessoas

* Cadastro, edição, exclusão e listagem

### Categorias

* Cadastro e listagem
* Definição de finalidade: Receita, Despesa ou Ambas

### Transações

* Registro de receitas e despesas
* Associação com pessoa e categoria

### Resumo Financeiro (Consultas)

* Totais por pessoa
* Totais por categoria
* Total geral consolidado

---

## Regras de Negócio

* O valor da transação deve ser maior que zero
* Pessoas menores de idade não podem possuir receitas
* A categoria deve ser compatível com o tipo da transação
* O saldo é calculado como: receitas-despesas

---

## Fluxo de Utilização

1. Cadastrar uma pessoa
2. Cadastrar uma categoria
3. Registrar uma transação
4. Consultar os totais no resumo financeiro

---

## Como Executar o Projeto

### Backend

cd ControleGastoApi/ControleGastoApi

dotnet run



A API será iniciada em: http://localhost:5000

---

### Frontend

cd ControleGastosUI

npm install

npm start



A aplicação estará disponível em: http://localhost:3000

---

## Observações

* O banco SQLite é criado automaticamente na primeira execução
* O CORS está liberado para facilitar a integração entre frontend e backend
* As regras de negócio estão centralizadas na camada de serviços

---

## Considerações Finais

O projeto foi desenvolvido com foco em clareza, simplicidade, organização e leitura, atendendo aos requisitos propostos e permitindo fácil manutenção.
