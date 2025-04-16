using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DebugLogAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DBG001";
        private static readonly LocalizableString Title = "Debug.Log usage interdit";
        private static readonly LocalizableString MessageFormat = "N'utilisez pas {0}, utilisez SpecialLogger.Log() à la place.";
        private static readonly LocalizableString
            Description = "Remplacez tous les appels à UnityEngine.Debug.Log par le logger maison.";
        private const string Category = "Usage";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            var symbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
            if (symbol == null)
                return;

            // Vérifie si c'est Debug.Log, Debug.LogWarning, etc.
            if (symbol.ContainingType.ToString() == "UnityEngine.Debug" &&
                symbol.Name.StartsWith("Log"))
            {
                var diagnostic = Diagnostic.Create(
                    Rule,
                    invocation.GetLocation(),
                    $"Debug.{symbol.Name}"
                );
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
