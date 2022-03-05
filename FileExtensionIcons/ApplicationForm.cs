using System.Runtime.InteropServices;

namespace FileExtensionIcons
{
    public partial class ApplictionForm : Form
    {
        public ApplictionForm()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            pictureBox.Image?.Dispose();

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                pictureBox.Image = null;
                return;
            }

            pictureBox.Image = FileExtensionIcons.GetFileIcon(textBox.Text, true);
        }
    }
}