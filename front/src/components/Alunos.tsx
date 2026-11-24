import React, { useEffect, useState } from 'react';
import { Aluno } from '../interfaces/Aluno';
import { Plano } from '../interfaces/Plano';

const API_URL = 'http://localhost:5101';

function Alunos() {
  const [alunos, setAlunos] = useState<Aluno[]>([]);
  const [planos, setPlanos] = useState<Plano[]>([]);
  const [nome, setNome] = useState<string>('');
  const [idade, setIdade] = useState<number>(18);
  const [totalHoras, setTotalHoras] = useState<number>(0);
  const [planoId, setPlanoId] = useState<number>(0);
  const [editandoId, setEditandoId] = useState<number | null>(null);
  const [mensagem, setMensagem] = useState<string>('');

  async function carregarPlanos() {
    try {
      const resposta = await fetch(`${API_URL}/api/plano/listar`);
      if (resposta.status === 404) {
        setPlanos([]);
        return;
      }
      if (!resposta.ok) {
        throw new Error('Erro ao carregar planos');
      }
      const dados: Plano[] = await resposta.json();
      setPlanos(dados);
      if (dados.length > 0 && planoId === 0) {
        setPlanoId(dados[0].id);
      }
    } catch (erro) {
      console.error(erro);
    }
  }

  async function carregarAlunos() {
    try {
      const resposta = await fetch(`${API_URL}/api/aluno/listar`);
      if (!resposta.ok) {
        throw new Error('Erro ao carregar alunos');
      }
      const dados: Aluno[] = await resposta.json();
      setAlunos(dados);
    } catch (erro) {
      console.error(erro);
      setMensagem('Erro ao buscar alunos.');
    }
  }

  useEffect(() => {
    carregarPlanos();
    carregarAlunos();
  }, []);

  function limparFormulario() {
    setNome('');
    setIdade(18);
    setTotalHoras(0);
    setPlanoId(planos.length > 0 ? planos[0].id : 0);
    setEditandoId(null);
  }

  function iniciarEdicao(aluno: Aluno) {
    setEditandoId(aluno.id);
    setNome(aluno.nome);
    setIdade(aluno.idade);
    setTotalHoras(aluno.totalHoras);
    setPlanoId(aluno.planoId);
    setMensagem('');
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setMensagem('');

    if (!planoId) {
      setMensagem('Cadastre e selecione um plano antes de salvar o aluno.');
      return;
    }

    const corpo = {
      nome,
      idade: Number(idade),
      totalHoras: Number(totalHoras),
      planoId: Number(planoId),
    };

    try {
      const url = editandoId
        ? `${API_URL}/api/aluno/${editandoId}`
        : `${API_URL}/api/aluno/cadastro`;
      const metodo = editandoId ? 'PUT' : 'POST';

      const resposta = await fetch(url, {
        method: metodo,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(corpo),
      });

      if (!resposta.ok) {
        const erroTexto = await resposta.text();
        setMensagem(erroTexto || 'Erro ao salvar aluno.');
        return;
      }

      setMensagem(editandoId ? 'Aluno atualizado!' : 'Aluno cadastrado!');
      limparFormulario();
      await carregarAlunos();
    } catch (erro) {
      console.error(erro);
      setMensagem('Erro de conexão com a API.');
    }
  }

  async function handleExcluir(id: number) {
    setMensagem('');
    try {
      const resposta = await fetch(`${API_URL}/api/aluno/${id}`, {
        method: 'DELETE',
      });

      if (!resposta.ok) {
        const erroTexto = await resposta.text();
        setMensagem(erroTexto || 'Erro ao excluir aluno.');
        return;
      }

      if (editandoId === id) {
        limparFormulario();
      }
      setMensagem('Aluno excluído!');
      await carregarAlunos();
    } catch (erro) {
      console.error(erro);
      setMensagem('Erro ao excluir aluno.');
    }
  }

  return (
    <section>
      <h2>Alunos</h2>
      {mensagem && <p>{mensagem}</p>}

      {planos.length === 0 && (
        <p>Atenção: cadastre pelo menos um plano na aba Planos antes de cadastrar alunos.</p>
      )}

      <form onSubmit={handleSubmit}>
        <label>
          Nome:
          <input
            type="text"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            required
          />
        </label>

        <label>
          Idade:
          <input
            type="number"
            value={idade}
            onChange={(e) => setIdade(Number(e.target.value))}
            min={1}
            required
          />
        </label>

        <label>
          Total de Horas:
          <input
            type="number"
            value={totalHoras}
            onChange={(e) => setTotalHoras(Number(e.target.value))}
            min={0}
            required
          />
        </label>

        <label>
          Plano:
          <select
            value={planoId}
            onChange={(e) => setPlanoId(Number(e.target.value))}
            required
          >
            <option value={0} disabled>
              Selecione um plano
            </option>
            {planos.map((plano) => (
              <option key={plano.id} value={plano.id}>
                {plano.nome} (R$ {Number(plano.preco).toFixed(2)})
              </option>
            ))}
          </select>
        </label>

        <button type="submit">
          {editandoId ? 'Salvar Alterações' : 'Cadastrar Aluno'}
        </button>

        {editandoId && (
          <button type="button" onClick={limparFormulario}>
            Cancelar
          </button>
        )}
      </form>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nome</th>
            <th>Idade</th>
            <th>Horas</th>
            <th>Plano</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {alunos.map((aluno) => (
            <tr key={aluno.id}>
              <td>{aluno.id}</td>
              <td>{aluno.nome}</td>
              <td>{aluno.idade}</td>
              <td>{aluno.totalHoras}</td>
              <td>{aluno.plano?.nome ?? `Plano #${aluno.planoId}`}</td>
              <td>
                <button type="button" onClick={() => iniciarEdicao(aluno)}>
                  Editar
                </button>
                <button type="button" onClick={() => handleExcluir(aluno.id)}>
                  Excluir
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}

export default Alunos;