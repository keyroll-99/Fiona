using Fiona.IDE.Compiler.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.IDE.Compiler
{
    public static class Extension
    {
        public static IServiceCollection AddCompiler(this IServiceCollection services)
        {
            services.AddSingleton<ICompiler, Compiler>();
            return services;
        }


        private static void AddTokens(this IServiceCollection services)
        {
            foreach (string tokenType in Enum.GetNames<TokenType>())
            {
                services.Add(new ServiceDescriptor(typeof(IToken), new Token(Enum.Parse<TokenType>(tokenType), tokenType), ServiceLifetime.Singleton));
            }
        }
    }
}