# Mercenary Fit

## Identificação do Projeto

- **Título do Projeto:** Mercenary Fit - Sistema de Gestão de Academia
- **Integrantes:**
  - Marcos Paulo Martins de Andrade
  - João Pedro Rosul Wojcik
  - Abraao Lima Gomes
- **Curso e Turma:** Ciência da Computação (4º / 5º Período)

---

## Resumo

O **Mercenary Fit** é um sistema de gerenciamento de academia projetado para otimizar a administração de membros, a gestão de planos de assinatura e o acompanhamento das rotinas de treino físico. Desenvolvido para atender a requisitos operacionais que vão além do simples armazenamento de dados, o sistema integra o cadastro centralizado de alunos, a atribuição de planos vigentes, o registro de instrutores e a montagem de fichas de treino personalizadas. A solução automatiza a alocação de treinos e garante uma gestão centralizada, ágil e eficiente para administradores e personal trainers.

---

## Entidades do Sistema

O sistema conta com **4 entidades** mapeadas no banco de dados:

1. **`Plano`**: Tipos de planos e assinaturas da academia (ex: Mensal, Trimestral, Anual).
2. **`Instrutor`**: Cadastro dos profissionais e personal trainers responsáveis pela prescrição de treinos.
3. **`Aluno`**: Cadastro principal dos membros da academia.
4. **`FichaDeTreino`**: Registro da rotina de exercícios prescrita, associando aluno, instrutor e conjunto de exercícios.

---

## Funcionalidades (em tópicos)

- **Gestão de Planos de Assinatura:**
  - Cadastro de modalidades de planos com valores e duração.
  - Listagem de planos para seleção nas matrículas.
- **Gestão de Instrutores:**
  - Cadastro e listagem dos instrutores da academia.
- **CRUD Completo de Alunos (Entidade Principal):**
  - Inserção de novos alunos vinculados obrigatoriamente a um **Plano** ativo.
  - Listagem geral dos alunos matriculados e consulta detalhada.
  - Atualização dos dados cadastrais e alteração de plano do aluno.
  - Remoção de cadastros de alunos do banco de dados.
- **Módulo Ativo de Ficha de Treino (Funcionalidade Principal):**
  - Vinculação de uma ficha de treino personalizada a um **Aluno** sob a responsabilidade de um **Instrutor**.
  - Montagem e consulta da divisão de rotinas (A/B/C), séries e repetições de cada membro.

---

## Descrição das Funcionalidades

### 1. Módulo de Planos e Instrutores (Entidades Base)
Permite o cadastro prévio das modalidades de assinatura (duração, preço) e dos instrutores da academia. Ambas as entidades servem como pré-requisitos fundamentais para garantir a integridade relacional do banco de dados ao matricular alunos e criar treinos.

### 2. Módulo de Alunos (CRUD Completo & Relacionamento)
É o núcleo operacional do sistema. Implementa todas as operações de um CRUD tradicional: permite cadastrar o aluno preenchendo dados pessoais e selecionando obrigatoriamente um **Plano** já existente no banco (relacionamento por Chave Estrangeira/Foreign Key). O módulo permite ainda listar todos os matriculados, atualizar seus dados e remover registros inativos.

### 3. Módulo de Ficha de Treino (Funcionalidade Ativa)
Atendendo ao requisito de ir além do cadastro básico, esta funcionalidade utiliza os dados cadastrados no banco para criar valor prático. O **Instrutor** seleciona um **Aluno** matriculado e estrutura sua ficha de treino personalizada (grupo muscular, séries, repetições e observações). Isso garante que o sistema atue como uma ferramenta completa de acompanhamento físico diário.

---

## Repositório

Este repositório contém todo o código-fonte, scripts do banco de dados e documentação do projeto **Mercenary Fit**, servindo como ambiente oficial de desenvolvimento e entrega para avaliação acadêmica.