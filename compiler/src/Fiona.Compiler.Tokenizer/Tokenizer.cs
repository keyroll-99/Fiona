using Fiona.Compiler.Tokenizer.Exceptions;
using System.Text;

namespace Fiona.Compiler.Tokenizer;

public static class Tokenizer
{
    private static readonly char[] SplitChars = [';', '*'];

    public static async Task<IReadOnlyCollection<IToken>> GetTokensAsync(StreamReader input)
    {
        if (input is null)
        {
            throw new EmptyInputStreamException();
        }

        return (await ReadTokensFromInputAsync(input)).Where(t => t.Type != TokenType.Comment).ToList();
    }

    private static async Task<IReadOnlyCollection<IToken>> ReadTokensFromInputAsync(StreamReader input)
    {
        List<IToken> tokens = [];
        StringBuilder buffer = new();
        while (!input.EndOfStream)
        {
            IList<string>? commands = (await GetNextCommands(input, buffer))?.ToList();
            if (commands is null || (!commands.Any()))
            {
                continue;
            }
            tokens.AddRange(commands.Select(TokenFactory.CreateToken));
            if (tokens[^1].Type != TokenType.BodyBegin)
            {
                continue;
            }
            IToken? bodyToken = TokenFactory.CreateBodyToken(input);
            if (bodyToken is not null)
            {
                tokens.Add(bodyToken);
            }
            tokens.Add(TokenFactory.CreateToken(TokenType.BodyEnd.GetTokenKeyword()));
        }

        return tokens;
    }

    private static async Task<IEnumerable<string>?> GetNextCommands(StreamReader input, StringBuilder buffer)
    {
        List<string> result = [];
        while (true)
        {
            string? inputLine = (await input.ReadLineAsync())?.Trim();
            if (inputLine is null)
            {
                throw new Exception("Something went wrong while read file");
            }
            if (string.IsNullOrWhiteSpace(inputLine))
            {
                return null;
            }

            int indexOfFirstSplitChar = inputLine.IndexOfAny(SplitChars);
            StringBuilder resultBuilder = new();
            while (indexOfFirstSplitChar != -1)
            {
                resultBuilder.Clear();
                string line = inputLine[..indexOfFirstSplitChar].Trim();
                inputLine = inputLine[(indexOfFirstSplitChar + 1)..].Trim();

                if (buffer.Length > 0)
                {
                    resultBuilder.Append(buffer);
                    buffer.Clear();
                }
                resultBuilder.Append(line);
                result.Add(resultBuilder.ToString());
                indexOfFirstSplitChar = inputLine.IndexOfAny(SplitChars);
            }

            if (!string.IsNullOrWhiteSpace(inputLine))
            {
                buffer.Append(inputLine);
            }
            return result;
        }
    }


}