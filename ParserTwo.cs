using System.Collections.Generic;

public partial class SecParser
{
	private string input;
	private int position;
	private List<ParserError> errors;
	public bool IsLeftPartofParMet = false; // для скобок
	public bool IsLeftPartofCurlyMet = false; // для фигурных скобок
	public bool LastState = false; // для фигурных скобок
	public SecParser(string input)
	{
		this.input = input;
		position = 0;
		errors = new List<ParserError>(); // Создаем список ошибок в конструкторе парсера
		
	}
	public List<ParserError> Parse()
	{
		while (position < input.Length)
		{
			StateKeywordFunction(input, ref position);
			StateFunctionID(input, ref position);
			StateLeftPar(input, ref position);
			StateFirstArgID(input, ref position);
			StateComma(input, ref position);
			StateSecondArgID(input, ref position);
			StateRightPar(input, ref position);
			StateLeftCurly(input, ref position);
			StateReturn(input, ref position);
			StateFirstArg(input, ref position);
			StateArifOperator(input, ref position);
			StateSecondArg(input, ref position);
			StateSemicolon(input, ref position);
			StateRightCurly(input, ref position);
			if(LastState) { break; }
		}

		return errors;
	}
	private void StateArifOperator(string input, ref int position)
	{
		if (position >= input.Length)
		{
			errors.Add(new ParserError("Входная строка закончилась раньше, чем ожидалось 10", position, position));
			return;
		}
		int keywordStartPos = position; // Запоминаем начальную позицию ключевого слова
		bool ArifOperMet = false;
		// Пропускаем пробелы до начала ключевого слова
		while (position < input.Length && (char.IsWhiteSpace(input[position]) || input[position] == '\n'))
		{
			position++; // Продвигаем позицию на следующий символ
		}
		char currentSymbol;

		ParserError error = new ParserError("Ожидался арифметический оператор", keywordStartPos + 1, position + 1);
		
		while (position < input.Length && (!char.IsWhiteSpace(input[position]) && (input[position] != ';' && input[position] != '$')))
		{
			if (position >= input.Length)
			{
				if (error.Value != string.Empty)
					errors.Add(error);
				errors.Add(new ParserError("Обнаружено незаконченное выражение", keywordStartPos, position, ErrorType.UnfinishedExpression));
				return;
			}

			currentSymbol = input[position];

			if (currentSymbol == '+' || currentSymbol == '-' || currentSymbol == '*' || currentSymbol == '/' )
			{

				ArifOperMet = true;
				if (error.Value != string.Empty)
					errors.Add(error);
				_ = new ParserError("Ожидался арифметический оператор", keywordStartPos + 1, position + 1);
				position++;
				break;
			}
			else
			{
				error.Value += input[position];
				error.EndIndex = position + 1;
			}

			position++;
		}

		// Если левая скобка не была найдена, добавляем сообщение об ошибке
		if (!ArifOperMet)
		{
			errors.Add(new ParserError("Не найден арифметический оператор", keywordStartPos, position));
		}

		
	}
	private void StateSecondArg(string input, ref int position)
	{
		int keywordStartPos = position; // Запоминаем начальную позицию ключевого слова


		if (position >= input.Length)
		{
			errors.Add(new ParserError("Входная строка закончилась раньше, чем ожидалось 11", position, position));
			return;
		}

		// Пропускаем пробелы до начала ключевого слова
		while (position < input.Length && (char.IsWhiteSpace(input[position]) || input[position] == '\n'))
		{
			position++; // Продвигаем позицию на следующий символ
		}

		bool IsNotFirstSymbol = false;
		bool IsNotMissingSymbol = false;
		char currentSymbol;
		ParserError error = new ParserError("Ожидалась переменная", keywordStartPos + 1, position + 1);

		while (position < input.Length && (!char.IsWhiteSpace(input[position]) && input[position] != ';' && input[position] != '\n'))
		{
			if (position >= input.Length)
			{
				if (error.Value != string.Empty)
					errors.Add(error);
				errors.Add(new ParserError("Обнаружено незаконченное выражение", keywordStartPos, position, ErrorType.UnfinishedExpression));
				return;
			}

			currentSymbol = input[position];

			if (currentSymbol == '$' && !IsNotFirstSymbol)
			{
				IsNotFirstSymbol = true;
				if (error.Value != string.Empty)
					errors.Add(error);
				error = new ParserError("Ожидалась переменная", position, position);
			}
			else if (IsNotFirstSymbol && !IsNotMissingSymbol && (char.IsLetter(currentSymbol) || currentSymbol == '_'))
			{
				IsNotMissingSymbol = true;
				if (error.Value != string.Empty)
					errors.Add(error);
				error = new ParserError("Ожидалась переменная", position, position);
			}
			else if (IsNotFirstSymbol && (char.IsLetter(currentSymbol) || char.IsDigit(currentSymbol) || currentSymbol == '_'))
			{
				if (error.Value != string.Empty)
					errors.Add(error);
				error = new ParserError("Ожидалась переменная", position, position);
			}
			else
			{
				error.Value += input[position];
				error.EndIndex = position + 1;
			}

			position++;
		}
		if (!IsNotFirstSymbol && !IsNotMissingSymbol)
		{
			errors.Add(error);
		}

		if (!IsNotMissingSymbol && IsNotFirstSymbol)
		{
			errors.Add(new ParserError("Недописана переменная", keywordStartPos, position));
		}

	}
	private void StateSemicolon(string input, ref int position)
	{
		
		if (position >= input.Length)
		{
			errors.Add(new ParserError("Входная строка закончилась раньше, чем ожидалось 12", position, position));
			return;
		}

		int keywordStartPos = position; // Запоминаем начальную позицию ключевого слова
		bool SemicolonMet = false;
		// Пропускаем пробелы до начала ключевого слова
		while (position < input.Length && (char.IsWhiteSpace(input[position]) || input[position] == '\n'))
		{
			position++; // Продвигаем позицию на следующий символ
		}
		char currentSymbol;

		ParserError error = new ParserError("Ожидалась точка с запятой", keywordStartPos + 1, position + 1);

		while (position < input.Length && (!char.IsWhiteSpace(input[position]) && input[position] != '}' && input[position] != '\n'))
		{

			if (position >= input.Length)
			{
				if (error.Value != string.Empty)
					errors.Add(error);
				errors.Add(new ParserError("Обнаружено незаконченное выражение", keywordStartPos, position, ErrorType.UnfinishedExpression));
				return;
			}

			currentSymbol = input[position];

			if (currentSymbol == ';')
			{
				SemicolonMet = true;
				if (error.Value != string.Empty)
					errors.Add(error);
				_ = new ParserError("Ожидалась точка с запятой", keywordStartPos + 1, position + 1);
				position++;
				break;
			}
			else
			{
				error.Value += input[position];
				error.EndIndex = position + 1;
			}

			position++;
		}

		// Если левая скобка не была найдена, добавляем сообщение об ошибке
		if (!SemicolonMet)
		{
			errors.Add(new ParserError("Не найдена точка с запятой", keywordStartPos, position));
			
		}

	
	}
	private void StateRightCurly(string input, ref int position)
	{
		if (position >= input.Length)
		{
			errors.Add(new ParserError("Входная строка закончилась раньше, чем ожидалось 13", position, position));
			return;
		}
		LastState = true;
		int keywordStartPos = position; // Запоминаем начальную позицию
		bool RightCurlyMet = false;

		// Пропускаем пробелы до начала ключевого слова
		while (position < input.Length && char.IsWhiteSpace(input[position]))
		{
			position++; // Продвигаем позицию на следующий символ
		}

		char currentSymbol;
		ParserError error = new ParserError("Ожидалась правая фигурная скобка", keywordStartPos + 1, position + 1);

		while (position < input.Length && (!char.IsWhiteSpace(input[position]) && input[position] != '\n'))
		{
			if (position >= input.Length)
			{
				if (error.Value != string.Empty)
					errors.Add(error);
				errors.Add(new ParserError("Обнаружено незаконченное выражение", keywordStartPos, position, ErrorType.UnfinishedExpression));
				return;
			}

			currentSymbol = input[position];

			if (currentSymbol == '}' && IsLeftPartofCurlyMet)
			{
				RightCurlyMet = true;
				if (error.Value != string.Empty)
					errors.Add(error);
				_ = new ParserError("Ожидалась правая фигурная скобка", keywordStartPos + 1, position + 1);
				position++;
				break;
			}
			else if (currentSymbol == '}' && !IsLeftPartofCurlyMet)
			{
				RightCurlyMet = true;
				errors.Add(new ParserError("Нет соответствующей левой фигурной скобки", keywordStartPos, position, ErrorType.UnfinishedExpression));
				position++;
				break;
			}
			else
			{
				error.Value += input[position];
				error.EndIndex = position + 1;
			}

			position++;
		}

		if (!RightCurlyMet)
		{
			errors.Add(new ParserError("Не найдена правая фигурная скобка", keywordStartPos, keywordStartPos + 1));
		}
	}

}