using Fiona.IDE.Compiler.Tokens;
using FluentAssertions;
using System.Text;

namespace Fiona.IDE.Compiler.Tests.Tokens;

public class TokenizerTests
{
    [Fact]
    public async Task When_Given_String_With_Using_Clause_Should_Return_Using_Tokens()
    {
        // Arrange
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(TokenizerTestsData.UsingTokens!));
        using StreamReader reader = new(stream);
        // Act
        IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);
        // Assert
        tokens.Count.Should().Be(5);
        tokens.First().Value.Should().Be(TokenType.UsingBegin.ToString());
        tokens.First().Type.Should().Be(TokenType.UsingBegin);
        tokens.Last().Value.Should().Be(TokenType.UsingEnd.ToString());
        tokens.Last().Type.Should().Be(TokenType.UsingEnd);
        tokens.ToList()[1].Value.Should().Be("using system;");
        tokens.ToList()[1].Type.Should().Be(TokenType.Using);
        tokens.ToList()[2].Value.Should().Be("using system.collections;");
        tokens.ToList()[2].Type.Should().Be(TokenType.Using);
        tokens.ToList()[3].Value.Should().Be("using system.collections.generic;");
        tokens.ToList()[3].Type.Should().Be(TokenType.Using);
    }
    
    [Fact]
    public async Task When_Given_String_With_Controller_Clause_Should_Return_Controller_Tokens()
    {
        // Arrange
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(TokenizerTestsData.ControllerTokens));
        using StreamReader reader = new(stream);
        // Act
        IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);
        // Assert
        tokens.Count.Should().Be(10);
        tokens.First().Value.Should().Be("class TestController");
        tokens.First().Type.Should().Be(TokenType.Class);
        tokens.ToList()[1].Value.Should().Be("route: /home;");
        tokens.ToList()[1].Type.Should().Be(TokenType.Route);
        tokens.ToList()[2].Value.Should().Be("endpoint Index");
        tokens.ToList()[2].Type.Should().Be(TokenType.Endpoint);
        tokens.ToList()[3].Value.Should().Be("route: /test;");
        tokens.ToList()[3].Type.Should().Be(TokenType.Route);
        tokens.ToList()[4].Value.Should().Be("method: [GET, POST];");
        tokens.ToList()[4].Type.Should().Be(TokenType.Method);
        tokens.ToList()[5].Value.Should().Be("bodyBegin");
        tokens.ToList()[5].Type.Should().Be(TokenType.BodyBegin);
        tokens.ToList()[6].Value.Should().Be("// comment todo: body");
        tokens.ToList()[6].Type.Should().Be(TokenType.Comment);
        tokens.ToList()[7].Value.Should().Be("bodyEnd");
        tokens.ToList()[7].Type.Should().Be(TokenType.BodyEnd);
        tokens.ToList()[8].Value.Should().Be("endpointEnd");
        tokens.ToList()[8].Type.Should().Be(TokenType.EndpointEnd);
        tokens.ToList()[9].Value.Should().Be("classEnd");
        tokens.ToList()[9].Type.Should().Be(TokenType.ClassEnd);
    }
}