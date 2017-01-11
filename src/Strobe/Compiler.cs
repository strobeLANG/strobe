using System.Collections.Generic;
using System;
namespace Strobe
{
	// The Compiler Class
	public class Compiler
	{
		// Errors & Warnings
		Result Res = new Result();
		// Store the Input as readonly
		readonly string input;

		// Load the Input
		public Compiler(string input)
		{
			this.input = input;
		}

		// Compile
		public CompilerResult compile()
		{
			// Define the arrays.
			LexerResult lexer;
			SimplifierResult simplifier;
			ParserResult parser;
			CodeGeneratorResult codegen;

			// Lexical analysis
			lexer = new Lexer(input).get();

			// Warnings
			foreach (Info e in lexer.Warnings)
			{
				Res.Warnings.Add((Warning)e);
			}
			// See if there are any errors in the lexer
			if (lexer.Errors.Count > 0)
			{
				// Errors were found add them to total errors
				foreach (Info e in lexer.Errors)
				{
					Res.Errors.Add((Error)e);
				}
				// Return the errors with an almost empty array of bytes
				return new CompilerResult
				{
					Errors = Res.Errors,
					Warnings = Res.Warnings,
					Bytes = new byte[] { 0 },
				};
			}

			// Do half of the parser's work
			simplifier = new Simplifier(lexer.Tokens).get();

			// Warnings
			foreach (Info e in simplifier.Warnings)
			{
				Res.Warnings.Add((Warning)e);
			}

			// See if there are any errors in the simplifier
			if (simplifier.Errors.Count > 0)
			{
				// Errors were found, and add them to total errors
				foreach (Info e in simplifier.Errors)
				{
					Res.Errors.Add((Error)e);
				}
				// Return the errors with an almost empty array of bytes
				return new CompilerResult
				{
					Errors = Res.Errors,
					Warnings = Res.Warnings,
					Bytes = new byte[] { 0 },
				};
			}

			// Do the rest of the work
			parser = new Parser(simplifier.STokens).get();

			// Warnings
			foreach (Info e in parser.Warnings)
			{
				Res.Warnings.Add((Warning)e);
			}

			// See if there are any errors in the parser
			if (parser.Errors.Count > 0)
			{
				// Errors were found, and add them to total errors
				foreach (Info e in parser.Errors)
				{
					Res.Errors.Add((Error)e);
				}
				// Return the errors with an almost empty array of bytes
				return new CompilerResult
				{
					Errors = Res.Errors,
					Warnings = Res.Warnings,
					Bytes = new byte[] { 0 },
				};
			}

			// Do the rest of the work
			codegen = new CodeGenerator(parser.Tree).get();

			// Warnings
			foreach (Info e in codegen.Warnings)
			{
				Res.Warnings.Add((Warning)e);
			}

			// See if there are any errors in the parser
			if (parser.Errors.Count > 0)
			{
				// Errors were found, and add them to total errors
				foreach (Info e in codegen.Errors)
				{
					Res.Errors.Add((Error)e);
				}
				// Return the errors with an almost empty array of bytes
				return new CompilerResult
				{
					Errors = Res.Errors,
					Warnings = Res.Warnings,
					Bytes = new byte[] { 0 },
				};
			}

			// Return the result
			return new CompilerResult
			{
				Errors = Res.Errors,
				Warnings = Res.Warnings,
				Bytes = codegen.Code,
			};
		}
	}
}
