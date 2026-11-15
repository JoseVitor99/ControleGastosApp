/**
 * Componente de navegação principal da aplicação.
 *
 * Responsável por:
 * - Exibir o menu superior com acesso às principais páginas
 * - Realizar a navegação entre rotas utilizando react-router
 *
 * Rotas disponíveis:
 * - /              → Resumo financeiro
 * - /pessoas       → Cadastro de pessoas
 * - /categorias    → Cadastro de categorias
 * - /transacoes    → Cadastro de transações
 */

import { Menubar } from "primereact/menubar";
import { useNavigate } from "react-router-dom";

export default function Menu() {
    const navigate = useNavigate();

    /**
     * Estrutura de itens do menu.
     * Cada item define o rótulo, ícone e ação de navegação.
     */
    const items = [
        {
            label: "Resumo Financeiro",
            icon: "pi pi-money-bill",
            command: () => navigate("/")
        },
        {
            label: "Pessoas",
            icon: "pi pi-users",
            command: () => navigate("/pessoas")
        },
        {
            label: "Categorias",
            icon: "pi pi-tags",
            command: () => navigate("/categorias")
        },
        {
            label: "Transações",
            icon: "pi pi-wallet",
            command: () => navigate("/transacoes")
        }
    ];

    return <Menubar model={items} />;
}