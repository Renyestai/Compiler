﻿public partial class Parser
{
	private void StateRightPar(string input, ref int position)
	{
		int keywordStartPos = position; // Запоминаем начальную позицию
		bool RightParMet = false;
		if (position >= input.Length)
		{
			errors.Add(new ParserError("Входная строка закончилась раньше, чем ожидалось 7", position, position, ErrorType.UnfinishedExpression));
			return;
		}
		// Пропускаем пробелы до начала ключевого слова
		while (position < input.Length && char.IsWhiteSpace(input[position]))
		{
			position++; // Продвигаем позицию на следующий символ
		}

		char currentSymbol;
		ParserError error = new ParserError("Ожидалась правая скобка", keywordStartPos + 1, position + 1);

		while (position < input.Length && !char.IsWhiteSpace(input[position]) && input[position] != '{' && input[position] != 'r')
		{
			if (position >= input.Length)
			{
				if (error.Value != string.Empty)
					errors.Add(error);
				errors.Add(new ParserError("Обнаружено незаконченное выражение", keywordStartPos, position, ErrorType.UnfinishedExpression));
				return;
			}

			currentSymbol = input[position];

			if (currentSymbol == ')' && IsLeftPartofParMet)
			{
				RightParMet = true;
				if (error.Value != string.Empty)
					errors.Add(error);
				_ = new ParserError("Ожидалась правая скобка", keywordStartPos + 1, position + 1);
				position++;
				break;
			}
			else if (currentSymbol == ')' && !IsLeftPartofParMet)
			{
				RightParMet = true;
				// Если левая скобка не существует, добавляем сообщение об ошибке
				errors.Add(new ParserError("Нет соответствующей левой скобки", keywordStartPos, position, ErrorType.UnfinishedExpression));
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
		//if (RightParMet && !IsLeftPartofParMet)
		//{
		//	// Если левая скобка не существует, добавляем сообщение об ошибке
		//	errors.Add(new ParserError("Нет соответствующей левой скобки", keywordStartPos, position, ErrorType.UnfinishedExpression));
		//}

		if (!RightParMet)
		{
			errors.Add(new ParserError("Не найдена правая скобка", keywordStartPos, keywordStartPos + 1, ErrorType.UnfinishedExpression));
		}

	}
}