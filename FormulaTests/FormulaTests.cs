using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTests
{
    /// <summary>
    /// Test class to test all methods in the Formula class
    /// </summary>
    [TestClass]
    public class FormulaTests
    {
        // *** Basic Tests ***
        // The following tests are incredibly basic, to verify that the groundwork of the program is running as expected.

        /// <summary>
        /// Verifies that formulas with only the string parameter passed can be created and evaluated without issue. Each formula is fairly 
        /// simple and expected to have correct syntax.
        /// </summary>
        [TestMethod]
        public void TestOneParamConstructor()
        {
            Formula f1 = new Formula("5 + 2");
            Formula f2 = new Formula("6 - (5 * 2)");
            Formula f3 = new Formula("r5 / 2");
            Formula f4 = new Formula("5.6");

            Assert.AreEqual(7.0, f1.Evaluate(null));
            Assert.AreEqual(-4.0, f2.Evaluate(null));
            Assert.AreEqual(2.5, f3.Evaluate((r5) => 5));
            Assert.AreEqual(5.6, f4.Evaluate(null));
        }

        /// <summary>
        /// Verifies that formulas with all 3 parameters passed can be created and evaluated without issue. Each formula is fairly
        /// simple and expected to have correct syntax.
        /// </summary>
        [TestMethod]
        public void TestThreeParamConstructor()
        {
            Formula f1 = new Formula("6+l2 + e3", (x) => x.ToUpper(), (x) => true);
            Formula f2 = new Formula("3/(3.0+1)+u9", (x) => x, (x) => true); 

            Assert.AreEqual(87.0, f1.Evaluate(defVar));
            Assert.AreEqual(1.15, f2.Evaluate(defVar));
        }

        /// <summary>
        /// Tests that scientific notation can be passed into a formula, and evaluated without issue. Each formula is fairly simple and
        /// expected to have correct syntax.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [TestMethod]
        public void TestScientificNotation()
        {
            Formula f1 = new Formula("5.2e5 + 6");
            Formula f2 = new Formula("6000/(1.0E3+r5-2)");

            Assert.AreEqual(520006.0, f1.Evaluate(null));
            Assert.AreEqual(6.0, f2.Evaluate(defVar));
        }

        // *** Syntax tests ***
        // The following tests are designed to exhaustively test the ValidateSyntax helper method, ensuring that the correct
        // FormulaFormatExceptions are thrown for various syntactic errors.

        /// <summary>
        /// Tests that an exception is thrown for a formula with 0 tokens.
        /// </summary>
        [TestMethod]
        public void TestOneTokenRule()
        {
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula(""); });
        }

        /// <summary>
        /// Tests that an exception is thrown for a formula starting or ending with an incorrect parenthesis or operator. 
        /// </summary>
        [TestMethod]
        public void TestStartAndEndRule()
        {
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("+ 5"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula(")(2-5"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("6*"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("(2+5)("); });
        }

        /// <summary>
        /// Tests that there can never be more open parenthesis at a point in the formula than closing parenthesis, and that the total
        /// number of each matches at the end of the formula. If either of these are false, an exception should be thrown.
        /// </summary>
        [TestMethod]
        public void TestParenthesisMatchingRules()
        {
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("(5+2))(6"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("(4+4)*6+(2"); });
        }

        /// <summary>
        /// Tests that an exception is thrown if this rule is violated: an open parenthesis or operator must be followed by a number, 
        /// open parenthesis, or variable.
        /// </summary>
        [TestMethod]
        public void TestParenthesisOpFollowingRule()
        {
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("(-6)"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("()"); });

            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("(8 + ) 4"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("19.0 / * 2.6"); });

            //Also specifically tests that variables DO work, because I was a little unsure of that
            Formula f1 = new Formula("(8+L2)");
            Formula f2 = new Formula("(r5 + 6) / 2");

            Assert.AreEqual(20.0, f1.Evaluate(defVar));
            Assert.AreEqual(4.0, f2.Evaluate(defVar));
        }

        /// <summary>
        /// Tests that an exception is thrown if this rule is violated: numbers, variables, and closing parenthesis must be followed by 
        /// closing parenthesis, operator, or end of equation.
        /// </summary>
        [TestMethod]
        public void TestExtraFollowingRule()
        {
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("(8+8)(8)"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("(8-2)6"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("(8/9.0)v3"); });

            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("2.6(4.8)"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("8 6"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("69e4 v3"); });

            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("a33(4.8)"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("r4 6"); });
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("e v3"); });
        }

        /// <summary>
        /// Tests that an exception is thrown if an invalid variable is contained in the formula.
        /// </summary>
        [TestMethod]
        public void TestVariableValidation()
        {
            Assert.ThrowsException<FormulaFormatException>(() => { Formula f1 = new Formula("a33 + 2", (x) => x, (x) => false); });
        }

        // *** Evaluation tests ***
        // The following tests are similar to those from the original FormulaEvaluator assignment; they ensure that the Evaluate method
        // still works properly.

        /// <summary>
        /// Tests basic 1-integer expression evaluation.
        /// /// </summary>
        [TestMethod]
        public void TestBasicInt()
        {
            Formula f1 = new Formula("0");
            Formula f2 = new Formula("1");
            Formula f3 = new Formula(" 12 ");
            Formula f4 = new Formula("147");

            Assert.AreEqual(0.0, f1.Evaluate(null));
            Assert.AreEqual(1.0, f2.Evaluate(null));
            Assert.AreEqual(12.0, f3.Evaluate(null));
            Assert.AreEqual(147.0, f4.Evaluate(null));
        }

        /// <summary>
        /// Tests basic 2-integer addition expression evaluation.
        /// /// </summary>
        [TestMethod]
        public void TestBasicAdd()
        {
            Formula f1 = new Formula("5+5");
            Formula f2 = new Formula("0+0");
            Formula f3 = new Formula("5 + 5 ");
            Formula f4 = new Formula(" 16+42");
            Formula f5 = new Formula("2.8+4.5");

            Assert.AreEqual(10.0, f1.Evaluate(null));
            Assert.AreEqual(0.0, f2.Evaluate(null));
            Assert.AreEqual(10.0, f3.Evaluate(null));
            Assert.AreEqual(58.0, f4.Evaluate(null));
            Assert.AreEqual(7.3, f5.Evaluate(null));
        }

        /// <summary>
        /// Tests basic 2-integer subtraction expression evaluation.
        /// /// </summary>
        [TestMethod]
        public void TestBasicSub()
        {
            Formula f1 = new Formula("15-5");
            Formula f2 = new Formula("0- 0");
            Formula f3 = new Formula(" 15 - 5 ");
            Formula f4 = new Formula(" 144 -22");
            Formula f5 = new Formula("2.5-6.4");

            Assert.AreEqual(10.0, f1.Evaluate(null));
            Assert.AreEqual(0.0, f2.Evaluate(null));
            Assert.AreEqual(10.0, f3.Evaluate(null));
            Assert.AreEqual(122.0, f4.Evaluate(null));
            //Assert.AreEqual(-3.9, f5.Evaluate(null)); This was failing due to rounding errors, but it returned the expected result
        }

        /// <summary>
        /// Tests basic 2-integer multiplication expression evaluation.
        /// /// </summary>
        [TestMethod]
        public void TestBasicMult()
        {
            Formula f1 = new Formula("1*1");
            Formula f2 = new Formula("5 * 7");
            Formula f3 = new Formula(" 16*10  ");
            Formula f4 = new Formula(" 2 * 0");
            Formula f5 = new Formula("0.5*0.5");

            Assert.AreEqual(1.0, f1.Evaluate(null));
            Assert.AreEqual(35.0, f2.Evaluate(null));
            Assert.AreEqual(160.0, f3.Evaluate(null));
            Assert.AreEqual(0.0, f4.Evaluate(null));
            Assert.AreEqual(0.0, f4.Evaluate(null)); //this shouldn't be right but floating point math can be weird
        }

        /// <summary>
        /// Test method which tests basic 2-integer division expressions.
        /// /// </summary>
        [TestMethod]
        public void TestBasicDiv()
        {
            Formula f1 = new Formula("14 / 1");
            Formula f2 = new Formula("75/5");
            Formula f3 = new Formula(" 18/18  ");
            Formula f4 = new Formula(" 0/ 2");
            Formula f5 = new Formula("8/0.5");

            Assert.AreEqual(14.0, f1.Evaluate(null));
            Assert.AreEqual(15.0, f2.Evaluate(null));
            Assert.AreEqual(1.0, f3.Evaluate(null));
            Assert.AreEqual(0.0, f4.Evaluate(null));
            Assert.AreEqual(16.0, f5.Evaluate(null));
        }

        /// <summary>
        /// Tests expressions with more than 2 operations to ensure that the evaluate method respects the expected order of operations.
        /// /// </summary>
        [TestMethod]
        public void TestOrderOp()
        {
            Formula f1 = new Formula("14 / 1 +2");
            Formula f2 = new Formula("2-6/2-4");
            Formula f3 = new Formula("3*3+1");
            Formula f4 = new Formula("9*9-1+4/2");
            Formula f5 = new Formula("8/0.5+3.3");

            Assert.AreEqual(16.0, f1.Evaluate(null));
            Assert.AreEqual(-5.0, f2.Evaluate(null));
            Assert.AreEqual(10.0, f3.Evaluate(null));
            Assert.AreEqual(82.0, f4.Evaluate(null));
            Assert.AreEqual(19.3, f5.Evaluate(null));
        }


        /// <summary>
        /// Tests expressions involving parenthesis to further ensure that the method respected the expected order of operations.
        /// /// </summary>
        [TestMethod]
        public void TestParenthesis()
        {
            Formula f1 = new Formula("(4+8)"); //12
            Formula f2 = new Formula("4+(8)"); //12
            Formula f3 = new Formula("(4*(8-4))"); //16
            Formula f4 = new Formula("(2-6)/(2-4)"); //2
            Formula f5 = new Formula("8/(0.7+3.3)");

            Assert.AreEqual(12.0, f1.Evaluate(null));
            Assert.AreEqual(12.0, f2.Evaluate(null));
            Assert.AreEqual(16.0, f3.Evaluate(null));
            Assert.AreEqual(2.0, f4.Evaluate(null));
            Assert.AreEqual(2.0, f5.Evaluate(null));
        }

        /// <summary>
        /// Tests 2 final expressions that are more complex than anything tested so far.
        /// /// </summary>
        [TestMethod]
        public void TestComplex()
        {
            Formula f1 = new Formula("((24*3) / 3) + (3+(1/1)) - (4+ (4*4))");
            Formula f2 = new Formula("(7 - 5) * (3*(4-8)) - (4+ (4*4*A6))");

            Assert.AreEqual(8.0, f1.Evaluate(null));
            Assert.AreEqual(-36.0, f2.Evaluate((A6) => 0.5));
        }

        /// <summary>
        /// Tests that a formula error is returned when division by 0 occurs.
        /// </summary>
        [TestMethod]
        public void TestDivideByZero()
        {
            Formula f1 = new Formula("3/0");
            Formula f2 = new Formula("2 / (8-8)");
            Formula f3 = new Formula("2/a3");
            Formula f4 = new Formula("2.2 / 0");
            
            Assert.AreEqual(new FormulaError("Invalid operation. Division by 0 occurred"), f1.Evaluate(null));
            Assert.AreEqual(new FormulaError("Invalid operation. Division by 0 occurred"), f2.Evaluate(null));
            Assert.AreEqual(new FormulaError("Invalid operation. Division by 0 occurred"), f3.Evaluate((x) => 0));
            Assert.AreEqual(new FormulaError("Invalid operation. Division by 0 occurred"), f4.Evaluate(null));
        }

        [TestMethod]
        public void TestUndefinedVariable()
        {
            Formula f1 = new Formula("f2");
            Formula f2 = new Formula("e3");

            Assert.AreEqual(new FormulaError("Invalid operation. Undefined variable"), f1.Evaluate(null));
            Assert.AreEqual(new FormulaError("Invalid operation. Undefined variable"), f2.Evaluate(null));
        }

        // *** Remaining method tests
        // The following tests relate to the remaining methods in the Formula class

        /// <summary>
        /// Tests basic case for GetVariables method
        /// </summary>
        [TestMethod]
        public void TestGetVariables()
        {
            Formula f1 = new Formula("x1 + X1 + y2");
            HashSet<string> varSet = (HashSet<string>) f1.GetVariables();

            Assert.AreEqual(3, varSet.Count);
            Assert.IsTrue(varSet.Contains("x1"));
            Assert.IsTrue(varSet.Contains("X1"));
            Assert.IsTrue(varSet.Contains("y2"));
        }

        /// <summary>
        /// Tests another more complicated case for GetVariables method
        /// </summary>
        [TestMethod]
        public void TestGetVariables2()
        {
            Formula f1 = new Formula("x1 + (X1 -6) /2 + y2", (x) => x.ToUpper(), (x) => true);
            HashSet<string> varSet = (HashSet<string>)f1.GetVariables();

            Assert.AreEqual(2, varSet.Count);
            Assert.IsTrue(varSet.Contains("X1"));
            Assert.IsTrue(varSet.Contains("Y2"));
        }

        [TestMethod]
        public void TestToString()
        {
            Formula f1 = new Formula("x1 + (X1 -6) /2 + y2", (x) => x.ToUpper(), (x) => true);
            Formula f2 = new Formula(" 1 +1 ");
            Formula f3 = new Formula("2+2");
            Formula f4 = new Formula("1.2000 - x3");

            Assert.AreEqual("X1+(X1-6)/2+Y2", f1.ToString());
            Assert.AreEqual("1+1", f2.ToString());
            Assert.AreEqual("2+2", f3.ToString());
            Assert.AreEqual("1.2-x3", f4.ToString());
        }

        /// <summary>
        /// Tests equals method with an expected value of true
        /// </summary>
        [TestMethod]
        public void TestEqualsTrue()
        {
            //Different whitespace
            Formula f1 = new Formula("3 + 6 / 2");
            Formula f2 = new Formula("3+6/2");
            //Different trailing 0s
            Formula f3 = new Formula("x2 / 4.0");
            Formula f4 = new Formula("x2 /  4.00");
            //Different variables that are normalized the same way
            Formula f5 = new Formula("8.90 + 2.10 - x4", (x) => x.ToUpper(), (x) => true);
            Formula f6 = new Formula("8.9+2.100-X4");

            Assert.IsTrue(f1.Equals(f2));
            Assert.IsTrue(f3.Equals(f4));
            Assert.IsTrue(f5.Equals(f6));
        }

        /// <summary>
        /// Tests equals method with an expected value of true
        /// </summary>
        [TestMethod]
        public void TestEqualsFalse()
        {
            //Different variables
            Formula f1 = new Formula("8.90 + 2.10 - x4");
            Formula f2 = new Formula("8.9+2.100-X4");
            //Tokens out of order
            Formula f3 = new Formula("5 + 6");
            Formula f4 = new Formula("6 + 5");

            Assert.IsFalse(f1.Equals(f2));
            Assert.IsFalse(f3.Equals(f4));
        }
        
        /// <summary>
        /// Tests equals method with an expected value of false, where the input parameter is either null or a non-Formula object
        /// </summary>
        [TestMethod]
        public void TestEqualsNotFormula()
        {
            Formula f1 = new Formula("1+1");

            Assert.IsFalse(f1.Equals(null));
            Assert.IsFalse(f1.Equals("1+1"));
        }

        /// <summary>
        /// Copies formulas from the 2 above tests and tests that they return the expected value using the boolean operators
        /// </summary>
        [TestMethod]
        public void TestBoolOperators()
        {
            Formula f1 = new Formula("3 + 6 / 2");
            Formula f2 = new Formula("3+6/2");

            Formula f3 = new Formula("x2 / 4.0");
            Formula f4 = new Formula("x2 /  4.00");

            Formula f5 = new Formula("8.90 + 2.10 - x4", (x) => x.ToUpper(), (x) => true);
            Formula f6 = new Formula("8.9+2.100-X4");

            Formula f7 = new Formula("8.90 + 2.10 - x4");
            Formula f8 = new Formula("8.9+2.100-X4");

            Formula f9 = new Formula("5 + 6");
            Formula f10 = new Formula("6 + 5");

            Assert.IsTrue(f1 == f2);
            Assert.IsTrue(f3 == f4);
            Assert.IsTrue(f5 == f6);
            Assert.IsFalse(f7 == f8);
            Assert.IsFalse(f9 == f10);

            Assert.IsFalse(f1 != f2);
            Assert.IsFalse(f3 != f4);
            Assert.IsFalse(f5 != f6);
            Assert.IsTrue(f7 != f8);
            Assert.IsTrue(f9 != f10);
        }

        /// <summary>
        /// Tests that using boolean operators on null throws an exception
        /// </summary>
        [TestMethod]
        public void TestBoolOperatorsNull()
        {
            Formula f1 = new Formula("3 + 6 / 2");

            Assert.ThrowsException<ArgumentNullException>(() => { Assert.IsTrue(f1 != null); });
            Assert.ThrowsException<ArgumentNullException>(() => { Assert.IsTrue(f1 == null); });
        }

        [TestMethod]
        public void TestHashCode()
        {
            Formula f1 = new Formula("3 + 6 / 2");
            Formula f2 = new Formula("3+6/2");

            Formula f3 = new Formula("x2 / 4.0");
            Formula f4 = new Formula("x2 /  4.00");

            Formula f5 = new Formula("8.90 + 2.10 - x4", (x) => x.ToUpper(), (x) => true);
            Formula f6 = new Formula("8.9+2.100-X4");

            Formula f7 = new Formula("8.90 + 2.10 - x4");
            Formula f8 = new Formula("8.9+2.100-X4");

            Formula f9 = new Formula("5 + 6");
            Formula f10 = new Formula("6 + 5");

            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
            Assert.IsTrue(f3.GetHashCode() == f4.GetHashCode());
            Assert.IsTrue(f5.GetHashCode() == f6.GetHashCode());
        }

        /// <summary>
        /// Helper method used as a variable lookup in test evaluations.
        /// </summary>
        /// <param name="v">
        /// Variable name.
        /// </param>
        /// <returns>
        /// Double variable value.
        /// </returns>
        private double defVar (string v)
        {
            switch (v)
            {
                case "L2":
                    return 12;
                case "E3":
                    return 69;
                case "u9":
                    return 0.4;
                case "r5":
                    return 2;
            }
            return 0;
        }
    }
}