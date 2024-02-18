using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TFCLab1
{
	internal class AppFunctions
	{
		internal static void AboutBox()
		{
			string aboutMessage = "Название программы: Компилятор\nВерсия: 1.0\nАвтор: Каршиганова Азиза\nГод: 2024";
			MessageBox.Show(aboutMessage, "О программе", MessageBoxButtons.OK);
		}

		internal static void ShowHelp()
		{
			HelpForm helpForm = new HelpForm();
			helpForm.ShowDialog();
		}

		public static void CreateNewFile(ref string filePath, ref bool isFileModified, RichTextBox inputBox)
		{
			if (isFileModified)
			{
				DialogResult result = MessageBox.Show("Предыдущий файл не был сохранен. Хотите сохранить изменения?", "Несохраненные изменения", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

				switch (result)
				{
					case DialogResult.Yes:
						SaveFile(ref filePath, ref isFileModified, inputBox);
						inputBox.Clear();
						break;
					case DialogResult.No:
						inputBox.Clear();
						break;
					default:
						break;
				}
			}
			else
			{
				inputBox.ReadOnly = false;
				inputBox.Enabled = true;
				inputBox.Clear();
			}
		}

		public static void OpenFile(ref string filePath, ref bool isFileModified, RichTextBox inputBox)
		{
			if (isFileModified)
			{
				DialogResult result = MessageBox.Show("Предыдущий файл не был сохранен. Хотите сохранить изменения?", "Несохраненные изменения", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

				switch (result)
				{
					case DialogResult.Yes:
						SaveFile(ref filePath, ref isFileModified, inputBox);
						break;
					case DialogResult.No:
						OpenFileDialogue(inputBox,ref filePath);
						break;
					default:
						break;
				}
			}
			else
			{
				OpenFileDialogue(inputBox,ref filePath);
			}
			FileModifiedSaved(ref isFileModified);
		}

		public static void OpenFileDialogue(RichTextBox inputBox, ref string filePath)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog
			{
				Filter = "Текстовые файлы (*.txt)|*.txt|Файлы (*.cs)|*.cs|Все файлы (*.*)|*.*",
				Title = "Открыть",
				DefaultExt = "txt",
			};

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				filePath = openFileDialog1.FileName;
				inputBox.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
				inputBox.ReadOnly = false;
				inputBox.Enabled = true;
			}
		}

		public static void SaveAsFileDialogue(ref string filePath, bool isFileModified, RichTextBox inputBox)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog
			{
				Filter = "Текстовые файлы (*.txt)|*.txt|Файлы (*.cs)|*.cs|Все файлы (*.*)|*.*",
				Title = "Сохранить как"
			};

			if (!string.IsNullOrEmpty(filePath))
			{
				saveFileDialog1.FileName = Path.GetFileName(filePath);
			}

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				filePath = saveFileDialog1.FileName;
				SaveExistingFile(ref filePath, ref isFileModified,inputBox);
			}
		}

		public static void SaveExistingFile(ref string filePath, ref bool isFileModified, RichTextBox inputBox)
		{
			inputBox.SaveFile(filePath, RichTextBoxStreamType.PlainText);
			FileModifiedSaved(ref isFileModified);
		}

		public static void SaveFile(ref string filePath,ref bool isFileModified, RichTextBox inputBox)
		{
			if (!string.IsNullOrEmpty(filePath))
			{
				SaveExistingFile(ref filePath, ref isFileModified, inputBox);
			}
			else
			{
				SaveAsFileDialogue(ref filePath, isFileModified, inputBox);
			}
		}

		public static void ExitApp(ref string filePath, ref	bool isFileModified, RichTextBox inputBox, FormClosingEventArgs e)
		{
			if (isFileModified)
			{
				DialogResult result = MessageBox.Show("Файл не был сохранен. Хотите сохранить?", "Несохраненный файл", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

				switch (result)
				{
					case DialogResult.Yes:
						SaveFile(ref filePath, ref isFileModified, inputBox);
						break;
					case DialogResult.Cancel:
						e.Cancel = true;
						break;
						// Для DialogResult.No ничего не делаем, форма будет закрыта без сохранения файла
				}

			}
		}

		public static void FileModifiedSaved(ref bool isFileModified)
		{
			isFileModified = false;
		}

		public static void FileModifiedNotSaved(ref bool isFileModified)
		{
			isFileModified = true;
		}

		public static void UpdateLineNumbers(RichTextBox lineNumbersRichBox, RichTextBox inputRichBox, Rectangle clientRectangle)
		{
			Point pt = new Point(0, 0);

			int charIndex = inputRichBox.GetCharIndexFromPosition(pt);
			int lineIndex = inputRichBox.GetLineFromCharIndex(charIndex);

			pt.X = clientRectangle.Width;
			pt.Y = clientRectangle.Height;

			int charLastIndex = inputRichBox.GetCharIndexFromPosition(pt);
			int lineLastIndex = inputRichBox.GetLineFromCharIndex(charLastIndex);

			lineNumbersRichBox.Text = "";
			lineNumbersRichBox.Width = GetWidth(inputRichBox);
			lineNumbersRichBox.Height = inputRichBox.Height;
			for (int i = lineIndex; i < lineLastIndex + 1 ; i++)
			{
				lineNumbersRichBox.Text += i + 1 + "\n";
			}
		}

		public static int GetWidth(RichTextBox inputRichBox)
		{
			int w = 25;
			// get total lines of richTextBox1    
			int line = inputRichBox.Lines.Length;

			if (line <= 99)
			{
				w = 20 + (int)inputRichBox.Font.Size;
			}
			else if (line <= 999)
			{
				w = 30 + (int)inputRichBox.Font.Size;
			}
			else
			{
				w = 50 + (int)inputRichBox.Font.Size;
			}

			return w;
		}
	}
}
