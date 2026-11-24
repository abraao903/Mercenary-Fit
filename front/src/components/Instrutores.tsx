import React, { useEffect, useState } from 'react';
import { Instrutor } from '../interfaces/Instrutor';

const API_URL = 'http://localhost:5101';

function Instrutores() {
  const [instrutores, setInstrutores] = useState<Instrutor[]>([]);
  const [nome, setNome] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [especialidade, setEspecialidade] = useState<string>('');
  const [editandoId, setEditandoId] = useState<number | null>(null);
  const [mensagem, setMensagem] = useState<string>('');

  async function carregarInstrutores() {
    try {
      const resposta = await fetch(`${API_URL}/api/instrutor/listar`);
      if (!resposta.ok) {
        throw new Error('Erro ao carregar instrutores');
      }
      const dados: Instrutor[] = await resposta.json();
      setInstrutores(dados);
    } catch (erro) {
      console.error(erro);
      setMensagem('Erro ao buscar instrutores.');
    }
  }

  useEffect(() => {
    carregarInstrutores();
  }, []);

  function limparFormulario() {
    setNome('');
    setEmail('');
    setEspecialidade('');
    setEditandoId(null);
  }

  function iniciarEdicao(instrutor: Instrutor) {
    setEditandoId(instrutor.id);
    setNome(instrutor.nome);
    setEmail(instrutor.email);
    setEspecialidade(instrutor.especialidade);
    setMensagem('');
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setMensagem('');

    const corpo = { nome, email, especialidade };

    try {
      const url = editandoId
        ? `${API_URL}/api/instrutor/${editandoId}`
        : `${API_URL}/api/instrutor/cadastro`;
      const metodo = editandoId ? 'PUT' : 'POST';

      const resposta = await fetch(url, {
        method: metodo,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(corpo),
      });

      if (!resposta.ok) {
        const erroTexto = await resposta.text();
        setMensagem(erroTexto || 'Erro ao salvar instrutor.');
        return;
      }

      setMensagem(editandoId ? 'Instrutor atualizado!' : 'Instrutor cadastrado!');
      limparFormulario();
      await carregarInstrutores();
    } catch (erro) {
      console.error(erro);
      setMensagem('Erro de conexão com a API.');
    }
  }

  async function handleExcluir(id: number) {
    setMensagem('');
    try {
      const resposta = await fetch(`${API_URL}/api/instrutor/${id}`, {
        method: 'DELETE',
      });

      if (!resposta.ok) {
        const erroTexto = await resposta.text();
        setMensagem(erroTexto || 'Erro ao excluir instrutor.');
        return;
      }

      if (editandoId === id) {
        limparFormulario();
      }
      setMensagem('Instrutor excluído!');
      await carregarInstrutores();
    } catch (erro) {
      console.error(erro);
      setMensagem('Erro ao excluir instrutor.');
    }
  }

  return (
    <section>
      <h2>Instrutores</h2>
      {mensagem && <p>{mensagem}</p>}

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
          E-mail:
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </label>

        <label>
          Especialidade:
          <input
            type="text"
            value={especialidade}
            onChange={(e) => setEspecialidade(e.target.value)}
            required
          />
        </label>

        <button type="submit">
          {editandoId ? 'Salvar Alterações' : 'Cadastrar Instrutor'}
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
            <th>E-mail</th>
            <th>Especialidade</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {instrutores.map((inst) => (
            <tr key={inst.id}>
              <td>{inst.id}</td>
              <td>{inst.nome}</td>
              <td>{inst.email}</td>
              <td>{inst.especialidade}</td>
              <td>
                <button type="button" onClick={() => iniciarEdicao(inst)}>
                  Editar
                </button>
                <button type="button" onClick={() => handleExcluir(inst.id)}>
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

export default Instrutores;