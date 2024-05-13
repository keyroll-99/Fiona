using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler.Tests.Parser;

public partial class ParserTests
{
    public static IEnumerable<object[]> InvalidUsingTokenData =>
    [
        [
            $"{nameof(InvalidUsingTokenData)}_1",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.UsingEnd),
        ],
        [
            $"{nameof(InvalidUsingTokenData)}_2",
            new Token(TokenType.Using, "system"),
            new Token(TokenType.UsingEnd),
        ],
        [
            $"{nameof(InvalidUsingTokenData)}_3",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using, "system"),
        ],
        [
            $"{nameof(InvalidUsingTokenData)}_4",
            new Token(TokenType.UsingEnd),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.UsingBegin),
        ],
        [
            $"{nameof(InvalidUsingTokenData)}_5",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using),
            new Token(TokenType.UsingEnd),
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using),
            new Token(TokenType.UsingEnd),
        ],
    ];

    public static IEnumerable<object[]> ValidUsingTokenData =>
    [
        [
            $"{nameof(ValidUsingTokenData)}_1",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using, "system"),
            new Token(TokenType.Using, "system.collections"),
            new Token(TokenType.UsingEnd),
        ],
    ];

    public static IEnumerable<object[]> InvalidClassTokenData =>
    [
        [
            $"{nameof(InvalidClassTokenData)}_1",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Endpoint),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.Route, "/home"),
        ],
        [
            $"{nameof(InvalidClassTokenData)}_2",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Endpoint),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.Endpoint, "/home"),
        ],
        [
            $"{nameof(InvalidClassTokenData)}_3",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Endpoint),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.Method, "/home"),
        ],
        [
            $"{nameof(InvalidClassTokenData)}_4",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.BodyBegin),
        ],
        [
            $"{nameof(InvalidClassTokenData)}_5",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Method),
        ],
        [
            $"{nameof(InvalidClassTokenData)}_6",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Endpoint, "Index"),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.BodyBegin)
        ],
        [
            $"{nameof(InvalidClassTokenData)}_7",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Endpoint, "Index"),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.BodyEnd),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.BodyEnd)
        ],
        [
            $"{nameof(InvalidClassTokenData)}_9",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Endpoint, "Index"),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.Method),
            new Token(TokenType.BodyEnd),
        ],
        [
            $"{nameof(InvalidClassTokenData)}_10",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Endpoint, "Index"),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.Route),
            new Token(TokenType.BodyEnd),
        ],
        [
            $"{nameof(InvalidClassTokenData)}_11",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Endpoint, "Index"),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.Endpoint),
            new Token(TokenType.BodyEnd),
        ],        [
            $"{nameof(InvalidClassTokenData)}_12",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Class, "Index"),
            new Token(TokenType.Endpoint, "Index"),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.Endpoint),
            new Token(TokenType.BodyEnd),
        ],        [
            $"{nameof(InvalidClassTokenData)}_13",
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Route),
            new Token(TokenType.Route)
        ],
    ];
    
    public static IEnumerable<object[]> ValidClassTokenData =>
    [
        [
            $"{nameof(ValidClassTokenData)}_1",
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using, "System"),
            new Token(TokenType.UsingEnd),
            new Token(TokenType.Class, "TestController"),
            new Token(TokenType.Route, "Home"),
            new Token(TokenType.Endpoint, "Index"),
            new Token(TokenType.Method, "[GET, POST]"),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.Comment, "comment todo: body"),
            new Token(TokenType.BodyEnd),
        ],
    ];

}