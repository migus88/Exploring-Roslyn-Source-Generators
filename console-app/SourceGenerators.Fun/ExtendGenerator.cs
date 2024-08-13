using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerators.Fun
{
    [Generator]
    public class ExtendGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Nothing to do here
        }

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var syntaxTree in context.Compilation.SyntaxTrees) // Basically for each cs file
            {
                var model = context.Compilation.GetSemanticModel(syntaxTree); // We're taking the semantic model of this file
                var classes = syntaxTree.GetRoot() // Finding the root node
                    .DescendantNodes() // Getting all of its descendant nodes
                    .OfType<ClassDeclarationSyntax>(); // and taking only class declarations (no structs, records, interfaces or enums)

                foreach (var @class in classes) // Now we iterate on all class declarations
                {
                    var classSymbol = model.GetDeclaredSymbol(@class); // Getting the actual symbol that declares the class

                    if (classSymbol == null)
                    {
                        continue;
                    }

                    if (classSymbol.IsStatic) // Filtering out static classes
                        continue;

                    var namespaceName = classSymbol.ContainingNamespace.ToDisplayString(); // Getting this class namespace
                    var className = classSymbol.Name; // And the class name

                    // Creating a partial class with the same namespace and class name as the class in our iteration
                    var source = $@"
namespace {namespaceName}
{{
    public partial class {className}
    {{
        private string _whatIsThisString;
        private bool _whoAmI;

        public void WhereIsThisMethodCameFrom()
        {{
            System.Console.WriteLine(""From Source Generator"");
        }}
    }}
}}";
                    // Adding newly generated code to the compilation with actual file name
                    context.AddSource($"{className}_generated.cs", SourceText.From(source, Encoding.UTF8));
                }
            }
        }
    }
}