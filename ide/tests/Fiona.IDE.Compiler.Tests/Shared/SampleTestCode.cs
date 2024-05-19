namespace Fiona.IDE.Compiler.Tests.Shared;

public static class SampleTestCode
{
    public const string UsingTokens = """
                                      
                                                  usingBegin;
                                                  using system; using system.collections;
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
                                           bodyEnd;
                                           """;

    public const string FullTokensTest = UsingTokens + ControllerTokens;

    private const string ControllerWithParameters = """
                                                    class TestController;
                                                    route: /home;

                                                    endpoint: Index;
                                                    route: /{name};
                                                    method: [GET, POST];
                                                    return: User;
                                                    input:
                                                        - [FromRoute] name: string
                                                        - [QueryParam] age: int
                                                        - [Body] user: User
                                                        - [Cookie] userId: long;
                                                    inject:
                                                        - userService: IUserService;
                                                        - logger: ILogger<TestController>;
                                                    bodyBegin;
                                                    bodyEnd;
                                                    """; // TODO: how to mark comment?

    public const string FullTokensTestWithParameter = UsingTokens + ControllerWithParameters;

}