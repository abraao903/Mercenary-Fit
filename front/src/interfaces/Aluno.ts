import { Plano } from "./Plano";

export interface Aluno{
    id: number;
    nome: string;
    idade: number;
    totalHoras: number;
    dataMatricula?: string;
    planoId: number;
    plano?: Plano | null;
}