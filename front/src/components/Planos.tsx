import React, { useEffect, useState } from 'react';
import { Plano } from '../interfaces/Plano';


const API_URL = 'http://localhost:5101';

function Planos() {
    const [planos, setPlanos] = useState<Plano[]>([]);
    const [nome, setNome] = useState<string>('');
    const [duracaoDias, setDuracaoDias] = useState<number>(30);
    const [preco, setPreco] = useState<number>(0);
    const [mensagem, setMensagem] = useState<string>('');

    async function carregarPlanos(){
        try{
            const resposta = await fetch(`${API_URL}/api/plano/listar`);
            if (resposta.status == 404){
                setPlanos([]);
                return;
            }
            if (!resposta.ok){
                throw new Error('Erro ao tentar carregar os planos');
            }
            const dados: Plano[] = await resposta.json();
            setPlanos(dados);
        } catch (erro) {
            console.error(erro);
            setMensagem('não foi possivel carregar os planos');
        } 
    }

    useEffect(() => {
        carregarPlanos();
    }, []);

    async function handleSubmit(e: React.FormEvent){
        e.preventDefault();
        setMensagem('');

        try{
            const resposta = await fetch(`${API_URL}/api/plano/cadastro`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    nome,
                    duracaoDias: Number(duracaoDias),
                    preco: Number(preco),
                }),
            });

            if (!resposta.ok){
                const erroTexto = await resposta.text();
                setMensagem(erroTexto || 'erro ao cadastrar seu plano');
                return;
            }
            setNome('');
            setDuracaoDias(30);
            setPreco(0);
            setMensagem('plano cadastrado com sucesso');
            await carregarPlanos();
        } catch (erro) {
            console.error(erro);
            setMensagem('Erro de conexão com a API')
        }
    }

    return (
        <section>
            <h2>Planos</h2>
            {mensagem && <p>{mensagem}</p>}

            <form onSubmit={handleSubmit}>
                <label>
                    nome do plano:
                    <input type='text' value={nome} onChange={(e) => setNome(e.target.value)}/>
                </label>

                <label>
                    duração (dias):
                    <input type='number' value={duracaoDias} onChange={(e) => setDuracaoDias(Number(e.target.value))}
                    min={1} required/>
                </label>

                <label>
                    Preço (R$):
                    <input type='number' step={0.01} value={preco} onChange={(e) => setPreco(Number(e.target.value))}
                    min={0.01} required/>
                </label>

                <button type='submit'>Cadastrar Plano</button>
            </form>

            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Nome</th>
                        <th>Duração (dias)</th>
                        <th>Preço</th>
                    </tr>
                </thead>

                <tbody>
                    {planos.map((plano) => ( 
                    <tr key={plano.id}>
                        <td>{plano.id}</td>
                        <td>{plano.nome}</td>
                        <td>{plano.duracaoDias}</td>
                        <td>R$ {Number(plano.preco).toFixed(2)}</td>  
                    </tr>
                    ))}
                </tbody>
            </table>
        </section>
    );
}

export default Planos