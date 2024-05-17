namespace Fiona.IDE.Compiler.Tests.Shared;

public static class SampleTestCode
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
                                           return: User;
                                           bodyBegin;
                                            // comment todo: body
                                           bodyEnd;
                                           """;

    public const string FullTokensTest = UsingTokens + ControllerTokens;

    public const string ControllerWithParameters = """
                                                   class TestController;
                                                   route: /home;
                                                   
                                                   endpoint: Index;
                                                   route: /{name};
                                                   method: [GET, POST];
                                                   return: User;
                                                   input:
                                                     - [FromRoute] name: string
                                                     - [FromQuery] age: int
                                                     - [FromBody] user: User
                                                     - [Cookie] userId: long;
                                                   bodyBegin;
                                                    // comment todo: body
                                                   bodyEnd;
                                                   """;

    public const string FullTokensTestWithParameter = UsingTokens + ControllerTokens;

}