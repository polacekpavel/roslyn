﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Shared.Extensions;

namespace Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers
{
    public abstract partial class DiagnosticAnalyzerCorrectnessAnalyzer : DiagnosticAnalyzer
    {
        protected abstract class CompilationAnalyzer
        {
            private readonly INamedTypeSymbol diagnosticAnalyzer;
            private readonly INamedTypeSymbol diagnosticAnalyzerAttribute;

            public CompilationAnalyzer(INamedTypeSymbol diagnosticAnalyzer, INamedTypeSymbol diagnosticAnalyzerAttribute)
            {
                this.diagnosticAnalyzer = diagnosticAnalyzer;
                this.diagnosticAnalyzerAttribute = diagnosticAnalyzerAttribute;
            }

            protected INamedTypeSymbol DiagnosticAnalyzer { get { return this.diagnosticAnalyzer; } }
            protected INamedTypeSymbol DiagnosticAnalyzerAttribute { get { return this.diagnosticAnalyzerAttribute; } }

            protected bool IsDiagnosticAnalyzer(INamedTypeSymbol type)
            {
                return type.Equals(this.diagnosticAnalyzer);
            }

            internal void AnalyzeSymbol(SymbolAnalysisContext symbolContext)
            {
                var namedType = (INamedTypeSymbol)symbolContext.Symbol;
                if (namedType.GetBaseTypes().Any(IsDiagnosticAnalyzer))
                {
                    AnalyzeDiagnosticAnalyzer(symbolContext);
                }
            }

            protected abstract void AnalyzeDiagnosticAnalyzer(SymbolAnalysisContext symbolContext);
        }
    }
}