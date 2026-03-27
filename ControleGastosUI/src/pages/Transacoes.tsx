/**
 * Página de cadastro de transações financeiras.
 *
 * Responsável por:
 * - Registrar novas transações (receita ou despesa)
 * - Associar transações a uma pessoa e uma categoria
 *
 * Integra com a API:
 * - GET  /pessoas
 * - GET  /categorias
 * - POST /transacoes
 */

import { useEffect, useState } from "react";
import { Toast } from "primereact/toast";
import { useRef } from "react";
import { api } from "../services/api";
import { TIPO_TRANSACAO, FINALIDADE_CATEGORIA } from "../utilities/constants";

import { InputText } from "primereact/inputtext";
import { InputNumber } from "primereact/inputnumber";
import { Dropdown } from "primereact/dropdown";
import { Button } from "primereact/button";
import { Card } from "primereact/card";

export default function Transacoes() {
    const toast = useRef<Toast>(null);

    const [descricao, setDescricao] = useState("");
    const [valor, setValor] = useState<number | null>(0);
    const [tipo, setTipo] = useState(1);
    const [pessoaId, setPessoaId] = useState<number | null>(null);
    const [categoriaId, setCategoriaId] = useState<number | null>(null);
    const [pessoas, setPessoas] = useState<any[]>([]);
    const [categorias, setCategorias] = useState<any[]>([]);

    /**
     * Carrega pessoas e categorias necessárias para o cadastro da transação
     */
    const load = async () => {
        const getCategoria = await api.get("/categorias");
        setCategorias(getCategoria.data || []);

        const getPessoa = await api.get("/pessoas");
        setPessoas(getPessoa.data || []);
    };

    /**
     * Envia os dados da transação para a API
     */
    const submit = async () => {
        if (!descricao.trim()) {
            toast.current?.show({
                severity: "warn",
                summary: "Atenção",
                detail: "Informe a descrição.",
                life: 3000
            });
            return;
        }

        if (!valor || valor <= 0) {
            toast.current?.show({
                severity: "warn",
                summary: "Atenção",
                detail: "Informe um valor válido.",
                life: 3000
            });
            return;
        }

        if (!pessoaId) {
            toast.current?.show({
                severity: "warn",
                summary: "Atenção",
                detail: "Selecione uma pessoa.",
                life: 3000
            });
            return;
        }

        if (!categoriaId) {
            toast.current?.show({
                severity: "warn",
                summary: "Atenção",
                detail: "Selecione uma categoria.",
                life: 3000
            });
            return;
        }

        try {
            await api.post("/transacoes", {
                descricao,
                valor,
                tipo,
                pessoaId,
                categoriaId
            })

            toast.current?.show({
                severity: "success",
                summary: "Sucesso",
                detail: "Transação salva com sucesso!",
                life: 3000
            });

        } catch (err: any) {

            const mensagem = err.response?.data || "Erro ao salvar transação.";

            toast.current?.show({
                severity: "error",
                summary: "Erro",
                detail: mensagem,
                life: 4000
            });

            console.log(err);
        }
    };

    useEffect(() => {
        load();
    }, []);

    /**
     * Customiza a exibição das categorias no dropdown,
     * apresentando "Descrição - Finalidade".
     *
     * Trata dois cenários:
     * 1- Quando recebe apenas o valor (id)
     * 2- Quando recebe o objeto completo (renderização da lista)
     */
    const formatacaoCategoria = (option: any) => {
        if (!option || typeof option !== "object") {
            const categoria = categorias.find(c => c.id === option);

            if (!categoria) return <span>Selecione uma categoria</span>;

            const finalidadeDesc = FINALIDADE_CATEGORIA.find(
                f => f.value === categoria.finalidade
            );

            return (
                <span>
                    {categoria.descricao} - {finalidadeDesc?.descricao}
                </span>
            );
        }

        const finalidadeDesc = FINALIDADE_CATEGORIA.find(
            f => f.value === option.finalidade
        );

        return (
            <span>
                {option.descricao} - {finalidadeDesc?.descricao}
            </span>
        );
    };

    return (
        <>
            <Toast ref={toast} />

            <div className="flex justify-content-center mt-5">
                <Card title="Cadastro de Transações" className="w-full md:w-6">

                    <div className="field mb-3">
                        <label className="block mb-2">Descrição</label>
                        <InputText
                            value={descricao}
                            onChange={(e) => setDescricao(e.target.value)}
                            className="w-full"
                            placeholder="Digite a descrição"
                        />
                    </div>

                    <div className="field mb-3">
                        <label className="block mb-2">Valor</label>
                        <InputNumber
                            value={valor}
                            onValueChange={(e) => setValor(e.value ?? 0)}
                            mode="currency"
                            currency="BRL"
                            locale="pt-BR"
                            className="w-full"
                        />
                    </div>

                    <div className="field mb-3">
                        <label className="block mb-2">Tipo</label>
                        <Dropdown
                            value={tipo}
                            options={TIPO_TRANSACAO}
                            onChange={(e) => setTipo(e.value)}
                            optionLabel="descricao"
                            optionValue="value"
                            placeholder="Selecione o tipo"
                            className="w-full"
                        />
                    </div>

                    <div className="field mb-3">
                        <label className="block mb-2">Pessoa</label>
                        <Dropdown
                            value={pessoaId}
                            options={pessoas}
                            onChange={(e) => setPessoaId(e.value)}
                            optionLabel="nome"
                            optionValue="id"
                            placeholder="Selecione uma pessoa"
                            className="w-full"
                        />
                    </div>

                    <div className="field mb-4">
                        <label className="block mb-2">Categoria</label>
                        <Dropdown
                            value={categoriaId}
                            options={categorias}
                            onChange={(e) => setCategoriaId(e.value)}
                            optionLabel="descricao"
                            optionValue="id"
                            placeholder="Selecione uma categoria"
                            className="w-full"
                            itemTemplate={formatacaoCategoria}
                        />
                    </div>

                    <div className="flex justify-content-end">
                        <Button
                            label="Salvar"
                            icon="pi pi-check"
                            onClick={submit}
                            className="p-button-success"
                        />
                    </div>

                </Card>
            </div>
        </>
    );
}