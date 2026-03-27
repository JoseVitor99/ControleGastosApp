/**
 * Página de cadastro e listagem de categorias.
 *
 * Responsável por:
 * - Criar novas categorias
 * - Listar categorias cadastradas
 *
 * Cada categoria possui:
 * - Descrição
 * - Finalidade (Receita, Despesa ou Ambas)
 *
 * Integra com a API:
 * - GET  /categorias
 * - POST /categorias
 */

import { useEffect, useState } from "react"
import { api } from "../services/api"
import ICategorias from "../interfaces/ICategorias"
import { FINALIDADE_CATEGORIA } from "../utilities/constants"
import { Toast } from "primereact/toast";
import { useRef } from "react";

import { InputText } from "primereact/inputtext";
import { Dropdown } from "primereact/dropdown";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";

export default function Categorias() {
    const toast = useRef<Toast>(null);

    const [categorias, setCategorias] = useState<ICategorias[]>([]);
    const [descricao, setDescricao] = useState("");
    const [finalidade, setFinalidade] = useState<number | null>(null);

    /**
     * Busca todas as categorias cadastradas
     * e atualiza a tabela
     */
    const load = async () => {
        const getCategoria = await api.get("/categorias")
        setCategorias(getCategoria.data || [])
    };

    /**
     * Realiza o cadastro de uma nova categoria
     * e atualiza a listagem após sucesso
     */
    const submit = async () => {
        try {
            await api.post("/categorias", { descricao, finalidade });

            toast.current?.show({
                severity: "success",
                summary: "Sucesso",
                detail: "Cadastro de categoria salvo com sucesso!",
                life: 3000
            });

            setDescricao("");
            setFinalidade(null);
            load();

        } catch (err: any) {

            const mensagem = err.response?.data || "Erro ao salvar categoria.";

            toast.current?.show({
                severity: "error",
                summary: "Erro",
                detail: mensagem,
                life: 4000
            });
        }
    };

    useEffect(() => {
        load();
    }, []);

    /**
     * Converte o valor numérico da finalidade para a descrição que é devida
     */
    const formatacaoFinalidade = (rowData: ICategorias) => {
        const item = FINALIDADE_CATEGORIA.find(f => f.value === rowData.finalidade);
        return item?.descricao;
    };

    return (
        <>
            <Toast ref={toast} />

            <div className="flex justify-content-center mt-5">
                <Card title="Cadastro de Categorias" className="w-full md:w-6">

                    <div className="field mb-3">
                        <label className="block mb-2">Descrição</label>
                        <InputText
                            value={descricao}
                            onChange={(e) => setDescricao(e.target.value)}
                            className="w-full"
                            placeholder="Digite a descrição"
                        />
                    </div>

                    <div className="field mb-4">
                        <label className="block mb-2">Finalidade</label>
                        <Dropdown
                            value={finalidade}
                            options={FINALIDADE_CATEGORIA}
                            onChange={(e) => setFinalidade(e.value)}
                            optionLabel="descricao"
                            optionValue="value"
                            placeholder="Selecione a finalidade"
                            className="w-full"
                        />
                    </div>

                    <div className="flex justify-content-end mb-4">
                        <Button
                            label="Salvar"
                            icon="pi pi-check"
                            onClick={submit}
                            className="p-button-success"
                        />
                    </div>

                    <DataTable value={categorias} paginator rows={10} className="p-datatable-sm">
                        <Column field="descricao" header="Descrição" />
                        <Column header="Finalidade" body={formatacaoFinalidade} />
                    </DataTable>

                </Card>
            </div>
        </>
    );
}