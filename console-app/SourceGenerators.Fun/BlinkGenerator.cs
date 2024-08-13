using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerators.Fun;

[Generator]
public class BlinkGenerator : ISourceGenerator
{
    private static string _content =
        """"""
        using System;
        using System.Threading.Tasks;

        namespace SourceGenerators.Example;

        partial class Program
        {
            static async Task Main(string[] args)
            {
                for (int i = 0; i < _content.Length; i++)
                {
                    Console.Write(_content[i]);
                    await Task.Delay(60);
                }

                Console.ReadLine();
            }

            private static string _content =
                """
                All the, small things
                True care, truth brings
                I'll take, one lift
                Your ride, best trip


                Always, I know
                You'll be at my show
                Watching, waiting,
                commiserating

                Say it ain't so, I will not go,
                turn the lights off, carry me home
                Na, na...

                Late night, come home
                Work sucks, I know
                She left me roses by the stairs,
                surprises let me know she cares

                Say it ain't so, I will not go,
                turn the lights off, carry me home
                Na, na...


                Say it ain't so, I will not go,
                turn the lights off, carry me home
                Keep your head still, I'll be your thrill,
                the night will go on, my little windmill

                Say it ain't so, I will not go, (Na, na...)
                turn the lights off, carry me home (Na, na...)
                Keep your head still, I'll be your thrill, (Na, na...)
                the night will go on, the night will go on, (Na, na...)
                my little windmill
                """;
        }
        """""";
    
    
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        context.AddSource($"BlinkForever.cs", SourceText.From(_content, Encoding.UTF8));
    }
}