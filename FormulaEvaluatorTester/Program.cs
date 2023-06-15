// See https://aka.ms/new-console-template for more information

using FormulaEvaluator;

TestBasicInt();
TestBasicAdd();
TestBasicSub();
TestBasicMult();
TestBasicDiv();

TestOrderOp();
TestParenthesis();

//These error tests worked as expected.
//TestDigitErrors();
//TestRightParErrors();
//TestInvalidChars();

TestVariables();
TestComplex();

/// <summary>
/// Test method which tests basic 1-integer expressions.
/// /// </summary>
void TestBasicInt()
{
    Console.WriteLine(Evaluator.Evaluate("0", null)); //0
    Console.WriteLine(Evaluator.Evaluate("1", null)); //1
    Console.WriteLine(Evaluator.Evaluate("12", null)); //12
    Console.WriteLine(Evaluator.Evaluate("147", null)); //147
    Console.WriteLine();
}

/// <summary>
/// Test method which tests basic 2-integer addition expressions.
/// /// </summary>
void TestBasicAdd()
{
    Console.WriteLine(Evaluator.Evaluate("5+5", null)); //10
    Console.WriteLine(Evaluator.Evaluate("0+0", null)); //0
    Console.WriteLine(Evaluator.Evaluate("5 + 5", null)); //10
    Console.WriteLine(Evaluator.Evaluate(" 5  + 5", null)); //10
    Console.WriteLine(Evaluator.Evaluate("16+42", null)); //58
    Console.WriteLine(Evaluator.Evaluate("42+16", null)); //58
    Console.WriteLine();
}

/// <summary>
/// Test method which tests basic 2-integer subtraction expressions.
/// /// </summary>
void TestBasicSub()
{
    Console.WriteLine(Evaluator.Evaluate("15-5", null)); //10
    Console.WriteLine(Evaluator.Evaluate(" 15  - 5", null)); //10
    Console.WriteLine(Evaluator.Evaluate("3-7", null)); //-4
    Console.WriteLine(Evaluator.Evaluate("99-33", null)); //66
    Console.WriteLine();
}

/// <summary>
/// Test method which tests basic 2-integer multiplication expressions.
/// /// </summary>
void TestBasicMult()
{
    Console.WriteLine(Evaluator.Evaluate("1*1", null)); //1
    Console.WriteLine(Evaluator.Evaluate("5 * 7", null)); //35
    Console.WriteLine(Evaluator.Evaluate("16* 10 ", null)); //160
    Console.WriteLine(Evaluator.Evaluate("2*3 ", null)); //6
    Console.WriteLine();
}

/// <summary>
/// Test method which tests basic 2-integer division expressions.
/// /// </summary>
void TestBasicDiv()
{
    Console.WriteLine(Evaluator.Evaluate("14/1", null)); //14
    Console.WriteLine(Evaluator.Evaluate("6/3", null)); //2
    Console.WriteLine(Evaluator.Evaluate(" 75 / 5 ", null)); //15
    Console.WriteLine(Evaluator.Evaluate("8/5 ", null)); //1
    Console.WriteLine(Evaluator.Evaluate("18/18", null)); //1
    Console.WriteLine(Evaluator.Evaluate("0/5 ", null)); //0
    Console.WriteLine();
}

/// <summary>
/// Test method which tests expressions with more than 2 operations to ensure that the method respects the expected order of operations.
/// /// </summary>
void TestOrderOp()
{
    Console.WriteLine(Evaluator.Evaluate("14/1+2", null)); //16
    Console.WriteLine(Evaluator.Evaluate("2-6/2-4", null)); //-5
    Console.WriteLine(Evaluator.Evaluate("3*3+1", null)); //10
    Console.WriteLine(Evaluator.Evaluate("9*9-1+4/2", null)); //82
    Console.WriteLine();
}

/// <summary>
/// Test method which tests expressions involving parenthesis to further ensure that the method respected the expected order of operations.
/// /// </summary>
void TestParenthesis()
{
    Console.WriteLine(Evaluator.Evaluate("(4+8)", null)); //12
    Console.WriteLine(Evaluator.Evaluate("4+(8)", null)); //12
    Console.WriteLine(Evaluator.Evaluate("(4*(8-4))", null)); //16
    Console.WriteLine(Evaluator.Evaluate("(2-6)/(2-4)", null)); //2
    Console.WriteLine();
}


