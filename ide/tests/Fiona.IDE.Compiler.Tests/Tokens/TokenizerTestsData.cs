namespace Fiona.IDE.Compiler.Tests.Tokens;

public static class TokenizerTestsData
{
    public const string UsingTokens = """
                                      
                                                  usingBegin;
                                                  using system;
                                                  using system.collections;
                                                  using system.collections.generic;
                                                  usingEnd;
                                              
                                      """;

    public const string ControllerTokens = """
                                           class TestController;
                                           route: /home;

                                           endpoint: Index;
                                           route: /test;
                                           method: [GET, POST];
                                           bodyBegin;
                                            // comment todo: body
                                           bodyEnd;
                                           endpointEnd;
                                           """;

    public const string FullTokensTest = UsingTokens + ControllerTokens;
}