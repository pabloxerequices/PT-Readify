using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Chat_Bot : Form
    {
        private static readonly Color UserColor = Color.FromArgb(26, 58, 92);
        private static readonly Color BotColor = Color.FromArgb(72, 133, 237);

        private readonly ChatConversationEngine _conversation = new ChatConversationEngine();
        private bool _isReplying;

        public Chat_Bot()
        {
            InitializeComponent();
            Load += Chat_Bot_Load;
            KeyPreview = true;
            KeyDown += Chat_Bot_KeyDown;
            guna2TextBox1.KeyDown += Guna2TextBox1_KeyDown;
            guna2Button1.Click += (s, e) => SendMessage(guna2TextBox1.Text);
            guna2Button2.Click += (s, e) => EndConversation();
        }

        private void Chat_Bot_Load(object sender, EventArgs e)
        {
            var cfg = ConfigManager.Current;
            ApplyConfig(cfg);
            Text = "Assistente PT Readify";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            richTextBox1.Clear();
            AppendMessage("Assistente", _conversation.GetOpeningMessage(), BotColor);
        }

        private void Chat_Bot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                EndConversation();
            }
        }

        private void Guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendMessage(guna2TextBox1.Text);
            }
        }

        private async void SendMessage(string text)
        {
            text = text?.Trim();
            if (string.IsNullOrEmpty(text) || _isReplying)
                return;

            _isReplying = true;
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            guna2TextBox1.Enabled = false;

            AppendMessage("Você", text, UserColor);
            guna2TextBox1.Clear();

            await Task.Delay(350);

            var response = _conversation.Reply(text);
            AppendMessage("Assistente", response, BotColor);

            if (_conversation.ShouldEndConversation)
            {
                await Task.Delay(800);
                Close();
                return;
            }

            guna2TextBox1.Enabled = true;
            guna2Button1.Enabled = true;
            guna2Button2.Enabled = true;
            guna2TextBox1.Focus();
            _isReplying = false;
        }

        private async void EndConversation(bool withGoodbye = false)
        {
            if (_isReplying)
                return;

            if (withGoodbye)
            {
                _isReplying = true;
                AppendMessage("Você", "Adeus", UserColor);
                await Task.Delay(350);
                AppendMessage("Assistente", _conversation.Reply("adeus"), BotColor);
                await Task.Delay(800);
            }

            Close();
        }

        private void AppendMessage(string sender, string message, Color color)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = color;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText(sender + ": ");
            richTextBox1.SelectionFont = richTextBox1.Font;
            richTextBox1.AppendText(message + Environment.NewLine + Environment.NewLine);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
            richTextBox1.ScrollToCaret();
        }

        private void Chat_Bot_Load_1(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;
            ConfigApplier.ApplyFont(this, cfg);
        }
    }
}
