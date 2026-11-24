import { Aluno } from "./Aluno";
import { Instrutor } from "./Instrutor";

export interface FichaDeTreino{
    id: number;
    titulo: string;
    descricao: string;
    alunoId: number;
    InstrutorId: number;
    aluno?: Aluno | null;
    Instrutor?: Instrutor | null;
}