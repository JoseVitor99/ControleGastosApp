/**
 * Componente principal da aplicação.
 *
 * Responsável por:
 * - Configurar o roteamento da aplicação
 * - Definir as páginas disponíveis
 * - Manter o layout base com menu e conteúdo
 *
 * Estrutura:
 * - Menu fixo no topo
 * - Área principal onde as páginas são renderizadas conforme a rota
 */


import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";

import Menu from "./layout/Menu";
import Pessoas from "./pages/Pessoas";
import Categorias from "./pages/Categorias";
import Transacoes from "./pages/Transacoes";
import Dashboard from "./pages/ResumoFinanceiro";

function App() {
  return (
    <BrowserRouter>
      {/* Menu principal exibido em todas as páginas */}
      <Menu />

      {/* Área de conteúdo controlada pelas rotas */}
      <div className="p-4">
        <Routes>
          {/* Página inicial com resumo financeiro */}
          <Route path="/" element={<Dashboard />} />
          {/* Página de gerenciamento de pessoas */}
          <Route path="/pessoas" element={<Pessoas />} />
          {/* Página de cadastro de categorias */}
          <Route path="/categorias" element={<Categorias />} />
          {/* Página de cadastro de transações */}
          <Route path="/transacoes" element={<Transacoes />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;