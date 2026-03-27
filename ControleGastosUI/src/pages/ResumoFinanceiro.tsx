/**
 * Página de visualização de totais financeiros.
 *
 * Responsável por:
 * - Exibir totais de receitas, despesas e saldo por pessoa/categoria
 * - Apresentar o total geral consolidado ao final de cada listagem
 *
 * Integra com a API:
 * - GET /pessoas/totais
 * - GET /categorias/totais
 */

import { useEffect, useState } from "react";
import { api } from "../services/api";

import { TabView, TabPanel } from "primereact/tabview";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";

export default function Dashboard() {
    const [totaisPessoas, setTotaisPessoas] = useState<any[]>([]);
    const [totaisCategorias, setTotaisCategorias] = useState<any[]>([]);

    /**
     * Carrega os dados totais de pessoas e categorias
     */
    const load = async () => {
        const pessoas = await api.get("/pessoas/totais");
        setTotaisPessoas(pessoas.data || []);

        const categorias = await api.get("/categorias/totais");
        setTotaisCategorias(categorias.data || []);
    };

    useEffect(() => {
        load();
    }, []);

    /**
     * Cálculo dos totais gerais por pessoa
     */
    const totalReceitasPessoas = totaisPessoas.reduce((acc, p) => acc + p.totalReceitas, 0);
    const totalDespesasPessoas = totaisPessoas.reduce((acc, p) => acc + p.totalDespesas, 0);
    const saldoPessoas = totaisPessoas.reduce((acc, p) => acc + p.saldo, 0);

    /**
     * Cálculo dos totais gerais por categoria
     */
    const totalReceitasCategorias = totaisCategorias.reduce((acc, c) => acc + c.totalReceitas, 0);
    const totalDespesasCategorias = totaisCategorias.reduce((acc, c) => acc + c.totalDespesas, 0);
    const saldoCategorias = totaisCategorias.reduce((acc, c) => acc + c.saldo, 0);

    /**
     * Formata valores numéricos para moeda brasileira
     */
    const moeda = (valor: number) => valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

    /**
     * Aplica formatação visual ao saldo,
     */
    const formatacaoSaldo = (row: any) => (
        <span>{moeda(row.saldo)}</span>
    );

    return (
        <div className="p-4">

            <TabView>

                <TabPanel header="Totais por Pessoa">

                    <h2 className="mb-3">Consulta de Totais por Pessoa</h2>

                    <DataTable value={totaisPessoas} emptyMessage="Nenhum registro encontrado">
                        <Column field="nome" header="Pessoa" />

                        <Column
                            header="Receitas"
                            body={(row) => moeda(row.totalReceitas)}
                        />

                        <Column
                            header="Despesas"
                            body={(row) => moeda(row.totalDespesas)}
                        />

                        <Column
                            header="Saldo"
                            body={formatacaoSaldo}
                        />
                    </DataTable>

                    <div className="mt-4 p-3 border-top-1 surface-border">
                        <h3>Total Geral</h3>

                        <div className="grid text-center mt-3">
                            <div className="col">
                                <strong>Receitas</strong>
                                <div className="text-green-500 font-bold">
                                    {moeda(totalReceitasPessoas)}
                                </div>
                            </div>

                            <div className="col">
                                <strong>Despesas</strong>
                                <div className="text-red-500 font-bold">
                                    {moeda(totalDespesasPessoas)}
                                </div>
                            </div>

                            <div className="col">
                                <strong>Saldo</strong>
                                <div className={`font-bold ${saldoPessoas >= 0 ? "text-green-500" : "text-red-500"}`}>
                                    {moeda(saldoPessoas)}
                                </div>
                            </div>
                        </div>
                    </div>

                </TabPanel>

                <TabPanel header="Totais por Categoria">

                    <h2 className="mb-3">Consulta de Totais por Categoria</h2>

                    <DataTable value={totaisCategorias} emptyMessage="Nenhum registro encontrado">
                        <Column field="descricao" header="Categoria" />
                        <Column header="Receitas" body={(row) => moeda(row.totalReceitas)} />
                        <Column header="Despesas" body={(row) => moeda(row.totalDespesas)} />
                        <Column header="Saldo" body={formatacaoSaldo} />
                    </DataTable>

                    <div className="mt-4 p-3 border-top-1 surface-border">
                        <h3>Total Geral</h3>

                        <div className="grid text-center mt-3">
                            <div className="col">
                                <strong>Receitas</strong>
                                <div className="text-green-500 font-bold">
                                    {moeda(totalReceitasCategorias)}
                                </div>
                            </div>

                            <div className="col">
                                <strong>Despesas</strong>
                                <div className="text-red-500 font-bold">
                                    {moeda(totalDespesasCategorias)}
                                </div>
                            </div>

                            <div className="col">
                                <strong>Saldo</strong>
                                <div className={`font-bold ${saldoCategorias >= 0 ? "text-green-500" : "text-red-500"}`}>
                                    {moeda(saldoCategorias)}
                                </div>
                            </div>
                        </div>
                    </div>

                </TabPanel>

            </TabView>
        </div>
    );
}