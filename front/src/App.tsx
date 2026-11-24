import React, {useState} from 'react';
import Planos from './components/Planos';
import Instrutores from './components/Instrutores';
import Alunos from './components/Alunos';
import FichasTreino from './components/FichasTreino';
import './App.css'

type AbaAtive = 'planos' | 'instrutores' | 'alunos' | 'fichas';

function App() {
  const [aba, setAba] = useState<AbaAtive>('planos');

  return (
    <div className='App'>
      <header>
        <h1> Mercenary Fit</h1>
        <nav>
          <button type='button' onClick={() => setAba('planos')}>
            Planos
          </button>

          <button type='button' onClick={() => setAba('instrutores')}>
            Instrutores
          </button>

          <button type='button' onClick={() => setAba('alunos')}>
            Alunos
          </button>

          <button type='button' onClick={() => setAba('fichas')}>
            fichas de treino
          </button>

        </nav>
      </header>
      
      <main>
        {aba === 'planos' && <Planos />}
        {aba === 'instrutores' && <Instrutores />}
        {aba === 'alunos' && <Alunos />}
        {aba === 'fichas' && <FichasTreino />}
      </main>
    </div>
  );
}

export default App;
