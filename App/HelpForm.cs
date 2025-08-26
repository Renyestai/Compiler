using System;
using System.IO;
using System.Windows.Forms;

namespace TFCLab1
{
	public partial class HelpForm : Form
	{
		public HelpForm()
		{
			InitializeComponent();
			FormBorderStyle = FormBorderStyle.FixedSingle; // Запрещает изменение размеров формы
			MaximizeBox = false; // Скрывает кнопку "Развернуть на весь экран"
		}

		private void Help_Load(object sender, EventArgs e)
		{
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string htmlFilePath = Path.Combine(baseDir, "Docs", "ShowHelpHTMLPage.html");

			if (System.IO.File.Exists(htmlFilePath))
			{
				webBrowserHelp.Navigate(htmlFilePath);
			}
			else
			{
				MessageBox.Show($"Файл справки не найден: {htmlFilePath}",
					"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void webBrowserHelp_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

		}
	}
}
