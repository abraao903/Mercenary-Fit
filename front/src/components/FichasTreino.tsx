import React, { useEffect, useState } from 'react';
import { FichaDeTreino } from '../interfaces/FichaDeTreino';
import { Aluno } from '../interfaces/Aluno';
import { Instrutor } from '../interfaces/Instrutor';

const API_URL = 'http://localhost:5101';

function FichasTreino() {
  const [fichas, setFichas] = useState<FichaDeTreino[]>([]);
  const [alunos, setAlunos] = useState<Aluno[]>([]);
  const [instrutores, setInstrutores] = useState<Instrutor[]>([]);

  const [titulo, setTitulo] = useState<string>('');
  const [descricao, setDescricao] = useState<string>('');
  const [alunoId, setAlunoId] = useState<number>(0);
  const [instrutorId, setInstrutorId] = useState<number>(0);
  const [mensagem, setMensagem] = useState<string>('');

  async function carregarDadosApoio() {
    try {
      const [resAlunos, resInstrutores] = await Promise.all([
        fetch(`${API_URL}/api/aluno/listar`),
        fetch(`${API_URL}/api/instrutor/listar`),
      ]);

      if (resAlunos.ok) {
        const listaAlunos: Aluno[] = await resAlunos.json();
        setAlunos(listaAlunos);
        if (listaAlunos.length > 0 && alunoId === 0) {
          setAlunoId(listaAlunos[0].id);
        }
      }

      if (resInstrutores.ok) {
        const listaInstrutores: Instrutor[] = await resInstrutores.json();
        setInstrutores(listaInstrutores);
        if (listaInstrutores.length > 0 && instrutorId === 0) {
          setInstrutorId(listaInstrutores[0].id);
        }
      }
    } catch (erro) {
      console.error(erro);
    }
  }

  async function carregarFichas() {
    try {
      const resposta = await fetch(`${API_URL}/api/ficha/listar`);
      if (!resposta.ok) {
        throw new Error('Erro ao carregar fichas de treino');
      }
      const dados: FichaDeTreino[] = await resposta.json();
      setFichas(dados);
    } catch (erro) {
      console.error(erro);
      setMensagem('Erro ao buscar fichas de treino.');
    }
  }

  useEffect(() => {
    carregarDadosApoio();
    carregarFichas();
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setMensagem('');

    if (!alunoId || !instrutorId) {
      setMensagem('Selecione um aluno e um instrutor cadastrados.');
      return;
    }

    try {
      const resposta = await fetch(`${API_URL}/api/ficha/cadastro`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          titulo,
          descricao,
          alunoId: Number(alunoId),
          instrutorId: Number(instrutorId),
        }),
      });

      if (!resposta.ok) {
        const erroTexto = await resposta.text();
        setMensagem(erroTexto || 'Erro ao cadastrar ficha de treino.');
        return;
      }

      setTitulo('');
      setDescricao('');
      setMensagem('Ficha de treino gerada com sucesso!');
      await carregarFichas();
    } catch (erro) {
      console.error(erro);
      setMensagem('Erro de conexão com a API.');
    }
  }

  return (
    <section>
      <h2>Fichas de Treino</h2>
      {mensagem && <p>{mensagem}</p>}

      <form onSubmit={handleSubmit}>
        <label>
          Título da Ficha:
          <input
            type="text"
            value={titulo}
            onChange={(e) => setTitulo(e.target.value)}
            required
          />
        </label>

        <label>
          Descrição / Treino:
          <input
            type="text"
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
            required
          />
        </label>

        <label>
          Aluno:
          <select
            value={alunoId}
            onChange={(e) => setAlunoId(Number(e.target.value))}
            required
          >
            <option value={0} disabled>
              Selecione um aluno
            </option>
            {alunos.map((aluno) => (
              <option key={aluno.id} value={aluno.id}>
                {aluno.nome}
              </option>
            ))}
          </select>
        </label>

        <label>
          Instrutor:
          <select
            value={instrutorId}
            onChange={(e) => setInstrutorId(Number(e.target.value))}
            required
          >
            <option value={0} disabled>
              Selecione um instrutor
            </option>
            {instrutores.map((inst) => (
              <option key={inst.id} value={inst.id}>
                {inst.nome} ({inst.especialidade})
              </option>
            ))}
          </select>
        </label>

        <button type="submit">Gerar Ficha de Treino</button>
      </form>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Título</th>
            <th>Descrição</th>
            <th>Aluno</th>
            <th>Plano do Aluno</th>
            <th>Instrutor</th>
          </tr>
        </thead>
        <tbody>
          {fichas.map((ficha) => (
            <tr key={ficha.id}>
              <td>{ficha.id}</td>
              <td>{ficha.titulo}</td>
              <td>{ficha.descricao}</td>
              <td>{ficha.aluno?.nome ?? `Aluno #${ficha.alunoId}`}</td>
              <td>{ficha.aluno?.plano?.nome ?? '-'}</td>
              <td>{ficha.instrutor?.nome ?? `Instrutor #${ficha.InstrutorId}`}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}

export default FichasTreino;