/**
 * Página de Cadastro, Atualização e Exclusão de pessoas.
 *
 * Responsável por:
 * - Listar pessoas cadastradas
 * - Criar novos registros
 * - Editar registros existentes
 * - Excluir registros com confirmação
 *
 * Integra com a API:
 * - GET    /pessoas
 * - POST   /pessoas
 * - PUT    /pessoas/{id}
 * - DELETE /pessoas/{id}
 */

import { useEffect, useState } from "react";
import { api } from "../services/api";
import IPessoas from "../interfaces/IPessoas";
import { Toast } from "primereact/toast";
import { useRef } from "react";

import { InputText } from "primereact/inputtext";
import { InputNumber } from "primereact/inputnumber";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { ConfirmDialog, confirmDialog } from "primereact/confirmdialog";

export default function Pessoas() {
    // Referência para exibição de mensagens ao usuário
    const toast = useRef<Toast>(null);

    // states definidos da tela
    const [pessoas, setPessoas] = useState<IPessoas[]>([]);
    const [nome, setNome] = useState("");
    const [idade, setIdade] = useState<number | null>(null);
    const [editandoId, setEditandoId] = useState<number | null>(null);

    const load = async () => {
        // Realiza a busca de pessoas na api e atualiza a listagem exibida na tabela
        const response = await api.get("/pessoas");
        setPessoas(response.data || []);
    };

    // Executa o carregamento inicial ao renderizar a página
    useEffect(() => {
        load();
    }, []);

    /**
     * Responsável por criar ou atualizar uma pessoa.
     * Se houver um ID em edição, realiza atualização (PUT),
     * caso não, cria um novo registro (POST).
     */
    const submit = async () => {
        try {
            if (editandoId) {
                // Atualização de registro existente
                await api.put(`/pessoas/${editandoId}`, { nome, idade });
            } else {
                // Criação de novo registro
                await api.post("/pessoas", { nome, idade });
            }

            // Toast de sucesso
            toast.current?.show({
                severity: "success",
                summary: "Sucesso",
                detail: `Pessoa ${editandoId ? "atualizada" : "cadastrada"} com sucesso!`,
                life: 3000
            });

            // Limpa o formulário e recarrega os dados
            setNome("");
            setIdade(null);
            setEditandoId(null);
            load();

        } catch (err: any) {
            // Tratamento de erro com retorno da api, se disponível
            const mensagem = err.response?.data || "Erro ao salvar pessoa.";

            toast.current?.show({
                severity: "error",
                summary: "Erro",
                detail: mensagem,
                life: 4000
            });
        }
    };

    /**
     * Formata a string de idade para exibição na tabela
     */
    const formatacaoIdade = (rowData: IPessoas) => {
        return `${rowData.idade} anos`;
    };

    /**
     * Solicita confirmação antes de excluir uma pessoa.
     * Caso confirmado, realiza a exclusão via endpoint delete.
     */
    const remover = (id: number) => {
        confirmDialog({
            message: "Tem certeza que deseja excluir esta pessoa? Esta ação não poderá ser desfeita.",
            header: "Confirmação",
            icon: "pi pi-exclamation-triangle",

            acceptLabel: "Sim",
            rejectLabel: "Não",

            acceptClassName: "p-button-danger",
            rejectClassName: "p-button-secondary",

            accept: async () => {
                try {
                    await api.delete(`/pessoas/${id}`);

                    toast.current?.show({
                        severity: "success",
                        summary: "Sucesso",
                        detail: "Pessoa removida com sucesso!",
                        life: 3000
                    });

                    load();

                } catch (err: any) {
                    toast.current?.show({
                        severity: "error",
                        summary: "Erro",
                        detail: err.response?.data || "Erro ao remover pessoa",
                        life: 4000
                    });
                }
            }
        });
    };

    /**
     * Preenche o formulário com os dados da pessoa selecionada
     * e ativa o modo de edição
     */
    const editar = (p: IPessoas) => {
        setEditandoId(p.id);
        setNome(p.nome);
        setIdade(p.idade);
    };

    /**
     * Template de ações da tabela (editar e excluir)
     */
    const acoesTemplate = (rowData: IPessoas) => {
        return (
            <div className="flex gap-2">
                <Button
                    icon="pi pi-pencil"
                    className="p-button-warning p-button-sm"
                    onClick={() => editar(rowData)}
                />

                <Button
                    icon="pi pi-trash"
                    className="p-button-danger p-button-sm"
                    onClick={() => remover(rowData.id)}
                />
            </div>
        );
    };

    return (
        <>
            <Toast ref={toast} />
            <ConfirmDialog />

            <div className="flex justify-content-center mt-5">
                <Card title="Cadastro de Pessoas" className="w-full md:w-6">

                    {editandoId && (
                        <div className="mb-3 text-orange-500 font-semibold">
                            <i className="pi pi-pencil mr-2"></i>
                            Editando pessoa...
                        </div>
                    )}

                    <div className="field mb-3">
                        <label className="block mb-2">Nome</label>
                        <InputText
                            value={nome}
                            onChange={(e) => setNome(e.target.value)}
                            className="w-full"
                            placeholder="Digite o nome"
                        />
                    </div>

                    <div className="field mb-4">
                        <label className="block mb-2">Idade</label>
                        <InputNumber
                            value={idade}
                            onValueChange={(e) => setIdade(e.value ?? 0)}
                            useGrouping={false}
                            className="w-full"
                            placeholder="Digite a idade"
                        />
                    </div>

                    <div className="flex justify-content-end gap-2 mb-4">
                        {editandoId && (
                            <Button
                                label="Cancelar"
                                icon="pi pi-times"
                                className="p-button-secondary ml-2"
                                onClick={() => {
                                    setEditandoId(null);
                                    setNome("");
                                    setIdade(null);
                                }}
                            />
                        )}
                        <Button
                            label={editandoId ? "Atualizar" : "Salvar"}
                            icon={editandoId ? "pi pi-refresh" : "pi pi-check"}
                            onClick={submit}
                            className="p-button-success"
                        />
                    </div>

                    <DataTable value={pessoas} paginator rows={10} className="p-datatable-sm">
                        <Column field="nome" header="Nome" />
                        <Column header="Idade" body={formatacaoIdade} />
                        <Column header="Ações" body={acoesTemplate} style={{ width: "120px" }} />
                    </DataTable>

                </Card>
            </div>
        </>
    );
}