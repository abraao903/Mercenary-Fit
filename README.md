# Mercenary Fit

## Identificação do Projeto

- **Título do Projeto:** Mercenary Fit - Sistema de Gestão de Academia
- **Disciplina:** Desenvolvimento de Software Visual (Projeto Final - Parte 2)
- **Integrantes:**
  - Marcos Paulo Martins de Andrade
  - João Pedro Rosul Wojcik
  - Abraao Lima Gomes
- **Curso e Turma:** Ciência da Computação (4º / 5º Período)

---

## Resumo

O **Mercenary Fit** é uma aplicação web full-stack de gerenciamento de academia projetada para otimizar a administração de membros, a gestão de planos de assinatura e o acompanhamento das rotinas de treino físico. Desenvolvido com integração completa entre **React Vanilla** no front-end, **Minimal API em C# (.NET 8)** no back-end e banco de dados **SQLite** (`mercenaryfit.db`) via **Entity Framework Core**, o sistema vai além do simples armazenamento de dados, integrando o cadastro centralizado de alunos, a atribuição de planos vigentes, o registro de instrutores e a montagem de fichas de treino personalizadas.

---

## Tecnologias Utilizadas

- **Front-end:** React Vanilla (consumo da API REST via Fetch/HTTP, gerenciamento de estado e interface interativa)
- **Estilização:** CSS puro gerado e refinado com o auxílio de Inteligência Artificial
- **Back-end:** C# com SDK 8 (Minimal API) com política de CORS configurada (`AllowFrontend`) e tratamento de referência cíclica JSON (`ReferenceHandler.IgnoreCycles`)
- **ORM:** Entity Framework Core
- **Banco de Dados:** SQLite (`mercenaryfit.db`)
- **Versionamento e Colaboração:** Git e GitHub

---

## Entidades do Sistema

O sistema conta com **4 entidades** mapeadas no banco de dados relacional via `AppDbContext`:

1. **`Plano`**: Tipos de planos e assinaturas da academia, contendo `Id`, `Nome`, `Preco` e `DuracaoDias`.
2. **`Instrutor`**: Cadastro dos profissionais e personal trainers responsáveis pela prescrição de treinos, contendo `Id`, `Nome`, `Email` e `Especialidade`.
3. **`Aluno`**: Cadastro principal dos membros da academia, contendo `Id`, `Nome`, `Idade`, `TotalHoras`, `DataMatricula` e `PlanoId` (relacionamento obrigatório com um `Plano`).
4. **`FichaDeTreino`**: Registro da rotina de exercícios prescrita, contendo `Id`, `Titulo`, `Descricao`, `AlunoId` e `InstrutorId` (relacionando `Aluno`, seu `Plano` e o `Instrutor` responsável).

---

## Funcionalidades Implementadas (Front-end & Back-end)

- **Interface Web Integrada (React Vanilla):**
  - Comunicação direta com a Minimal API por meio de requisições HTTP (`GET`, `POST`, `PUT`, `DELETE`) liberadas via política CORS.
  - Navegação fluida entre os módulos de Planos, Instrutores, Alunos e Fichas de Treino com validação de formulários e atualização dinâmica das listas.
- **Gestão de Planos de Assinatura (`/api/plano`):**
  - Cadastro de planos (`POST /api/plano/cadastro`) com validação de nome obrigatório, preço positivo e duração mínima em dias.
  - Listagem de planos (`GET /api/plano/listar`) para exibição e seleção nas matrículas de alunos.
- **CRUD Completo de Instrutores (`/api/instrutor`):**
  - Cadastro (`POST /api/instrutor/cadastro`) exigindo nome e e-mail, além de especialidade.
  - Listagem geral (`GET /api/instrutor/listar`) e busca individual (`GET /api/instrutor/{id}`).
  - Atualização de dados cadastrais (`PUT /api/instrutor/{id}`) e remoção (`DELETE /api/instrutor/{id}`).
