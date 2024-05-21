using Fiona.IDE.Compiler.Tests.Shared;
using Fiona.IDE.Compiler.Tokens;
using FluentAssertions;
using System.Text;

namespace Fiona.IDE.Compiler.Tests.Tokens;

public sealed class TokenizerTests
{
    [Fact]
    public async Task When_Given_String_With_Using_Clause_Should_Return_Using_Tokens()
    {
        // Arrange
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.UsingTokens!));
        using StreamReader reader = new(stream);
        // Act
        IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);
        // Assert
        tokens.Count.Should().Be(5);
        tokens.First().Value.Should().Be(TokenType.UsingBegin.ToString());
        tokens.First().Type.Should().Be(TokenType.UsingBegin);
        tokens.Last().Value.Should().Be(TokenType.UsingEnd.ToString());
        tokens.Last().Type.Should().Be(TokenType.UsingEnd);
        tokens.ToList()[1].Value.Should().Be("system");
        tokens.ToList()[1].Type.Should().Be(TokenType.Using);
        tokens.ToList()[2].Value.Should().Be("system.collections");
        tokens.ToList()[2].Type.Should().Be(TokenType.Using);
        tokens.ToList()[3].Value.Should().Be("system.collections.generic");
        tokens.ToList()[3].Type.Should().Be(TokenType.Using);
    }
    
    [Fact]
    public async Task When_Given_String_With_Controller_Clause_Should_Return_Controller_Tokens()
    {
        // Arrange
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.ControllerTokens));
        using StreamReader reader = new(stream);
        // Act
        IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);
        // Assert
        tokens.Count.Should().Be(8);
        tokens.First().Value.Should().Be("TestController");
        tokens.First().Type.Should().Be(TokenType.Class);
        tokens.ElementAt(1).Value.Should().Be("/home");
        tokens.ElementAt(1).Type.Should().Be(TokenType.Route);
        tokens.ElementAt(2).Value.Should().Be("Index");
        tokens.ElementAt(2).Type.Should().Be(TokenType.Endpoint);
        tokens.ElementAt(3).Value.Should().Be("/test");
        tokens.ElementAt(3).Type.Should().Be(TokenType.Route);
        tokens.ElementAt(4).Value.Should().Be("[GET, POST]");
        tokens.ElementAt(4).Type.Should().Be(TokenType.Method);
        tokens.ElementAt(5).Value.Should().Be("User");
        tokens.ElementAt(5).Type.Should().Be(TokenType.ReturnType);       
        tokens.ElementAt(6).Value.Should().Be(TokenType.BodyBegin.ToString());
        tokens.ElementAt(6).Type.Should().Be(TokenType.BodyBegin);
        tokens.ElementAt(7).Value.Should().Be(TokenType.BodyEnd.ToString());
        tokens.ElementAt(7).Type.Should().Be(TokenType.BodyEnd);
    }
    
      [Fact]
    public async Task When_Given_String_With_Full_Endpoint_Definition_Clause_Should_Return_Using_Tokens()
    {
        // Arrange
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(SampleTestCode.FullTokensTest!));
        using StreamReader reader = new(stream);
        // Act
        IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);
        // Assert
        tokens.Count.Should().Be(13);
        tokens.First().Value.Should().Be(TokenType.UsingBegin.ToString());
        tokens.First().Type.Should().Be(TokenType.UsingBegin);
        tokens.ElementAt(1).Value.Should().Be("system");
        tokens.ElementAt(1).Type.Should().Be(TokenType.Using);
        tokens.ElementAt(2).Value.Should().Be("system.collections");
        tokens.ElementAt(2).Type.Should().Be(TokenType.Using);
        tokens.ElementAt(3).Value.Should().Be("system.collections.generic");
        tokens.ElementAt(3).Type.Should().Be(TokenType.Using);
        tokens.ElementAt(4).Value.Should().Be(TokenType.UsingEnd.ToString());
        tokens.ElementAt(4).Type.Should().Be(TokenType.UsingEnd);
        tokens.ElementAt(5).Value.Should().Be("TestController");
        tokens.ElementAt(5).Type.Should().Be(TokenType.Class);
        tokens.ElementAt(6).Value.Should().Be("/home");
        tokens.ElementAt(6).Type.Should().Be(TokenType.Route);
        tokens.ElementAt(7).Value.Should().Be("Index");
        tokens.ElementAt(7).Type.Should().Be(TokenType.Endpoint);
        tokens.ElementAt(8).Value.Should().Be("/test");
        tokens.ElementAt(8).Type.Should().Be(TokenType.Route);
        tokens.ElementAt(9).Value.Should().Be("[GET, POST]");
        tokens.ElementAt(9).Type.Should().Be(TokenType.Method);
        tokens.ElementAt(10).Type.Should().Be(TokenType.ReturnType);
        tokens.ElementAt(10).Value.Should().Be("User");
        tokens.ElementAt(11).Value.Should().Be(TokenType.BodyBegin.ToString());
        tokens.ElementAt(11).Type.Should().Be(TokenType.BodyBegin);
        tokens.ElementAt(12).Value.Should().Be(TokenType.BodyEnd.ToString());
        tokens.ElementAt(12).Type.Should().Be(TokenType.BodyEnd);
    }
}