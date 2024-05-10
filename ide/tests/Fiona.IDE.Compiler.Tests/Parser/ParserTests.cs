using Fiona.IDE.Compiler.Parser;
using Fiona.IDE.Compiler.Parser.Exceptions;
using Fiona.IDE.Compiler.Tests.Shared;
using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager.Models;
using FluentAssertions;
using System.Text;
using System.Text.RegularExpressions;

namespace Fiona.IDE.Compiler.Tests.Parser;

public sealed class ParserTests : IDisposable
{
    private readonly IDE.Compiler.Parser.Parser _parser;
    private readonly ProjectFile _projectFile; 


    public ParserTests()
    {
        Validator validator = new();
        _parser = new IDE.Compiler.Parser.Parser(validator);
        try
        {
            File.Delete("./Test.fn");
        }
        catch (Exception)
        {
            // ignored
        }
        _projectFile = ProjectFile.Create("./Test.fn");
    }

    [Fact]
    public async Task When_Given_TokenizedCode_Should_Return_Parsed_Code()
    {
        // arrange
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.FullTokensTest!));
        using StreamReader reader = new(stream);
        IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);

        // act
        string result = await _parser.ParseAsync(tokens, _projectFile);

        // assert
        string expectedResult = """
                                using system;
                                using system.collections;
                                using system.collections.generic;

                                namespace Token.Test

                                [Controller("/home")]
                                public class TestController()
                                {
                                
                                     [Route(HttpMethodType.Get | HttpMethodType.Post, "option/get")]
                                     public Task Index()
                                     {
                                        // comment todo: body
                                     }
                                }
                                """;
        expectedResult = Regex.Replace(expectedResult, @"\s+", "");
        result = Regex.Replace(result, @"\s+", "");
        result.Should().Be(expectedResult);
    }

    [Theory, MemberData(nameof(InvalidTokenMemberData))]
    internal async Task When_Given_Invalid_Tokens_Then_Throw_Exception(params Token[] tokens)
    {
        // Act
        Func<Task<string>> action = async () => await _parser.ParseAsync(tokens, _projectFile);
        
        // Assert
        await action.Should().ThrowAsync<ParserException>();
    }

    public static IEnumerable<object[]> InvalidTokenMemberData =>
    [
        [
            new Token(TokenType.UsingBegin),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.UsingEnd),
        ],
        [
            new Token(TokenType.Using, "system"),
            new Token(TokenType.UsingEnd),
        ],
        [
            new Token(TokenType.UsingBegin),
            new Token(TokenType.Using, "system"),
        ],[
            new Token(TokenType.UsingEnd),
            new Token(TokenType.BodyBegin),
            new Token(TokenType.UsingBegin),
        ],
    ];

    public void Dispose()
    {
        File.Delete("./Test.fn");
    }
}