using System.Text.RegularExpressions;

namespace FormulaEvaluator
{  
    /// <summary> 
    /// This is a class which evaluates arithmetic expressions, respecting ordinary order of operation. 
    /// </summary>
    public static class Evaluator
    {
        private static Stack<int> values;
        private static Stack<char> operators;

        public delegate int lookup(string varName);

        /// <summary>
        /// Evaluate method takes in the expression to be evaluated, then computes and returns the result as an integer. 
        /// The expression is formatted as a string consisting of non-negative integers, variables, operators, and parenthesis.
        /// </summary>
        /// <param name="expression">
        /// String parameter representing the expression to be evaluated. Must be formatted properly, with no extra operators or
        /// parenthsis, negative numbers, or invalid variables (variables must be one or more letter followed by one or
        /// more number). Any whitespace is ignored.
        /// </param>
        /// <param name="varEval">
        /// Parameter representing lookup, which is a delegate defined as any method that takes in a string parameter and 
        /// returns an int value. The method uses this to look up the value of any variables in the expression. If varEval
        /// returns a valid value for each variable, then that value is used in place of the variable in the expression.
        /// </param>
        /// <returns>
        /// The int value representing the final integer value of the expression. Returns 0 along with printing an error
        /// message if the expression is invalid in any way.
        /// </returns>
        public static int Evaluate(string expression, lookup varEval)
        {
            values = new Stack<int>();
            operators = new Stack<char>();
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach(string s in substrings)
            {
                string t = Regex.Replace(s, "^ +", "");
                t = Regex.Replace(t, " $+", "");
                if (t.Length > 0)
                {
                    if (char.IsDigit(t[0]) == true)
                    {
                        try
                        {
                            Digit(int.Parse(t));
                        }
                        catch (InvalidOperationException)
                        {
                            throw new ArgumentException();
                        }
                        catch (DivideByZeroException)
                        {
                            throw new ArgumentException();
                        }
                        catch (FormatException)
                        {
                            throw new ArgumentException();
                        }
                    }
                    else if (Regex.IsMatch(t, "^[a-zA-Z]+[0-9]+$"))
                    {
                        Variable(t, varEval);
                    }
                    else if (t.Equals("+") || t.Equals("-"))
                    {
                        try {
                            if (operators.Count > 0)
                            {
                                AddSub();
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            throw new ArgumentException();
                        }
                        operators.Push(t[0]);
                    }
                    else if (t.Equals("*") || t.Equals("/") || t.Equals("("))
                    {
                        operators.Push(t[0]);
                    }
                    else if (t.Equals(")"))
                    {
                        try
                        {
                            RightPar();
                        }
                        catch (InvalidOperationException)
                        {
                            throw new ArgumentException();
                        }
                        catch (DivideByZeroException)
                        {
                            throw new ArgumentException();
                        }
                    } else
                    {
                        throw new ArgumentException();
                    }
                }
            }

            if (operators.Count() > 0)
            {
                if(!(operators.Peek().Equals('+') || operators.Peek().Equals('-')) || operators.Count() > 1)
                {
                    throw new ArgumentException();
                }
                try
                {
                    AddSub();
                } 
                catch (InvalidOperationException)
                {
                    throw new ArgumentException();
                }
            }

            if (values.Count() == 1)
            {
                return values.Pop();
            } else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Private helper method which runs the proper algorithm for dealing with a single numerical value in the 
        /// expression.
        /// </summary>
        /// <param name="t">
        /// Digit to be operated on.
        /// </param>
        private static void Digit(int t)
        {
            if (operators.Count() > 0) {
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
            } else
            {
                values.Push(t);
            }
            return;
        }

        /// <summary>
        /// Private helper method which runs the proper algorithm for dealing with a single variable in the expression. 
        /// Uses the lookup delegate to find the numerical value of the variable, then calls the Digit method with that
        /// int as the parameter.
        /// </summary>
        /// <param name="varName">
        /// String representing the variable to be operated on.
        /// </param>
        /// <param name="varEval">
        /// Parameter representing lookup, which is the same delegate passed to the original Evaluate method. This delegate
        /// is used to evaluate the numerical value of the variable.
        /// </param>
        private static void Variable(string varName, lookup varEval)
        {
            int value = varEval(varName);
            Digit(value);
            return;
        }

        /// <summary>
        /// Private helper method which runs the proper algorithm for dealing with either a + or - operator.
        /// /// </summary>
        private static void AddSub()
        {
            if (operators.Peek().Equals('+'))
            {
                operators.Pop();
                values.Push(values.Pop() + values.Pop());
            }
            else if (operators.Peek().Equals('-'))
            {
                operators.Pop();
                int num = values.Pop();
                values.Push(values.Pop() - num);
            }
            return;
        }

        /// <summary>
        /// Private helper method which runs the proper algorithm for dealing with a right parenthesis.
        /// </summary>
        private static void RightPar()
        {
            AddSub();
            
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
                    int num = values.Pop();
                    values.Push(values.Pop() / num);
                }
            }
        }
      
    }
}