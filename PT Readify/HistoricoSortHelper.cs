using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PT_Readify
{
    internal class HistoricoSortHelper
    {
        private static readonly Color CorAtiva = Color.FromArgb(39, 174, 96);
        private static readonly Color CorInativa = Color.FromArgb(46, 204, 113);

        private readonly DataGridView _grid;
        private readonly Guna2Button _btnListar;
        private readonly Guna2Button _btnDesc;
        private readonly Guna2Button _btnAsc;
        private readonly string _colunaData;

        private DataTable _dados;
        private string _ordenacaoAtual;

        public HistoricoSortHelper(
            DataGridView grid,
            Guna2Button btnListar,
            Guna2Button btnDesc,
            Guna2Button btnAsc,
            string colunaData)
        {
            _grid = grid;
            _btnListar = btnListar;
            _btnDesc = btnDesc;
            _btnAsc = btnAsc;
            _colunaData = colunaData;

            _btnDesc.Visible = false;
            _btnAsc.Visible = false;
        }

        public void DefinirDados(DataTable dados)
        {
            _dados = dados;
            _ordenacaoAtual = null;
            _grid.DataSource = _dados;
            ReporBotoes();
        }

        public void MostrarOpcoesOrdenacao()
        {
            if (_dados == null || !_dados.Columns.Contains(_colunaData))
            {
                MessageBox.Show(
                    "Não existem dados com datas para ordenar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            _btnListar.Visible = false;
            _btnDesc.Visible = true;
            _btnAsc.Visible = true;
            _btnDesc.BringToFront();
            _btnAsc.BringToFront();
            DestacarBotoes(null);
        }

        public void OrdenarDecrescente() => AplicarOrdenacao("DESC");

        public void OrdenarCrescente() => AplicarOrdenacao("ASC");

        private void AplicarOrdenacao(string direcao)
        {
            if (_dados == null)
            {
                MessageBox.Show("Não há dados para ordenar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReporBotoes();
                return;
            }

            if (!_dados.Columns.Contains(_colunaData))
            {
                MessageBox.Show(
                    $"Coluna de data \"{_colunaData}\" não encontrada.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ReporBotoes();
                return;
            }

            _ordenacaoAtual = direcao;

            if (_dados.Rows.Count > 0)
            {
                var linhas = _dados.AsEnumerable();
                linhas = direcao == "DESC"
                    ? linhas.OrderByDescending(r => ObterData(r))
                    : linhas.OrderBy(r => ObterData(r));

                DataTable ordenada = _dados.Clone();
                foreach (DataRow row in linhas)
                    ordenada.ImportRow(row);

                _dados = ordenada;
            }

            _grid.DataSource = _dados;
            ReporBotoes();
            DestacarBotoes(direcao);
        }

        private DateTime ObterData(DataRow row)
        {
            object valor = row[_colunaData];
            if (valor == null || valor == DBNull.Value)
                return DateTime.MinValue;
            if (valor is DateTime data)
                return data;
            return DateTime.TryParse(valor.ToString(), out data) ? data : DateTime.MinValue;
        }

        private void ReporBotoes()
        {
            _btnListar.Visible = true;
            _btnDesc.Visible = false;
            _btnAsc.Visible = false;
            _btnListar.BringToFront();

            if (_ordenacaoAtual == "DESC")
                _btnListar.Text = "Datas: mais recentes";
            else if (_ordenacaoAtual == "ASC")
                _btnListar.Text = "Datas: mais antigas";
            else
                _btnListar.Text = "Listar por Datas";
        }

        private void DestacarBotoes(string direcaoAtiva)
        {
            _btnDesc.FillColor = direcaoAtiva == "DESC" ? CorAtiva : CorInativa;
            _btnAsc.FillColor = direcaoAtiva == "ASC" ? CorAtiva : CorInativa;
        }
    }
}