/// <summary>
/// Test method which catches possible errors resulting from digit processing.
/// /// </summary>
void TestDigitErrors()
{
    Console.WriteLine(Evaluator.Evaluate("*6", null)); //empty value stack
    Console.WriteLine(Evaluator.Evaluate("-4", null)); //empty value stack
    Console.WriteLine(Evaluator.Evaluate("6--6", null)); //empty value stack
    Console.WriteLine(Evaluator.Evaluate("4+", null)); //empty value stack
    Console.WriteLine(Evaluator.Evaluate("1 1", null)); //empty value stack
    Console.WriteLine(Evaluator.Evaluate("10 / 0", null)); //divide by 0
    Console.WriteLine(Evaluator.Evaluate("0/0", null)); //divide by 0
    Console.WriteLine();
}

/// <summary>
/// Test method which catches possible errors resulting from right parenthesis processing.
/// /// </summary>
void TestRightParErrors()
{
    Console.WriteLine(Evaluator.Evaluate("6)", null)); //incorrect formatting
    Console.WriteLine(Evaluator.Evaluate("))", null)); //incorrect formatting
    Console.WriteLine(Evaluator.Evaluate("1+2(3)", null)); //leftover values
    Console.WriteLine(Evaluator.Evaluate("(2-2))", null)); //incorrect formatting
    Console.WriteLine(Evaluator.Evaluate("2-2)", null)); //incorrect formatting
    Console.WriteLine(Evaluator.Evaluate("(2-8", null)); //leftover operators
    Console.WriteLine(Evaluator.Evaluate("(1*1", null)); //leftover operators
    Console.WriteLine(Evaluator.Evaluate("(4/0)", null)); //divide by 0
    Console.WriteLine(Evaluator.Evaluate("12/(4-4)", null)); //divide by 0
    Console.WriteLine(Evaluator.Evaluate("()", null)); //leftover values
    Console.WriteLine();
}

/// <summary>
/// Test method which catches possible errors resulting from invalid characters.
/// /// </summary>
void TestInvalidChars()
{
    Console.WriteLine(Evaluator.Evaluate("d", null)); //invalid character
    Console.WriteLine(Evaluator.Evaluate("eEeEe", null)); //invalid character
    Console.WriteLine(Evaluator.Evaluate("!", null)); //invalid character
    Console.WriteLine(Evaluator.Evaluate("9F", null)); //invalid character
    Console.WriteLine(Evaluator.Evaluate("d9d", null)); //invalid character
    Console.WriteLine(Evaluator.Evaluate("d9 + 2", null)); //invalid character
    Console.WriteLine();
}

/// <summary>
/// Test method which tests expressions involving variables.
/// /// </summary>
void TestVariables()
{
    Console.WriteLine(Evaluator.Evaluate("x2", (x2) => 5)); //5
    Console.WriteLine(Evaluator.Evaluate("3+A1*A2", defVars)); //103
    Console.WriteLine(Evaluator.Evaluate("4*(1+AFXe223)/e9", defVars)); //18
    Console.WriteLine();
}

/// <summary>
/// Helper method to be passed as a lookup in tests involving variables.
/// /// </summary>
int defVars (string var)
{
    if (var.Equals("A1"))
    {
        return 5;
    } else if (var.Equals("A2")) 
    {
        return 20;
    } else if (var.Equals("AFXe223")) 
    {
        return 44;
    } else
    {
        return 10;
    }
}

/// <summary>
/// Test method which tests 2 final expressions that are more complex than anything tested so far.
/// /// </summary>
void TestComplex()
{
    Console.WriteLine(Evaluator.Evaluate("((24*3) / 3) + (3+(1/1)) - (4+ (4*4))", null)); //8
    Console.WriteLine(Evaluator.Evaluate("(7 - 5) * (3*(4-8)) - (4+ (4*A6))", (A6) => 2)); //-36
}

