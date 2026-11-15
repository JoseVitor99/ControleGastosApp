interface ITransacoes {
  id?: number;
  descricao: string;
  valor: number;
  tipo: number;
  pessoaId: number;
  categoriaId: number;
}

export default ITransacoes;