- **CRUD Completo de Alunos (`/api/aluno`):**
  - **Inserção (`POST /api/aluno/cadastro`):** Cadastro de novos alunos com validação de idade e vinculação obrigatória a um `PlanoId` existente no banco.
  - **Listagem e Consulta (`GET /api/aluno/listar` e `GET /api/aluno/{id}`):** Exibição estruturada dos alunos matriculados trazendo os dados do plano associado.
  - **Alteração (`PUT /api/aluno/{id}`):** Edição de nome, idade, carga horária (`TotalHoras`) e troca de plano do aluno.
  - **Remoção (`DELETE /api/aluno/{id}`):** Exclusão de cadastros de alunos do banco de dados.
- **Módulo Ativo de Ficha de Treino (`/api/ficha`):**
  - Cadastro de fichas (`POST /api/ficha/cadastro`) validando título obrigatório e a existência prévia do `AlunoId` e do `InstrutorId`.
  - Listagem detalhada (`GET /api/ficha/listar`) projetando de forma limpa os dados da ficha junto às informações do aluno, seu plano e o instrutor responsável.

---

## Descrição das Funcionalidades

### 1. Módulo de Planos e Instrutores (Entidades Base)
Permite o cadastro prévio das modalidades de assinatura (nome, preço e duração em dias) e o gerenciamento completo dos instrutores da academia (nome, e-mail e especialidade). Ambas as entidades garantem a integridade relacional do banco SQLite ao matricular alunos e prescrever treinos.

### 2. Módulo de Alunos (CRUD Completo & Relacionamento)
É o núcleo operacional do sistema. Implementa todas as operações de um CRUD completo integrado à API em C#: permite cadastrar o aluno validando idade e vinculando obrigatoriamente um **Plano** ativo via chave estrangeira (`PlanoId`). A interface permite ainda listar todos os matriculados com projeção limpa dos dados do plano, buscar por ID, editar dados ou plano e remover registros.

### 3. Módulo de Ficha de Treino (Funcionalidade Ativa)
Atendendo ao requisito de ir além do cadastro básico, esta funcionalidade cruza os dados das entidades já cadastradas para gerar valor prático no dia a dia da academia. Na tela de treinos, seleciona-se um **Instrutor** e um **Aluno** matriculado para estruturar sua ficha personalizada (`Titulo` e `Descricao`), consolidando o sistema como uma ferramenta funcional de acompanhamento físico.

---

## Estrutura e Versionamento do Repositório

Este repositório contém todo o código-fonte do **Front-end (React Vanilla)** e do **Back-end (C# .NET 8 Minimal API + Entity Framework + SQLite)** do projeto **Mercenary Fit**, além da documentação oficial para entrega acadêmica. O desenvolvimento foi realizado de forma colaborativa utilizando **Git**, com commits distribuídos entre todos os integrantes da equipe ao longo da construção do banco de dados, endpoints da API e interface visual.

---

## Uso de Inteligência Artificial

Em conformidade com as diretrizes do projeto, ferramentas de Inteligência Artificial foram utilizadas como apoio técnico no desenvolvimento e na documentação do **Mercenary Fit**:

- **Ferramenta Utilizada:** Gemini (Google).
- **Forma de Uso (Estilização, Código e Documentação):**
  - Geração e refinamento das folhas de estilo (**CSS**) da aplicação front-end em React Vanilla, criando uma identidade visual moderna e organizada para o sistema de academia.
  - Apoio na estruturação de prompts para definição das regras de negócio, validações de endpoints e relacionamentos entre as entidades (`Aluno`, `Plano`, `Instrutor` e `FichaDeTreino`).
  - Elaboração e atualização da documentação em Markdown refletindo a arquitetura final do sistema.
- **Revisões Realizadas pela Equipe:**
  - A equipe revisou, adaptou e testou manualmente todo o CSS gerado para garantir compatibilidade com os componentes React e responsividade nas telas.
  - Validação integral das rotas da Minimal API, regras de negócio no `AppDbContext` e integração com o front-end para cumprir todos os critérios de avaliação da disciplina.
