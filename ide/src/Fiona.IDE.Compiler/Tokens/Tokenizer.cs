using Fiona.IDE.Compiler.Tokens.Exceptons;

namespace Fiona.IDE.Compiler.Tokens;

internal static class Tokenizer
{
    private static readonly char[] SplitChars = [';'];

    public static Task<IReadOnlyCollection<IToken>> GetTokensAsync(StreamReader input)
    {
        if (input is null)
        {
            throw new EmptyInputStreamException();
        }

        return ReadTokensFromInputAsync(input);
    }

    private static async Task<IReadOnlyCollection<IToken>> ReadTokensFromInputAsync(StreamReader input)
    {
        List<IToken> tokens = [];
        try{
            while (true)
            {
                string commands = await GetNextCommand(input);
                if (string.IsNullOrWhiteSpace(commands))
                {
                    continue;
                }
                tokens.Add(TokenFactory.CreateToken(commands));
            }
        } catch (EndOfStreamException)
        {
            return tokens;
        }
        // while (true)
        // {
        //     string commands = await GetNextCommand(input);
        //     if (string.IsNullOrWhiteSpace(commands))
        //     {
        //         continue;
        //     }
        //     tokens.Add(TokenFactory.CreateToken(commands));
        // }
        //
        // return tokens;
    }

    private static async Task<string> GetNextCommand(StreamReader input)
    {
        // TODO: fix buffer issue
        string result = "";
        while (true)
        {
            string? inputLine = await input.ReadLineAsync();
            if (inputLine is null)
            {
                throw new Exception("Something went wrong while read file");
            }
            if (string.IsNullOrWhiteSpace(inputLine))
            {
                return string.Empty;
            }

            int indexOfFirstSplitChar = inputLine.IndexOfAny(SplitChars);
            if (indexOfFirstSplitChar == -1)
            {
                result += inputLine;
                continue;
            }
            var x = inputLine.Length - indexOfFirstSplitChar - 1;
            var currentPosition = input.BaseStream.Position;

            input.DiscardBufferedData();
            input.BaseStream.Seek(currentPosition - x, SeekOrigin.Begin);
            result += inputLine[..indexOfFirstSplitChar].Trim();
            break;
        }

        return result;
    }
}