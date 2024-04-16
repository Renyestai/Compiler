using System.Collections.Generic;
public class Parser
{
	private List<Token> tokens;
	private List<Token> errorTokens;
	private int current; // для отслеживания текущего индекса в списке токенов	 
	private int currentState;

	public Parser(List<Token> tokens)
	{
		this.tokens = tokens;
		current = 0;
		currentState = 1;
		errorTokens = new List<Token>(); // Создаем список ошибок в конструкторе парсера
	}

	public List<Token> Parse()
	{
		foreach (Token token in tokens)
		{
			if (token.Type == TokenType.NewLine)
			{
				// Пропускаем токен NewLine
				continue;
			}

			switch (currentState)
			{
				case 1:
					State1(token);
					break;
				case 3:
					State3(token);
					break;
				case 4:
					State4(token);
					break;
				case 5:
					State56(token);
					break;
				case 7:
					State7(token);
					break;
				case 8:
					State8(token);
					break;
				case 9:
					State9(token);
					break;
				case 11:
					State1112(token);
					break;
				case 12:
					State1213(token);
					break;
				case 14:
					State1415(token);
					break;
				case 16:
					State16(token);
					break;
				case 17:
					State17(token);
					break;
			}

			current++;
		}

		return errorTokens;
	}

	private void State1(Token token)

	{
		if (token.Type == TokenType.FunctionIdentifier && (tokens[current + 1].Type == TokenType.LeftParenthesis))
		{
			errorTokens.Add(token); // сказать, что пропущено и что стоит
			currentState = 4;
			errorTokens[current].ErrorString = "Неверный фрагмент " + tokens[current].Value + ". Пропущено ключевое слово 'function'.";
		}
		else if (token.Type != TokenType.KeywordFunction) // если не совпадает с ожидаемым, добавить в список ошибок
		{
			errorTokens.Add(token);
			currentState = 1;
			
			if ((tokens[current + 2].Type == TokenType.LeftParenthesis))
			{
				currentState = 3;
			}
			
		}
		else
		{
			currentState = 3;
		}
	}
	private void State3(Token token)
	{
		if (token.Type != TokenType.FunctionIdentifier)
		{
			errorTokens.Add(token);
			if(token.Type == TokenType.LeftParenthesis)
			{
				//сказать что пропущен идетификатор
				currentState = 5;

			}

			if((tokens[current + 2].Type == TokenType.LeftParenthesis))
				{

			}
		}
		else
		{
			currentState = 4;
		}
	}
	private void State4(Token token)
	{

		if (token.Type != TokenType.LeftParenthesis)
		{
			errorTokens.Add(token);

		}

		currentState = 5;
	}
	private void State56(Token token)
	{
		if (token.Type != TokenType.ArgumentIdentifier)
		{
			errorTokens.Add(token);

		}

		currentState = 7;
	}

	private void State7(Token token)
	{
		if (token.Type == TokenType.RightParenthesis)
		{
			currentState = 8;
		}
		else if (token.Type == TokenType.Comma)
		{
			currentState = 5;
		}
		else
		{
			errorTokens.Add(token);
			currentState = 7;
		}
	}

	private void State8(Token token)
	{
		if (token.Type == TokenType.NewLine)
		{
			currentState = 8;
		}
		else if (token.Type == TokenType.CurlyBrace)
		{
			currentState = 9;
		}
		else
		{
			errorTokens.Add(token);
		}
	}

	private void State9(Token token)
	{
		if (token.Type == TokenType.NewLine)
		{
			currentState = 9;
		}
		else if (token.Type == TokenType.KeywordReturn) // добавить пробел
		{
			currentState = 11;
		}
		else
		{
			errorTokens.Add(token);
		}
	}

	private void State1112(Token token)
	{
		if (token.Type == TokenType.ArgumentIdentifier)
		{
			currentState = 12;
		}
		else
		{
			errorTokens.Add(token);
		}
	}

	private void State1213(Token token)
	{
		if (token.Type == TokenType.Add || token.Type == TokenType.Subtract || token.Type == TokenType.Multiply || token.Type == TokenType.Divide)
		{
			currentState = 14;
		}
		else if (token.Type == TokenType.Semicolon)
		{
			currentState = 17;
		}
		else
		{
			errorTokens.Add(token);
			currentState = 16;
		}
	}

	private void State1415(Token token)
	{
		if (token.Type == TokenType.ArgumentIdentifier)
		{
			currentState = 16;
		}
		else
		{
			errorTokens.Add(token);
		}
	}

	private void State16(Token token)
	{
		if (token.Type == TokenType.Semicolon)
		{
			currentState = 17;
		}
		else
		{
			errorTokens.Add(token);
		}
	}

	private void State17(Token token)
	{
		if (token.Type == TokenType.CurlyBrace)
		{
			currentState = 18; //end
		}
		else
		{
			errorTokens.Add(token);
		}
	}
}
