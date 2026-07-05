using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BusinessLogicLayer;
using Guna.UI2.WinForms;

namespace PT_Readify
{
    public partial class Carteira : Form
    {
        // --- VARIÁVEIS DE CONTROLO DE ESTADO ---
        private string passwordUtilizador = string.Empty;

        // Variáveis para limite de tentativas
        private int tentativasFalhadas = 0;
        private const int MAX_TENTATIVAS = 3;

        // Variável para guardar método selecionado
        private string metodoAtualSelecionado = string.Empty;
        private Guna2Button btnAdicionarFundos;

        public Carteira()
        {
            InitializeComponent();
            CarteiraService.SaldoAlterado += OnSaldoAlterado;
            FormClosed += (s, e) => CarteiraService.SaldoAlterado -= OnSaldoAlterado;
        }

        private void OnSaldoAlterado()
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action(AtualizarSaldo));
            else
                AtualizarSaldo();
        }

        private void Carteira_Load(object sender, EventArgs e)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show(
                    "Inicie sessão para aceder à Carteira Digital.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            CarteiraService.CarregarParaUtilizador(globais.id_utilizador);
            ConfigurarBotaoAdicionarFundos();
            AtualizarSaldo();
            SolicitarPasswordUtilizador();
        }

        private void ConfigurarBotaoAdicionarFundos()
        {
            if (btnAdicionarFundos != null)
                return;

            btnAdicionarFundos = new Guna2Button
            {
                BorderRadius = 8,
                FillColor = Color.FromArgb(46, 204, 113),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(60, 200),
                Name = "btnAdicionarFundos",
                Size = new Size(380, 45),
                TabIndex = 3,
                Text = "Adicionar Fundos"
            };
            btnAdicionarFundos.Click += BtnAdicionarFundos_Click;
            panelCarteira.Controls.Add(btnAdicionarFundos);
            btnAlterarPagamento.Location = new Point(60, 260);
        }

        private void BtnAdicionarFundos_Click(object sender, EventArgs e)
        {
            using (Form promptForm = new Form())
            {
                promptForm.Text = "Adicionar Fundos";
                promptForm.Width = 420;
                promptForm.Height = 220;
                promptForm.StartPosition = FormStartPosition.CenterParent;
                promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                promptForm.MaximizeBox = false;
                promptForm.MinimizeBox = false;
                promptForm.BackColor = Color.WhiteSmoke;

                Label lblInstrucao = new Label
                {
                    Text = "Introduza o valor a carregar na carteira (€):",
                    Left = 20,
                    Top = 20,
                    Width = 360,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };
                promptForm.Controls.Add(lblInstrucao);

                TextBox txtValor = new TextBox
                {
                    Left = 20,
                    Top = 55,
                    Width = 360,
                    Height = 35,
                    Font = new Font("Segoe UI", 12F),
                    BorderStyle = BorderStyle.FixedSingle
                };
                promptForm.Controls.Add(txtValor);

                Button btnConfirmar = new Button
                {
                    Text = "CONFIRMAR",
                    Left = 90,
                    Top = 115,
                    Width = 110,
                    Height = 35,
                    BackColor = Color.FromArgb(46, 204, 113),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    DialogResult = DialogResult.OK
                };
                promptForm.Controls.Add(btnConfirmar);

                Button btnCancelar = new Button
                {
                    Text = "CANCELAR",
                    Left = 220,
                    Top = 115,
                    Width = 110,
                    Height = 35,
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    DialogResult = DialogResult.Cancel
                };
                promptForm.Controls.Add(btnCancelar);

                promptForm.AcceptButton = btnConfirmar;
                promptForm.CancelButton = btnCancelar;

                if (promptForm.ShowDialog(this) != DialogResult.OK)
                    return;

                string textoValor = txtValor.Text.Trim().Replace(',', '.');
                if (!decimal.TryParse(textoValor, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal valor)
                    || valor <= 0)
                {
                    MessageBox.Show(
                        "Introduza um valor válido maior que zero.",
                        "Valor inválido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    AdicionarSaldo(valor);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Atualiza a exibição do saldo atual.
        /// </summary>
        private void AtualizarSaldo()
        {
            lblSaldo.Text = $"Saldo Atual: {CarteiraService.Saldo:F2}€";
        }

        /// <summary>
        /// Solicita ao utilizador que defina a sua password de acesso.
        /// Se não definir, a aplicação fecha.
        /// </summary>
        private void SolicitarPasswordUtilizador()
        {
            while (string.IsNullOrEmpty(passwordUtilizador))
            {
                using (Form promptForm = new Form())
                {
                    promptForm.Text = "Definir Password";
                    promptForm.Width = 400;
                    promptForm.Height = 200;
                    promptForm.StartPosition = FormStartPosition.CenterParent;
                    promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    promptForm.MaximizeBox = false;
                    promptForm.MinimizeBox = false;
                    promptForm.BackColor = Color.WhiteSmoke;

                    // ===== LABEL INSTRUÇÃO =====
                    Label lblInstrucao = new Label()
                    {
                        Text = "Defina a sua password de acesso à Carteira Digital:",
                        Left = 20,
                        Top = 20,
                        Width = 400,
                        AutoSize = true,
                        Font = new Font("Arial", 10, FontStyle.Bold)
                    };
                    promptForm.Controls.Add(lblInstrucao);

                    // ===== TEXTBOX PASSWORD =====
                    TextBox txtPassword = new TextBox()
                    {
                        Left = 20,
                        Top = 60,
                        Width = 400,
                        Height = 35,
                        PasswordChar = '*',
                        Font = new Font("Arial", 12),
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    promptForm.Controls.Add(txtPassword);

                    // ===== LABEL REQUISITOS =====
                    Label lblRequisitos = new Label()
                    {
                        Text = "• Mínimo 4 caracteres\n• Máximo 20 caracteres\n• Pode conter letras, números e símbolos",
                        Left = 20,
                        Top = 105,
                        Width = 400,
                        AutoSize = true,
                        Font = new Font("Arial", 9),
                        ForeColor = Color.DarkBlue
                    };
                    promptForm.Controls.Add(lblRequisitos);

                    // ===== BOTÃO OK =====
                    Button btnOK = new Button()
                    {
                        Text = "CONFIRMAR",
                        Left = 170,
                        Top = 185,
                        Width = 110,
                        Height = 35,
                        BackColor = Color.DodgerBlue,
                        ForeColor = Color.White,
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        DialogResult = DialogResult.OK
                    };
                    promptForm.Controls.Add(btnOK);

                    // ===== BOTÃO SAIR =====
                    Button btnSair = new Button()
                    {
                        Text = "SAIR",
                        Left = 290,
                        Top = 185,
                        Width = 130,
                        Height = 35,
                        BackColor = Color.Red,
                        ForeColor = Color.White,
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        DialogResult = DialogResult.Cancel
                    };
                    promptForm.Controls.Add(btnSair);

                    promptForm.AcceptButton = btnOK;
                    promptForm.CancelButton = btnSair;

                    if (promptForm.ShowDialog() == DialogResult.OK)
                    {
                        // Valida a password introduzida
                        if (ValidarPasswordDefinicao(txtPassword.Text))
                        {
                            passwordUtilizador = BLL.utilizador.HashPassword(txtPassword.Text);
                            MessageBox.Show(
                                "Password definida com sucesso!",
                                "Sucesso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                    }
                    else
                    {
                        // Utilizador clicou em SAIR
                        this.Close();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Valida a password na altura da definição.
        /// Verifica se não está vazia e cumpre os requisitos mínimos.
        /// </summary>
        private bool ValidarPasswordDefinicao(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "A password não pode estar vazia. Tente novamente.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            if (password.Length < 4)
            {
                MessageBox.Show(
                    "A password deve ter um mínimo de 4 caracteres.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            if (password.Length > 20)
            {
                MessageBox.Show(
                    "A password deve ter um máximo de 20 caracteres.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Permite ao utilizador alterar a sua password existente.
        /// Exige a password antiga para confirmar.
        /// </summary>
        private void AlterarPassword()
        {
            using (Form promptForm = new Form())
            {
                promptForm.Text = "Alterar Password";
                promptForm.Width = 450;
                promptForm.Height = 280;
                promptForm.StartPosition = FormStartPosition.CenterScreen;
                promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                promptForm.MaximizeBox = false;
                promptForm.MinimizeBox = false;
                promptForm.BackColor = Color.WhiteSmoke;

                // ===== LABEL PASSWORD ANTIGA =====
                Label lblPasswordAntiga = new Label()
                {
                    Text = "Password Atual:",
                    Left = 20,
                    Top = 20,
                    AutoSize = true,
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                promptForm.Controls.Add(lblPasswordAntiga);

                // ===== TEXTBOX PASSWORD ANTIGA =====
                TextBox txtPasswordAntiga = new TextBox()
                {
                    Left = 20,
                    Top = 45,
                    Width = 400,
                    Height = 30,
                    PasswordChar = '*',
                    Font = new Font("Arial", 11),
                    BorderStyle = BorderStyle.FixedSingle
                };
                promptForm.Controls.Add(txtPasswordAntiga);

                // ===== LABEL PASSWORD NOVA =====
                Label lblPasswordNova = new Label()
                {
                    Text = "Nova Password:",
                    Left = 20,
                    Top = 90,
                    AutoSize = true,
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                promptForm.Controls.Add(lblPasswordNova);

                // ===== TEXTBOX PASSWORD NOVA =====
                TextBox txtPasswordNova = new TextBox()
                {
                    Left = 20,
                    Top = 115,
                    Width = 400,
                    Height = 30,
                    PasswordChar = '*',
                    Font = new Font("Arial", 11),
                    BorderStyle = BorderStyle.FixedSingle
                };
                promptForm.Controls.Add(txtPasswordNova);

                // ===== BOTÃO ALTERAR =====
                Button btnAlterar = new Button()
                {
                    Text = "ALTERAR",
                    Left = 170,
                    Top = 165,
                    Width = 110,
                    Height = 35,
                    BackColor = Color.Green,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnAlterar.Click += (s, e) =>
                {
                    if (!BLL.utilizador.VerificarPassword(passwordUtilizador, txtPasswordAntiga.Text))
                    {
                        MessageBox.Show(
                            "Password atual incorreta!",
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }

                    if (ValidarPasswordDefinicao(txtPasswordNova.Text))
                    {
                        passwordUtilizador = BLL.utilizador.HashPassword(txtPasswordNova.Text);
                        MessageBox.Show(
                            "Password alterada com sucesso!",
                            "Sucesso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        promptForm.Close();
                    }
                };
                promptForm.Controls.Add(btnAlterar);

                // ===== BOTÃO CANCELAR =====
                Button btnCancelar = new Button()
                {
                    Text = "CANCELAR",
                    Left = 290,
                    Top = 165,
                    Width = 130,
                    Height = 35,
                    BackColor = Color.Gray,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    DialogResult = DialogResult.Cancel
                };
                promptForm.Controls.Add(btnCancelar);
                promptForm.CancelButton = btnCancelar;

                promptForm.ShowDialog();
            }
        }

        // -----------------------------------------------------------------
        // --- LÓGICA DE EVENTOS ---
        // -----------------------------------------------------------------

        /// <summary>
        /// Valida o acesso à carteira através da password introduzida.
        /// Inclui limite de tentativas falhadas.
        /// </summary>
        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(
                    "Por favor, introduza a password.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtPassword.Focus();
                return;
            }

            if (BLL.utilizador.VerificarPassword(passwordUtilizador, txtPassword.Text))
            {
                // ✓ Password correta - Acesso concedido
                tentativasFalhadas = 0;  // Reset contador
                panelAutenticacao.Visible = false;
                panelCarteira.Visible = true;
                txtPassword.Clear();
                MessageBox.Show(
                    "Bem-vindo à sua Carteira Digital!",
                    "Autenticação Bem-Sucedida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                // ✗ Password incorreta - Incrementa contador
                tentativasFalhadas++;
                int tentativasRestantes = MAX_TENTATIVAS - tentativasFalhadas;

                if (tentativasRestantes <= 0)
                {
                    // Bloqueado por excesso de tentativas
                    MessageBox.Show(
                        "Tentativas de acesso excedidas!\nA aplicação será fechada.",
                        "Acesso Bloqueado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop
                    );
                    this.Close();
                }
                else
                {
                    // Avisar do número de tentativas restantes
                    MessageBox.Show(
                        $"Password incorreta!\n\nTentativas restantes: {tentativasRestantes}",
                        "Erro de Autenticação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }

                txtPassword.Focus();
                txtPassword.SelectAll();
            }
        }

        /// <summary>
        /// Altera a navegação do painel da carteira para o painel de edição.
        /// </summary>
        private void BtnAlterarPagamento_Click(object sender, EventArgs e)
        {
            panelCarteira.Visible = false;
            panelEdicao.Visible = true;
            cbMetodosPagamento.SelectedIndex = 0;
        }

        /// <summary>
        /// Volta ao painel da carteira a partir do painel de edição.
        /// </summary>
        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            panelEdicao.Visible = false;
            panelCarteira.Visible = true;
            LimparCamposPagamento();
        }

        /// <summary>
        /// Botão para alterar a password da Carteira.
        /// </summary>
        private void BtnAlterarPassword_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Tem a certeza que deseja alterar a sua password?",
                "Confirmar Alteração",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                AlterarPassword();
            }
        }

        /// <summary>
        /// Controla a visibilidade dos sub-painéis com base no método de pagamento selecionado.
        /// </summary>
        private void CbMetodosPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelPayPal.Visible = false;
            panelMBWay.Visible = false;
            panelTransferencia.Visible = false;
            panelApplePay.Visible = false;

            switch (cbMetodosPagamento.SelectedItem?.ToString())
            {
                case "PayPal":
                    panelPayPal.Visible = true;
                    txtEmailPayPal.Focus();
                    metodoAtualSelecionado = "PayPal";
                    break;
                case "MBWay":
                    panelMBWay.Visible = true;
                    txtTelemovelMBWay.Focus();
                    metodoAtualSelecionado = "MBWay";
                    break;
                case "Transferência Bancária":
                    panelTransferencia.Visible = true;
                    txtIbanTransferencia.Focus();
                    metodoAtualSelecionado = "Transferência Bancária";
                    break;
                case "Apple Pay":
                    panelApplePay.Visible = true;
                    txtAppleID.Focus();
                    metodoAtualSelecionado = "Apple Pay";
                    break;
            }
        }

        /// <summary>
        /// Valida e grava os dados do método de pagamento selecionado.
        /// </summary>
        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            string metodoSelecionado = cbMetodosPagamento.SelectedItem?.ToString();
            bool validado = false;
            string valorIntroduzido = string.Empty;

            if (string.IsNullOrEmpty(metodoSelecionado))
            {
                MessageBox.Show(
                    "Por favor, selecione um método de pagamento.",
                    "Aviso de Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            switch (metodoSelecionado)
            {
                case "PayPal":
                    validado = !string.IsNullOrWhiteSpace(txtEmailPayPal.Text);
                    valorIntroduzido = txtEmailPayPal.Text;
                    break;
                case "MBWay":
                    validado = !string.IsNullOrWhiteSpace(txtTelemovelMBWay.Text);
                    valorIntroduzido = txtTelemovelMBWay.Text;
                    break;
                case "Transferência Bancária":
                    validado = !string.IsNullOrWhiteSpace(txtIbanTransferencia.Text);
                    valorIntroduzido = txtIbanTransferencia.Text;
                    break;
                case "Apple Pay":
                    validado = !string.IsNullOrWhiteSpace(txtAppleID.Text);
                    valorIntroduzido = txtAppleID.Text;
                    break;
            }

            if (validado)
            {
                MessageBox.Show(
                    $"Método [{metodoSelecionado}] configurado com sucesso!\nDados guardados: {valorIntroduzido}",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                panelEdicao.Visible = false;
                panelCarteira.Visible = true;
                LimparCamposPagamento();
            }
            else
            {
                MessageBox.Show(
                    "Por favor, preencha o campo de dados do método de pagamento selecionado.",
                    "Aviso de Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        /// <summary>
        /// Limpa todos os campos de pagamento.
        /// </summary>
        private void LimparCamposPagamento()
        {
            txtEmailPayPal.Clear();
            txtTelemovelMBWay.Clear();
            txtIbanTransferencia.Clear();
            txtAppleID.Clear();
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        public bool ComprarLivro(string titulo, decimal preco)
        {
            if (!CarteiraService.TemSaldoSuficiente(preco))
            {
                MessageBox.Show("Saldo insuficiente para completar a compra.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                CarteiraService.Debitar(preco);
                MessageBox.Show($"Compra bem-sucedida: {titulo}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void AdicionarSaldo(decimal valor)
        {
            try
            {
                CarteiraService.AdicionarSaldo(valor);
                AtualizarSaldo();
                MessageBox.Show(
                    $"Fundos adicionados e guardados com sucesso: +{valor:F2}€\n\nSaldo atual: {CarteiraService.Saldo:F2}€",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("O valor a adicionar deve ser positivo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public decimal ObterSaldo() => CarteiraService.Saldo;

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
