using System.Collections.Generic;

public enum TokenType
{
	KeywordFunction = 1, // ключевое слово function
	Identifier,    // Токен для идентификаторов функций
	ArgumentIdentifier,    // Токен для идентификаторов аргументов
	Space, // разделитель (пробел)
	Parenthesis, // фигурная скобка
	Operator, // оператор
	// фигурная скобка
	Keyword, // ключевое слово
	Number, // число
	Unacceptable	// недопустимый символ
}

public class Token
{
	public int CodeType { get; set; } = 0;
	public TokenType Type { get; set; }
	public string Value { get; set; }
	public int Column { get; set; }
	public Token(int codetype, TokenType type, string value, int firstposition)
	{
		CodeType = codetype;
		Type = type;
		Value = value;
		Column = firstposition;
	}

}

public class Lexer
{
	private string input;
	private int position;

	public Lexer(string input)
	{
		this.input = input;
		position = 1;
	}

	public List<Token> Tokenize()
	{
		List<Token> tokens = new List<Token>();

		while (position < input.Length)
		{
			char currentChar = input[position];

			if (char.IsLetter(currentChar))
			{
				tokens.Add(ScanWord());
			}
			else if (currentChar == '$')
			{
				tokens.Add(new Token((int)TokenType.Parenthesis, TokenType.Parenthesis, currentChar.ToString(), position));
				position++;
			}
			else if (currentChar == '(' || currentChar == ')')
			{
				tokens.Add(new Token((int)TokenType.Parenthesis,TokenType.Parenthesis, currentChar.ToString(), position));
				position++;
			}
			else if (currentChar == ',')
			{
				tokens.Add(new Token((int)TokenType.Operator, TokenType.Operator, currentChar.ToString(), position));
				position++;
			}
			else if (currentChar == ';')
			{
				tokens.Add(new Token((int)TokenType.Operator, TokenType.Operator, currentChar.ToString(), position));
				position++;
			}
			else if (currentChar == '+')
			{
				tokens.Add(new Token((int)TokenType.Operator, TokenType.Operator, currentChar.ToString(), position));
				position++;
			}
			else if (currentChar == '-')
			{
				tokens.Add(new Token((int)TokenType.Operator, TokenType.Operator, currentChar.ToString(), position));
				position++;
			}
			else if (currentChar == '*')
			{
				tokens.Add(new Token((int)TokenType.Operator, TokenType.Operator, currentChar.ToString(), position));
				position++;
			}
			else if (currentChar == '/')
			{
				tokens.Add(new Token((int)TokenType.Operator, TokenType.Operator, currentChar.ToString(), position));
				position++;
			}
			else if (char.IsWhiteSpace(currentChar)) // если пробел
			{
				position++;
			}
			else
			{
				// Неизвестный символ
				tokens.Add(new Token((int)TokenType.Unacceptable, TokenType.Unacceptable, currentChar.ToString(), position));
				position++;
			}
		}

		return tokens;
	}
	

	private Token ScanWord()
	{
		string word = ""; // Инициализация строки для хранения текущего слова
		int length = 0;

		//// Если текущий символ - "$", это может быть идентификатором переменной
		//if (position < input.Length && input[position] == '$')
		//{
		//	word += input[position]; // Добавляем текущий символ к строке
		//	position++; // Переходим к следующему символу
		//	length++;
		//}
		//else
		//{
		//	// Если текущий символ - буква, считываем последовательность букв
		//	while (position < input.Length && char.IsLetter(input[position]))
		//	{
		//		word += input[position]; // Добавляем текущий символ к строке
		//		position++; // Переходим к следующему символу
		//		length++;
		//	}
		//}
		// Считываем символ за символом до тех пор, пока текущий символ является буквой
		while (position < input.Length && char.IsLetter(input[position]))
		{
			word += input[position]; // Добавляем текущий символ к строке
			position++; // Переходим к следующему символу
			length++;
		}

		// После того, как прочитано ключевое слово, проверяем, соответствует ли оно известному ключевому слову
		switch (word)
		{
			case "function":
				return new Token((int)TokenType.Keyword,TokenType.Keyword, word, position - length);
			case "return":
				return new Token((int)TokenType.Keyword, TokenType.Keyword, word, position - length);
			default:
				// Если последовательность символов не является ключевым словом, это может быть идентификатором
				return new Token((int)TokenType.Identifier, TokenType.Identifier, word, position - length);
		}
	}

}
