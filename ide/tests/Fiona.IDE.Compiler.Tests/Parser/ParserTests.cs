using Fiona.IDE.Compiler.Parser;
using Fiona.IDE.Compiler.Parser.Exceptions;
using Fiona.IDE.Compiler.Tests.Shared;
using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager.Models;
using FluentAssertions;
using System.Text;
using System.Text.RegularExpressions;

namespace Fiona.IDE.Compiler.Tests.Parser;

public partial class ParserTests
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
        ProjectFile projectFile = GetTestProjectFile(nameof(When_Given_TokenizedCode_Should_Return_Parsed_Code));
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.FullTokensTest!));
        using StreamReader reader = new(stream);
        IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);

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

    [Theory, MemberData(nameof(InvalidTokenMemberData))]
    internal async Task When_Given_Invalid_Using_Tokens_Then_Throw_Exception(string fileName, params Token[] tokens)
    {
        // Arrange
        ProjectFile projectFile = GetTestProjectFile(fileName);

        // Act
        Func<Task<string>> action = async () => await _parser.ParseAsync(tokens, projectFile);

        // Assert
        await action.Should().ThrowAsync<ParserException>();
    }

    [Theory, MemberData(nameof(ValidTokenMemberData))]
    internal async Task When_Given_Valid_Using_Tokens_Then_Should_Not_Throw_Error(string fileName, params Token[] tokens)
    {
        // Arrange
        ProjectFile projectFile = GetTestProjectFile(fileName);

        // Act
        Func<Task<string>> action = async () => await _parser.ParseAsync(tokens, projectFile);

        // Assert
        await action.Should().NotThrowAsync<ParserException>();
    }
    
    private ProjectFile GetTestProjectFile(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
        }
        return ProjectFile.Create(name);
    }

}