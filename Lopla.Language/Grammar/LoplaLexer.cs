/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 3.4.0.0
 */
using System.Collections.Generic;
using System.IO;
using Hime.Redist;
using Hime.Redist.Lexer;

namespace Lopla.Language.Grammar
{
	/// <summary>
	/// Represents a lexer
	/// </summary>
	internal class LoplaLexer : ContextFreeLexer
	{
		/// <summary>
		/// The automaton for this lexer
		/// </summary>
		private static readonly Automaton commonAutomaton = Automaton.Find(typeof(LoplaLexer), "LoplaLexer.bin");
		/// <summary>
		/// Contains the constant IDs for the terminals for this lexer
		/// </summary>
		public class ID
		{
			/// <summary>
			/// The unique identifier for terminal NEW_LINE
			/// </summary>
			public const int TerminalNewLine = 0x0003;
			/// <summary>
			/// The unique identifier for terminal TAB
			/// </summary>
			public const int TerminalTab = 0x0004;
			/// <summary>
			/// The unique identifier for terminal WHITE_SPACE
			/// </summary>
			public const int TerminalWhiteSpace = 0x0005;
			/// <summary>
			/// The unique identifier for terminal COMMENT
			/// </summary>
			public const int TerminalComment = 0x0006;
			/// <summary>
			/// The unique identifier for terminal SEPARATOR
			/// </summary>
			public const int TerminalSeparator = 0x0007;
			/// <summary>
			/// The unique identifier for terminal CHAR
			/// </summary>
			public const int TerminalChar = 0x0008;
			/// <summary>
			/// The unique identifier for terminal DIGIT
			/// </summary>
			public const int TerminalDigit = 0x0009;
			/// <summary>
			/// The unique identifier for terminal DECIMAL
			/// </summary>
			public const int TerminalDecimal = 0x000A;
			/// <summary>
			/// The unique identifier for terminal INTEGER
			/// </summary>
			public const int TerminalInteger = 0x000B;
			/// <summary>
			/// The unique identifier for terminal REAL
			/// </summary>
			public const int TerminalReal = 0x000C;
			/// <summary>
			/// The unique identifier for terminal NUMBER
			/// </summary>
			public const int TerminalNumber = 0x000D;
			/// <summary>
			/// The unique identifier for terminal STRING
			/// </summary>
			public const int TerminalString = 0x000E;
			/// <summary>
			/// The unique identifier for terminal IF
			/// </summary>
			public const int TerminalIf = 0x000F;
			/// <summary>
			/// The unique identifier for terminal FI
			/// </summary>
			public const int TerminalFi = 0x0010;
			/// <summary>
			/// The unique identifier for terminal THEN
			/// </summary>
			public const int TerminalThen = 0x0011;
			/// <summary>
			/// The unique identifier for terminal TO
			/// </summary>
			public const int TerminalTo = 0x0012;
			/// <summary>
			/// The unique identifier for terminal FROM
			/// </summary>
			public const int TerminalFrom = 0x0013;
			/// <summary>
			/// The unique identifier for terminal STEP
			/// </summary>
			public const int TerminalStep = 0x0014;
			/// <summary>
			/// The unique identifier for terminal WHILE
			/// </summary>
			public const int TerminalWhile = 0x0015;
			/// <summary>
			/// The unique identifier for terminal LITERAL
			/// </summary>
			public const int TerminalLiteral = 0x0016;
			/// <summary>
			/// The unique identifier for terminal OPERATOR_TERM
			/// </summary>
			public const int TerminalOperatorTerm = 0x0017;
		}
		/// <summary>
		/// Contains the constant IDs for the contexts for this lexer
		/// </summary>
		public class Context
		{
			/// <summary>
			/// The unique identifier for the default context
			/// </summary>
			public const int Default = 0;
		}
		/// <summary>
		/// The collection of terminals matched by this lexer
		/// </summary>
		/// <remarks>
		/// The terminals are in an order consistent with the automaton,
		/// so that terminal indices in the automaton can be used to retrieve the terminals in this table
		/// </remarks>
		private static readonly Symbol[] terminals = {
			new Symbol(0x0001, "ε"),
			new Symbol(0x0002, "$"),
			new Symbol(0x0003, "NEW_LINE"),
			new Symbol(0x0004, "TAB"),
			new Symbol(0x0005, "WHITE_SPACE"),
			new Symbol(0x0006, "COMMENT"),
			new Symbol(0x0007, "SEPARATOR"),
			new Symbol(0x0008, "CHAR"),
			new Symbol(0x0009, "DIGIT"),
			new Symbol(0x000A, "DECIMAL"),
			new Symbol(0x000B, "INTEGER"),
			new Symbol(0x000C, "REAL"),
			new Symbol(0x000D, "NUMBER"),
			new Symbol(0x000E, "STRING"),
			new Symbol(0x000F, "IF"),
			new Symbol(0x0010, "FI"),
			new Symbol(0x0011, "THEN"),
			new Symbol(0x0012, "TO"),
			new Symbol(0x0013, "FROM"),
			new Symbol(0x0014, "STEP"),
			new Symbol(0x0015, "WHILE"),
			new Symbol(0x0016, "LITERAL"),
			new Symbol(0x0017, "OPERATOR_TERM"),
			new Symbol(0x0038, "-"),
			new Symbol(0x0039, "("),
			new Symbol(0x003A, ")"),
			new Symbol(0x003B, "*"),
			new Symbol(0x003C, "/"),
			new Symbol(0x003D, "+"),
			new Symbol(0x003E, "&&"),
			new Symbol(0x003F, "||"),
			new Symbol(0x0040, "!="),
			new Symbol(0x0041, "=="),
			new Symbol(0x0042, "<"),
			new Symbol(0x0043, "<="),
			new Symbol(0x0044, ">"),
			new Symbol(0x0045, ">="),
			new Symbol(0x0046, ","),
			new Symbol(0x0048, "["),
			new Symbol(0x0049, "]"),
			new Symbol(0x004B, "."),
			new Symbol(0x004C, "="),
			new Symbol(0x004E, "{"),
			new Symbol(0x0050, "}"),
			new Symbol(0x0052, "function"),
			new Symbol(0x0053, "return") };
		/// <summary>
		/// Initializes a new instance of the lexer
		/// </summary>
		/// <param name="input">The lexer's input</param>
		public LoplaLexer(string input) : base(commonAutomaton, terminals, 0x0007, input) {}
		/// <summary>
		/// Initializes a new instance of the lexer
		/// </summary>
		/// <param name="input">The lexer's input</param>
		public LoplaLexer(TextReader input) : base(commonAutomaton, terminals, 0x0007, input) {}
	}
}
