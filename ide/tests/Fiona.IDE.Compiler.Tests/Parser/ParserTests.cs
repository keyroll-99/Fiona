using Fiona.IDE.Compiler.Parser;
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

    public ParserTests()
    {
        Validator validator = new();
        _parser = new IDE.Compiler.Parser.Parser(validator);
    }

    [Fact]
    public async Task When_Given_TokenizedCode_Should_Return_Parsed_Code()
    {
        // arrange
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.FullTokensTest!));
        using StreamReader reader = new(stream);
        IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);
        ProjectFile projectFile = ProjectFile.Create("./Test.fn");

        // act
        string result = await _parser.ParseAsync(tokens, projectFile);

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

    public void Dispose()
    {
        File.Delete("./Test.fn");
    }
}