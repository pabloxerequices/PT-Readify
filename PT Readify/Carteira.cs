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
        private Config _config;

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
            _config = ConfigManager.Current;
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
            _config = ConfigManager.Current;
            ApplyConfig(_config);
            ApplyLanguage();

            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show(
                    LanguageHelper.T("WalletAccess", _config) + " - " + LanguageHelper.T("PleaseEnterPassword", _config),
                    LanguageHelper.T("ValidationWarning", _config),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            CarteiraService.CarregarParaUtilizador(globais.id_utilizador);
            ConfigurarBotaoAdicionarFundos();
            AtualizarSaldo();
            
            // Load password from database if it exists
            passwordUtilizador = BLL.Carteira.ObterPasswordCarteira(globais.id_utilizador);
            
            if (!BLL.Carteira.TemPasswordDefinida(globais.id_utilizador))
            {
                SolicitarPasswordUtilizador();
            }
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
                Location = new Point(20, 120),
                Name = "btnAdicionarFundos",
                Size = new Size(310, 35),
                TabIndex = 3,
                Text = LanguageHelper.T("AddFunds", _config)
            };
            btnAdicionarFundos.Click += BtnAdicionarFundos_Click;
            panelCarteira.Controls.Add(btnAdicionarFundos);
            btnAlterarPagamento.Location = new Point(20, 170);
        }

        private void BtnAdicionarFundos_Click(object sender, EventArgs e)
        {
            using (Form promptForm = new Form())
            {
                promptForm.Text = LanguageHelper.T("AddFunds", _config);
                promptForm.Width = 420;
                promptForm.Height = 280;
                promptForm.StartPosition = FormStartPosition.CenterParent;
                promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                promptForm.MaximizeBox = false;
                promptForm.MinimizeBox = false;
                promptForm.BackColor = Color.WhiteSmoke;

                Label lblInstrucao = new Label
                {
                    Text = LanguageHelper.T("EnterAmount", _config),
                    Left = 20,
                    Top = 20,
                    Width = 360,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };
                promptForm.Controls.Add(lblInstrucao);

                Label lblSaldoAtual = new Label
                {
                    Text = string.Format(LanguageHelper.T("CurrentBalanceLabel", _config), CarteiraService.Saldo),
                    Left = 20,
                    Top = 45,
                    Width = 360,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Gray
                };
                promptForm.Controls.Add(lblSaldoAtual);

                TextBox txtValor = new TextBox
                {
                    Left = 20,
                    Top = 75,
                    Width = 360,
                    Height = 35,
                    Font = new Font("Segoe UI", 12F),
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = "0.00"
                };
                promptForm.Controls.Add(txtValor);

                Label lblInfo = new Label
                {
                    Text = LanguageHelper.T("MinValue", _config),
                    Left = 20,
                    Top = 115,
                    Width = 360,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.DarkGray
                };
                promptForm.Controls.Add(lblInfo);

                Button btnConfirmar = new Button
                {
                    Text = LanguageHelper.T("Confirm", _config),
                    Left = 90,
                    Top = 150,
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
                    Text = LanguageHelper.T("Cancel", _config),
                    Left = 220,
                    Top = 150,
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
                txtValor.SelectAll();
                txtValor.Focus();

                if (promptForm.ShowDialog(this) != DialogResult.OK)
                    return;

                string textoValor = txtValor.Text.Trim().Replace(',', '.');
                if (!decimal.TryParse(textoValor, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal valor)
                    || valor <= 0)
                {
                    MessageBox.Show(
                        LanguageHelper.T("EnterValidValue", _config),
                        LanguageHelper.T("InvalidValue", _config),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (valor < 1.00m)
                {
                    MessageBox.Show(
                        LanguageHelper.T("MinValueError", _config),
                        LanguageHelper.T("InvalidValue", _config),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (valor > 1000.00m)
                {
                    MessageBox.Show(
                        LanguageHelper.T("MaxValueError", _config),
                        LanguageHelper.T("InvalidValue", _config),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                decimal novoSaldo = CarteiraService.Saldo + valor;
                DialogResult confirmacao = MessageBox.Show(
                    string.Format(LanguageHelper.T("ConfirmLoadingMsg", _config), valor, CarteiraService.Saldo, novoSaldo),
                    LanguageHelper.T("ConfirmLoading", _config),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacao != DialogResult.Yes)
                    return;

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
            lblSaldo.Text = string.Format(LanguageHelper.T("CurrentBalance", _config), CarteiraService.Saldo);
        }

        private void ApplyLanguage()
        {
            if (_config == null) _config = ConfigManager.Current;

            // Header
            lblTitulo.Text = LanguageHelper.T("WalletTitle", _config);
            btnFechar.Text = LanguageHelper.T("Close", _config);

            // Autenticação
            lblTituloAuth.Text = LanguageHelper.T("WalletAccess", _config);
            lblPasswordLabel.Text = LanguageHelper.T("EnterPassword", _config);
            btnEntrar.Text = LanguageHelper.T("Enter", _config);

            // Carteira
            lblTituloCarteira.Text = LanguageHelper.T("MyWallet", _config);
            btnAlterarPagamento.Text = LanguageHelper.T("ChangePaymentMethod", _config);

            // Edição
            lblTituloEdicao.Text = LanguageHelper.T("ConfigurePayment", _config);
            lblSelecione.Text = LanguageHelper.T("SelectMethod", _config);
            lblEmailPayPal.Text = LanguageHelper.T("PayPalEmail", _config);
            lblTelemovelMBWay.Text = LanguageHelper.T("PhoneNumber", _config);
            lblIbanTransferencia.Text = LanguageHelper.T("AccountIBAN", _config);
            lblAppleID.Text = LanguageHelper.T("AppleID", _config);
            btnSalvar.Text = LanguageHelper.T("SaveChanges", _config);
            btnVoltar.Text = LanguageHelper.T("Back", _config);

            if (btnAdicionarFundos != null)
                btnAdicionarFundos.Text = LanguageHelper.T("AddFunds", _config);
        }

        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;
            ConfigApplier.ApplyFont(this, cfg);
            ConfigApplier.ApplyFont(panelAutenticacao, cfg);
            ConfigApplier.ApplyFont(panelCarteira, cfg);
            ConfigApplier.ApplyFont(panelEdicao, cfg);
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
                    promptForm.Text = LanguageHelper.T("DefinePassword", _config);
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
                        Text = LanguageHelper.T("DefinePasswordMsg", _config),
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
                        Text = LanguageHelper.T("PasswordRequirements", _config),
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
                        Text = LanguageHelper.T("Confirm", _config),
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
                        Text = LanguageHelper.T("Exit", _config),
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
                            BLL.Carteira.DefinirPasswordCarteira(globais.id_utilizador, passwordUtilizador);
                            MessageBox.Show(
                                LanguageHelper.T("PasswordDefined", _config),
                                LanguageHelper.T("Success", _config),
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
                    LanguageHelper.T("PasswordCannotBeEmpty", _config),
                    LanguageHelper.T("ValidationWarning", _config),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            if (password.Length < 4)
            {
                MessageBox.Show(
                    LanguageHelper.T("PasswordMinChars", _config),
                    LanguageHelper.T("ValidationWarning", _config),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            if (password.Length > 20)
            {
                MessageBox.Show(
                    LanguageHelper.T("PasswordMaxChars", _config),
                    LanguageHelper.T("ValidationWarning", _config),
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
                promptForm.Text = LanguageHelper.T("ChangePassword", _config);
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
                    Text = LanguageHelper.T("CurrentPassword", _config),
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
                    Text = LanguageHelper.T("NewPassword", _config),
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
                    Text = LanguageHelper.T("Change", _config),
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
                            LanguageHelper.T("CurrentPasswordIncorrect", _config),
                            LanguageHelper.T("InvalidValue", _config),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }

                    if (ValidarPasswordDefinicao(txtPasswordNova.Text))
                    {
                        passwordUtilizador = BLL.utilizador.HashPassword(txtPasswordNova.Text);
                        BLL.Carteira.DefinirPasswordCarteira(globais.id_utilizador, passwordUtilizador);
                        MessageBox.Show(
                            LanguageHelper.T("PasswordChanged", _config),
                            LanguageHelper.T("Success", _config),
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
                    Text = LanguageHelper.T("Cancel", _config),
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
                    LanguageHelper.T("PleaseEnterPassword", _config),
                    LanguageHelper.T("ValidationWarning", _config),
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
                    LanguageHelper.T("WelcomeToWallet", _config),
                    LanguageHelper.T("AuthenticationSuccessful", _config),
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
                        LanguageHelper.T("AttemptsExceeded", _config),
                        LanguageHelper.T("AccessBlocked", _config),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop
                    );
                    this.Close();
                }
                else
                {
                    // Avisar do número de tentativas restantes
                    MessageBox.Show(
                        string.Format(LanguageHelper.T("PasswordIncorrect", _config), tentativasRestantes),
                        LanguageHelper.T("AuthenticationError", _config),
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
                LanguageHelper.T("AreYouSureChangePassword", _config),
                LanguageHelper.T("ConfirmChange", _config),
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
                    LanguageHelper.T("SelectPaymentMethod", _config),
                    LanguageHelper.T("ValidationWarning", _config),
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
                    string.Format(LanguageHelper.T("MethodConfigured", _config), metodoSelecionado, valorIntroduzido),
                    LanguageHelper.T("Success", _config),
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
                    LanguageHelper.T("FillPaymentField", _config),
                    LanguageHelper.T("ValidationWarning", _config),
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
                MessageBox.Show(LanguageHelper.T("InsufficientBalance", _config), LanguageHelper.T("InvalidValue", _config), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                CarteiraService.Debitar(preco);
                MessageBox.Show(string.Format(LanguageHelper.T("PurchaseSuccessful", _config), titulo), LanguageHelper.T("Success", _config), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string.Format(LanguageHelper.T("FundsAdded", _config), valor, CarteiraService.Saldo),
                    LanguageHelper.T("Success", _config),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show(LanguageHelper.T("ValueMustBePositive", _config), LanguageHelper.T("InvalidValue", _config), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
