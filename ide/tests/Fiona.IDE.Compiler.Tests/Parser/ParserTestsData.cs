using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler.Tests.Parser;

public partial class ParserTests
{
    public static IEnumerable<object[]> InvalidTokenMemberData =>
    [
        [
            $"{nameof(When_Given_Invalid_Using_Tokens_Then_Throw_Exception)}_1",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.UsingEnd),
        ],
        [
            $"{nameof(When_Given_Invalid_Using_Tokens_Then_Throw_Exception)}_2",
            new Token(TokenType.Using, "system"),
            new Token(TokenType.UsingEnd),
        ],
        [
            $"{nameof(When_Given_Invalid_Using_Tokens_Then_Throw_Exception)}_3",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using, "system"),
        ],
        [
            $"{nameof(When_Given_Invalid_Using_Tokens_Then_Throw_Exception)}_4",
            new Token(TokenType.UsingEnd),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.UsingBegin),
        ],
        [
            $"{nameof(When_Given_Invalid_Using_Tokens_Then_Throw_Exception)}_5",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using),
            new Token(TokenType.UsingEnd),
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using),
            new Token(TokenType.UsingEnd),
        ],
    ];

    public static IEnumerable<object[]> ValidTokenMemberData =>
    [
        [
            $"{nameof(When_Given_Valid_Using_Tokens_Then_Should_Not_Throw_Error)}_1",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using, "system"),
            new Token(TokenType.Using, "system.collections"),
            new Token(TokenType.UsingEnd),
        ],
    ];

}