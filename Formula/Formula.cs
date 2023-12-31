﻿// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        List<string> tokens;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) : this(formula, s => s, s => true) { }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            ValidateSyntax(formula, normalize, isValid);
        }

        /// <summary>
        /// Private helper method called by both constructors which validates that the syntax in the formula being created is valid.
        /// Thus validity of the formula is ensured in the constructor, so all subsequent methods can be performed with the assumption
        /// that we are only dealing with valid formulas. Additionally, this formula normalizes all variables in the formula.
        /// </summary>
        /// <param name="formula">
        /// String formula passed from constructor
        /// </param>
        /// <param name="normalize">
        /// Normalize function passed from constructor
        /// </param>
        /// Normalize function passed from constructor
        /// <param name="isValid">
        /// Validate function passed from constructor
        /// </param>
        /// <exception cref="FormulaFormatException">
        /// Exception defined by an internal class, to be thrown if any syntax is invalid
        /// </exception>

        private void ValidateSyntax(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //Tokenize formula input
            tokens = new List<string>(GetTokens(formula));
            //Condense operators into an array so we can quickly check if a token is an operator
            string[] ops = { "+", "-", "*", "/" };

            Stack<string> openPar = new Stack<string>();
            Stack<string> closePar = new Stack<string>();

            //validates one token rule
            if (tokens.Count() < 1)
            {
                throw new FormulaFormatException("Incorrect syntax. No tokens contained in formula; add values to formula");
            }

            //validates starting token rule and ending token rule
            if (tokens[0] == ")" || ops.Contains(tokens[0]))
            {
                throw new FormulaFormatException("Incorrect syntax. Formula does not start with a number or parenthesis; add " +
                    "proper token to the start of the formula");
            }
            if (tokens[tokens.Count - 1] == "(" || ops.Contains(tokens[tokens.Count - 1]))
            {
                throw new FormulaFormatException("Incorrect syntax. Formula does not end with a number or parenthesis; add " +
                   "proper token to the end of the formula");
            }

            //for loop validates specific token rule; then checks further things to ensure correct syntax
            for (int i = 0; i < tokens.Count(); i++)
            {
                {
                    string v = tokens[i];
                    string next = "";
                    try
                    {
                        next = tokens[i + 1];
                    }
                    catch (ArgumentOutOfRangeException) { }

                    //parenthesis tracking validates right parenthesis rule
                    if (v.Equals("("))
                    {
                        openPar.Push(v);
                    }
                    if (v.Equals(")"))
                    {
                        closePar.Push(v);
                        if (closePar.Count > openPar.Count)
                        {
                            throw new FormulaFormatException("Incorrect syntax. Parenthesis mismatching. Review formula and fix parenthesis");
                        }
                    }

                    //validates parenthesis/operator following rule 
                    if (v.Equals("(") || ops.Contains(v))
                    {
                        if (!((double.TryParse(next, out double temp3) || next.Equals("(") || Regex.IsMatch(next, "^[_a-zA-Z]+[0-9]*$"))))
                        {
                            throw new FormulaFormatException("Incorrect syntax. All operators or open parenthesis must be followed by a " +
                                "number, variable, or open parenthesis; review that this is true");
                        }
                    }
                    //validates extra following rule. If the token is a variable, ensures that it is valid.
                    else if (double.TryParse(v, out double temp3) || v.Equals(")") || Regex.IsMatch(v, "^[_a-zA-Z]+[0-9]*$"))
                    {
                        if (Regex.IsMatch(v, "^[_a-zA-Z]+[0-9]*$"))
                        {
                            if (!normalizeVariable(normalize, isValid, i, v))
                            {
                                throw new FormulaFormatException("Incorrect syntax. Invalid variable names contained within formula; review " +
                                    "variable syntax");
                            }

                            tokens[i] = normalize(v);
                        }
                        if (!(next.Equals("") || next.Equals(")") || ops.Contains(next)))
                        {
                            throw new FormulaFormatException("Incorrect syntax. All numbers, variables, or closing parenthesis must be " +
                                "followed by an operator or closing parenthesis; review that this is true");
                        }
                    }
                    else
                    {
                        throw new FormulaFormatException("Incorrect syntax. Unrecognized token.");
                    }
                }
            }

            //validates balanced parenthesis rule
            if (openPar.Count != closePar.Count)
            {
                throw new FormulaFormatException("Incorrect syntax. Parenthesis mismatching. Review formula and fix parenthesis");
            }

            return;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> values = new Stack<double>();
            Stack<char> operators = new Stack<char>();

            foreach (string s in tokens)
            {
                string t = Regex.Replace(s, "^ +", "");
                t = Regex.Replace(t, " $+", "");
                if (t.Length > 0)
                {
                    if (Double.TryParse(t, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double temp) == true)
                    {
                        Digit(temp, values, operators);
                    }
                    else if (Regex.IsMatch(t, "^[a-zA-Z]+[0-9]+$"))
                    {
                        try
                        {
                            double value = lookup(t);
                            Digit(value, values, operators);
                        } catch (Exception)
                        {
                            return new FormulaError("Invalid operation. Undefined variable");
                        }
                    }
                    else if (t.Equals("+") || t.Equals("-"))
                    {
                        if (operators.Count > 0)
                        {
                            AddSub(values, operators);
                        }
                        operators.Push(t[0]);
                    }
                    else if (t.Equals("*") || t.Equals("/") || t.Equals("("))
                    {
                        operators.Push(t[0]);
                    }
                    else if (t.Equals(")"))
                    {
                        RightPar(values, operators);
                    }
                    //We can never get here
                    else
                    {
                        return new FormulaError("Invalid operation. This text should never be seen.");
                    }
                }
            }

            if (operators.Count() > 0)
            {
                AddSub(values, operators);
            }

            if (values.Peek() == double.PositiveInfinity)
            {
                return new FormulaError("Invalid operation. Division by 0 occurred");
            }

            return values.Pop();
        }

        /// <summary>
        /// Private helper method that uses fields from ValidateSyntax method to normalize and validate variables.
        /// </summary>
        private bool normalizeVariable(Func<string, string> normalize, Func<string, bool> isValid, int i, string v)
        {
            string norm = normalize(v);
            if (!isValid(norm))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Private helper method which runs the proper algorithm for dealing with a single numerical value in the 
        /// expression.
        /// </summary>
        /// <param name="t">
        /// Digit to be operated on.
        /// </param>
        private static void Digit(double t, Stack<double> values, Stack<char> operators)
        {
            if (operators.Count() > 0)
            {
                if (operators.Peek().Equals('*'))
                {
                    operators.Pop();
                    values.Push(values.Pop() * t);
                }
                else if (operators.Peek().Equals('/'))
                {
                    operators.Pop();
                    values.Push(values.Pop() / t);
                }
                else
                {
                    values.Push(t);
                }
            }
            else
            {
                values.Push(t);
            }
            return;
        }

        /// <summary>
        /// Private helper method which runs the proper algorithm for dealing with either a + or - operator.
        /// /// </summary>
        private static void AddSub(Stack<double> values, Stack<char> operators)
        {
            if (operators.Peek().Equals('+'))
            {
                operators.Pop();
                values.Push(values.Pop() + values.Pop());
            }
            else if (operators.Peek().Equals('-'))
            {
                operators.Pop();
                double num = values.Pop();
                values.Push(values.Pop() - num);
            }
            return;
        }

        /// <summary>
        /// Private helper method which runs the proper algorithm for dealing with a right parenthesis.
        /// </summary>
        private static void RightPar(Stack<double> values, Stack<char> operators)
        {
            AddSub(values, operators);

            operators.Pop();

            if (operators.Count() > 0)
            {
                if (operators.Peek().Equals('*'))
                {
                    operators.Pop();
                    values.Push(values.Pop() * values.Pop());
                }
                else if (operators.Peek().Equals('/'))
                {
                    operators.Pop();
                    double num = values.Pop();
                    values.Push(values.Pop() / num);
                }
            }
        }

    /// <summary>
    /// Enumerates the normalized versions of all of the variables that occur in this 
    /// formula.  No normalization may appear more than once in the enumeration, even 
    /// if it appears more than once in this Formula.
    /// 
    /// For example, if N is a method that converts all the letters in a string to upper case:
    /// 
    /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
    /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
    /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
    /// </summary>
    public IEnumerable<String> GetVariables()
        {
            HashSet<string> variables = new HashSet<string>();
            foreach (string t in tokens)
            {
                if (!(double.TryParse(t, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double temp) || t.Equals("+") || t.Equals("-") || t.Equals("*") || t.Equals("/") || t.Equals("(") || t.Equals(")")))
                {
                    variables.Add(t);
                }
            }
            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string v in tokens)
            {
                if (double.TryParse(v, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double num))
                {
                    sb.Append(num.ToString());
                }
                else
                {
                    sb.Append(v);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Formula))
            {
                return false;
            }

            return this.ToString().Equals(obj.ToString());
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (f1 is null || f2 is null)
            {
                throw new ArgumentNullException();
            }
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (f1 is null || f2 is null)
            {
                throw new ArgumentNullException();
            }
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 1;
            string s = ToString();
            for (int i = 0; i < s.Length; i++)
            {
                hash *= s[i].GetHashCode();
                hash *= i + 17;
            }
            return hash;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                          lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
