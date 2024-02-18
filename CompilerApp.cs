using System;
using System.Windows.Forms;


namespace TFCLab1
{
	public partial class CompilerApp : Form
	{
		private string filePath; // Путь к текущему открытому файлу

		private bool isFileModified = false; // Был ли изменен файл в окне

		public CompilerApp()

		{
			InitializeComponent();
			FormClosing += new FormClosingEventHandler(CompilerApp_FormClosing);
			inputRichBox.TextChanged += InputRichBox_TextChanged;
			inputRichBox.VScroll += InputRichBox_VScroll;
			inputRichBox.FontChanged += InputRichBox_FontChanged;
			inputRichBox.SelectionChanged += inputRichBox_SelectionChanged;
		}
		private void UndoFile()
		{
			inputRichBox.Undo();
		}

		private void RedoFile()
		{
			inputRichBox.Redo();

		}

		private void InputRichBox_TextChanged(object sender, EventArgs e)
		{
			AppFunctions.FileModifiedNotSaved(ref isFileModified);
			AppFunctions.UpdateLineNumbers(LineNumberTextBox, inputRichBox, ClientRectangle);
			
		}

		private void InputRichBox_VScroll(object sender, EventArgs e)
		{
			LineNumberTextBox.Text = "";
			AppFunctions.UpdateLineNumbers(LineNumberTextBox, inputRichBox,ClientRectangle);
			LineNumberTextBox.Invalidate();
		}

		private void InputRichBox_FontChanged(object sender, EventArgs e)//если будет меняться шрифт
		{
			LineNumberTextBox.Font = inputRichBox.Font;
			inputRichBox.Select();
			AppFunctions.UpdateLineNumbers(LineNumberTextBox, inputRichBox, ClientRectangle);
		}

		private void inputRichBox_SelectionChanged(object sender, EventArgs e)
		{
			int cursorPosition = inputRichBox.SelectionStart;
			int currentLine = inputRichBox.GetLineFromCharIndex(cursorPosition) + 1;
			inputRichBox.Update();
			toolStripStatusLabel1.Text = "Строка: " + currentLine.ToString();
		}


		private void СоздатьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AppFunctions.CreateNewFile(ref filePath, ref isFileModified, inputRichBox);
			toolStripStatusLabel1.Text = "Создан новый файл";
		}

		private void CreateFileBtn_Click(object sender, EventArgs e)
		{
			AppFunctions.CreateNewFile(ref filePath, ref isFileModified, inputRichBox);
		}

		private void ОткрытьToolStripMenuItem_Click(object sender, EventArgs e)
		{	
			AppFunctions.OpenFile(ref filePath,ref isFileModified,inputRichBox);
		}

		private void OpenFileBtn_Click(object sender, EventArgs e)
		{
			AppFunctions.OpenFile(ref filePath, ref isFileModified, inputRichBox);
		}

		private void СохранитьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AppFunctions.SaveFile(ref filePath, ref isFileModified, inputRichBox);
		}

		private void SaveFileBtn_Click(object sender, EventArgs e)
		{
			AppFunctions.SaveFile(ref filePath, ref isFileModified, inputRichBox);

		}

		private void СохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AppFunctions.SaveAsFileDialogue(ref filePath, isFileModified, inputRichBox);
			toolStripStatusLabel1.Text = "Файл сохранен";
		}

		private void ОтменитьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UndoFile();
		}
		private void UndoBtn_Click(object sender, EventArgs e)
		{
			UndoFile();
		}

		private void ПовторитьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RedoFile();
		}

		private void RedoBtn_Click(object sender, EventArgs e)
		{
			RedoFile();
		}

		private void ВырезатьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			inputRichBox.Cut();
		}

		private void CutBtn_Click(object sender, EventArgs e)
		{
			inputRichBox.Cut();
		}

		private void КопироватьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			inputRichBox.Copy();
		}

		private void CopyBtn_Click(object sender, EventArgs e)
		{
			inputRichBox.Copy();
		}

		private void ВставитьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			inputRichBox.Paste();
		}

		private void PasteBtn_Click(object sender, EventArgs e)
		{
			inputRichBox.Paste();
		}

		private void ВыделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
		{
			inputRichBox.SelectAll();
		}

		private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			inputRichBox.Clear();
		}

		private void ВызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AppFunctions.ShowHelp();
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{
			AppFunctions.ShowHelp();
		}

		private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AppFunctions.AboutBox();
		}

		private void InfoBtn_Click(object sender, EventArgs e)
		{
			AppFunctions.AboutBox();
		}

		private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CompilerApp_FormClosing(object sender, FormClosingEventArgs e)
		{
			AppFunctions.ExitApp(ref filePath, ref isFileModified, inputRichBox, e);
		}

	}
}