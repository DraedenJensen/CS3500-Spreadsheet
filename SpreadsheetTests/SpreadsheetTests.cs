/// <summary>
/// Author:    Draeden Jensen
/// Partner:   None
/// Date:      06-20-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Draeden Jensen - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Draeden Jensen, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents:
/// Tests for the Spreadsheet class.
/// </summary>

using SpreadsheetUtilities;
using SS;
using System.Xml;

namespace SpreadsheetTests
{
    /// <summary>
    /// Test class providing tests for the full Spreadsheet project.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        // ***** A4 Tests *****
        // These tests were originally written for the Assignment 4 version of the Spreadsheet, and were then refactored to work with
        // The new specifications for Assignment 5.

        /// <summary>
        /// The most basic test -- tests that a Spreadsheet object can be created without issue.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            Spreadsheet ss = new Spreadsheet();

            Assert.IsNotNull(ss);
        }

        /// <summary>
        /// Tests adding numbers to cells, and tests that the GetCellContents method returns the expected value.
        /// </summary>
        [TestMethod]
        public void TestAddNumber()
        {
            Spreadsheet ss = new Spreadsheet();

            List<string> a1 = (List<string>)ss.SetContentsOfCell("a1", "2.3");
            Assert.AreEqual(1, a1.Count);
            Assert.IsTrue(a1.Contains("a1"));

            List<string> b1 = (List<string>)ss.SetContentsOfCell("b1", "4");
            Assert.AreEqual(1, b1.Count);
            Assert.IsTrue(b1.Contains("b1"));

            List<string> a2 = (List<string>)ss.SetContentsOfCell("a2", "0.0");
            Assert.AreEqual(1, a2.Count);
            Assert.IsTrue(a2.Contains("a2"));

            List<string> c4 = (List<string>)ss.SetContentsOfCell("c4", "-1.2");
            Assert.AreEqual(1, c4.Count);
            Assert.IsTrue(c4.Contains("c4"));
        }

        /// <summary>
        /// Tests adding text to cells, and tests that the GetCellContents method returns the expected values.
        /// </summary>
        [TestMethod]
        public void TestAddString()
        {
            Spreadsheet ss = new Spreadsheet();

            List<string> a1 = (List<string>)ss.SetContentsOfCell("a1", "a1");
            Assert.AreEqual(1, a1.Count);
            Assert.IsTrue(a1.Contains("a1"));

            List<string> b1 = (List<string>)ss.SetContentsOfCell("b1", " ");
            Assert.AreEqual(1, b1.Count);
            Assert.IsTrue(b1.Contains("b1"));

            List<string> a2 = (List<string>)ss.SetContentsOfCell("a2", "Hello World!");
            Assert.AreEqual(1, a2.Count);
            Assert.IsTrue(a2.Contains("a2"));

            List<string> c4 = (List<string>)ss.SetContentsOfCell("c4", "42 + 8");
            Assert.AreEqual(1, c4.Count);
            Assert.IsTrue(c4.Contains("c4"));
        }

        /// <summary>
        /// Tests adding formulas to cells, and tests that the GetCellContents method returns the expected values. This test
        /// just deals with the basic action of adding formulas to the spreadsheet; we are not yet dealing with dependencies.
        /// </summary>
        [TestMethod]
        public void TestAddFormulaSimple()
        {
            Spreadsheet ss = new Spreadsheet();

            Formula f1 = new Formula("4 + 8");
            Formula f2 = new Formula("9-0");
            Formula f3 = new Formula("18 / 2");

            List<string> a1 = (List<string>)ss.SetContentsOfCell("a1", "=" + f1.ToString());
            Assert.AreEqual(1, a1.Count);
            Assert.IsTrue(a1.Contains("a1"));

            List<string> b1 = (List<string>)ss.SetContentsOfCell("b1", "=" + f2.ToString());
            Assert.AreEqual(1, b1.Count);
            Assert.IsTrue(b1.Contains("b1"));

            List<string> a2 = (List<string>)ss.SetContentsOfCell("a2", "=" + f3.ToString());
            Assert.AreEqual(1, a2.Count);
            Assert.IsTrue(a2.Contains("a2"));
        }

        /// <summary>
        /// Tests more complicated things with formulas, updating dependencies and ensuring
        /// that updating contents continues to return the correct values
        /// </summary>
        [TestMethod]
        public void TestAddFormulasWithDependencies()
        {
            Spreadsheet ss = new Spreadsheet();

            Formula f1 = new Formula("4 + c3");
            Formula f2 = new Formula("9-a1");
            Formula f3 = new Formula("18 / a1");
            Formula f4 = new Formula("a2 + b1");

            List<string> a1 = (List<string>)ss.SetContentsOfCell("a1", "=" + f1.ToString());
            Assert.AreEqual(1, a1.Count);
            Assert.IsTrue(a1.Contains("a1"));

            List<string> b1 = (List<string>)ss.SetContentsOfCell("b1", "=" + f2.ToString()  );
            Assert.AreEqual(1, b1.Count);
            Assert.IsTrue(b1.Contains("b1"));

            List<string> a2 = (List<string>)ss.SetContentsOfCell("a2", "=" + f3.ToString());
            Assert.AreEqual(1, a2.Count);
            Assert.IsTrue(a2.Contains("a2"));

            List<string> c4 = (List<string>)ss.SetContentsOfCell("c4", "=" + f4.ToString());
            Assert.AreEqual(1, c4.Count);
            Assert.IsTrue(c4.Contains("c4"));

            a1 = (List<string>)ss.SetContentsOfCell("a1", "=" + f1);
            Assert.AreEqual(4, a1.Count);
            Assert.IsTrue(a1.Contains("a1") && a1.Contains("a2") && a1.Contains("b1") && a1.Contains("c4"));

            b1 = (List<string>)ss.SetContentsOfCell("b1", "=" + f2);
            Assert.AreEqual(2, b1.Count);
            Assert.IsTrue(b1.Contains("b1") && b1.Contains("c4"));

            a2 = (List<string>)ss.SetContentsOfCell("a2", "=" + f3.ToString());
            Assert.AreEqual(2, a2.Count);
            Assert.IsTrue(a2.Contains("a2") && a2.Contains("c4"));
        }

        /// <summary>
        /// Tests Regex matching by testing a variety of cell names that should be valid.
        /// </summary>
        [TestMethod]
        public void TestValidVariableNames()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("z1", "2");
            ss.SetContentsOfCell("xxeDgi443", "2");
            ss.SetContentsOfCell("Zed03", "2");

            HashSet<string> cells = (HashSet<string>)ss.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(3, cells.Count);
        }

        /// <summary>
        /// Tests GetCellContents method with cells containing numbers.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsNumbers()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "2.3");
            ss.SetContentsOfCell("b1", "4");
            ss.SetContentsOfCell("a2", "0.0");
            ss.SetContentsOfCell("c4", "-1.2");

            Assert.AreEqual(2.3, ss.GetCellContents("a1"));
            Assert.AreEqual(4.0, ss.GetCellContents("b1"));
            Assert.AreEqual(0.0, ss.GetCellContents("a2"));
            Assert.AreEqual(-1.2, ss.GetCellContents("c4"));
        }

        /// <summary>
        /// Tests GetCellContents method with cells containing text.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsString()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "a1");
            ss.SetContentsOfCell("b1", " ");
            ss.SetContentsOfCell("a2", "Hello World!");
            ss.SetContentsOfCell("c4", "42 + 8");

            Assert.AreEqual("a1", ss.GetCellContents("a1"));
            Assert.AreEqual(" ", ss.GetCellContents("b1"));
            Assert.AreEqual("Hello World!", ss.GetCellContents("a2"));
            Assert.AreEqual("42 + 8", ss.GetCellContents("c4"));
        }

        /// <summary>
        /// Tests GetCellContents method with cells containing formulas.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsFormula()
        {
            Spreadsheet ss = new Spreadsheet();

            Formula f1 = new Formula("4 + c3");
            Formula f2 = new Formula("9-a1");
            Formula f3 = new Formula("18 / a1");
            Formula f4 = new Formula("a2 + b1");

            ss.SetContentsOfCell("a1", "=" + f1.ToString());
            ss.SetContentsOfCell("b1", "=" + f2.ToString());
            ss.SetContentsOfCell("a2", "=" + f3.ToString());
            ss.SetContentsOfCell("c4", "=" + f4.ToString());

            Assert.AreEqual(f1, ss.GetCellContents("a1"));
            Assert.AreEqual(f2, ss.GetCellContents("b1"));
            Assert.AreEqual(f3, ss.GetCellContents("a2"));
            Assert.AreEqual(f4, ss.GetCellContents("c4"));
        }

        /// <summary>
        /// Tests that a single spreadsheet can hold cells holding multiple types of data without issue.
        /// Ensures that cells holding formulas can point to cells holding text or numbers.
        /// </summary>
        [TestMethod]
        public void TestDifferentCellTypes()
        {
            Spreadsheet ss = new Spreadsheet();

            Formula f1 = new Formula("4 + a1");
            Formula f2 = new Formula("9-b1");

            List<string> a2 = (List<string>)ss.SetContentsOfCell("a2", "=" + f1.ToString());
            List<string> c4 = (List<string>)ss.SetContentsOfCell("c4", "=" + f2.ToString());
            List<string> a1 = (List<string>)ss.SetContentsOfCell("a1", "12");
            List<string> b1 = (List<string>)ss.SetContentsOfCell("b1", "a3");

            Assert.AreEqual(12.0, ss.GetCellContents("a1"));
            Assert.AreEqual("a3", ss.GetCellContents("b1"));
            Assert.AreEqual(f1, ss.GetCellContents("a2"));
            Assert.AreEqual(f2, ss.GetCellContents("c4"));

            Assert.AreEqual(1, a2.Count);
            Assert.AreEqual(1, c4.Count);

            Assert.AreEqual(2, a1.Count);
            Assert.IsTrue(a1.Contains("a2") && a1.Contains("a1"));

            Assert.AreEqual(2, b1.Count);
            Assert.IsTrue(b1.Contains("c4") && b1.Contains("b1"));
        }

        /// <summary>
        /// Tests that a cell's contents can be changed between the three types without issue. A cell
        /// that is dependet on this cell should remain dependent across these changes.
        /// </summary>
        [TestMethod]
        public void TestChangeCellType()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula f1 = new Formula("b2 + 4");
            ss.SetContentsOfCell("a2", "=" + new Formula("4 + a1").ToString());

            List<string> a1 = (List<string>)ss.SetContentsOfCell("a1", "12");
            Assert.AreEqual(12.0, ss.GetCellContents("a1"));
            Assert.IsTrue(a1.Contains("a2"));

            a1 = (List<string>)ss.SetContentsOfCell("a1", "a1");
            Assert.AreEqual("a1", ss.GetCellContents("a1"));
            Assert.IsTrue(a1.Contains("a2"));

            a1 = (List<string>)ss.SetContentsOfCell("a1", "=" + f1.ToString());
            Assert.AreEqual(f1, ss.GetCellContents("a1"));
            Assert.IsTrue(a1.Contains("a2"));
        }

        /// <summary>
        /// Tests that a nonempty cell's contents can be changed back to empty without issue. A cell
        /// that is dependent on this cell should remain dependent after this change.
        /// </summary>
        [TestMethod]
        public void TestSetCellBackToEmpty()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a2", "=" + new Formula("4 + a1").ToString());

            List<string> a1 = (List<string>)ss.SetContentsOfCell("a1", "This is a string!");
            Assert.AreEqual("This is a string!", ss.GetCellContents("a1"));
            Assert.IsTrue(a1.Contains("a2"));

            a1 = (List<string>)ss.SetContentsOfCell("a1", "");
            Assert.AreEqual("", ss.GetCellContents("a1"));
            Assert.AreEqual(2, a1.Count);
            Assert.IsTrue(a1.Contains("a1") && a1.Contains("a2"));
        }

        /// <summary>
        /// Tests that a cell can be dependent on an empty cell.
        /// </summary>
        [TestMethod]
        public void TestDependentOnEmptyCell()
        {
            Spreadsheet ss = new Spreadsheet();

            Formula f1 = new Formula("1 + a2");

            ss.SetContentsOfCell("a1", "=" + f1.ToString());

            Assert.AreEqual("", ss.GetCellContents("a2"));
        }

        [TestMethod]
        public void TestGetEmptyCell()
        {
            Spreadsheet ss = new Spreadsheet();

            Assert.AreEqual("", ss.GetCellContents("a2"));
            Assert.AreEqual("", ss.GetCellContents("x4"));
        }

        /// <summary>
        /// Tests that if no cells have been added to the spreadsheet, GetNamesOfAllNonemptyCells returns
        /// an empty set.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsOnEmtpySS()
        {
            Spreadsheet ss = new Spreadsheet();

            HashSet<string> cells = (HashSet<string>)ss.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, cells.Count);
        }

        /// <summary>
        /// Tests GetNamesOfAllNonemptyCells after adding content to cells.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsAfterAdding()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "hey");
            ss.SetContentsOfCell("a2", "12.2");
            ss.SetContentsOfCell("b1", "=" + new Formula("12 + a1").ToString());

            HashSet<string> cells = (HashSet<string>)ss.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(3, cells.Count);
            Assert.IsTrue(cells.Contains("a1") && cells.Contains("a2") && cells.Contains("b1"));
        }

        /// <summary>
        /// Tests GetNamesOfAllNonemptyCells after adding content to cells, then updating that content.
        /// Each cell name should still only be listed once.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsAfterUpdating()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "hey");
            ss.SetContentsOfCell("a2", "12.2");
            ss.SetContentsOfCell("b1", "=" + new Formula("12 + a1").ToString());

            ss.SetContentsOfCell("c4", "0.0");
            ss.SetContentsOfCell("a1", "a1 updated");
            ss.SetContentsOfCell("b1", "4.4");

            HashSet<string> cells = (HashSet<string>)ss.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(4, cells.Count);
            Assert.IsTrue(cells.Contains("a1") && cells.Contains("a2") && cells.Contains("b1") && cells.Contains("c4"));
        }

        /// <summary>
        /// Tests GetNamesOfAllNonemptyCells after adding content to cells, then removing that content.
        /// Any cell that is empty should not be included in the resulting set, even if it was nonempty
        /// at some point in the past.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsAfterRemoving()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "hey");
            ss.SetContentsOfCell("a2", "12.2");
            ss.SetContentsOfCell("b1", "=" + new Formula("12 + a1").ToString());

            HashSet<string> cells = (HashSet<string>)ss.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(3, cells.Count);

            ss.SetContentsOfCell("a1", "");
            ss.SetContentsOfCell("b1", "");

            cells = (HashSet<string>)ss.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(1, cells.Count);
            Assert.IsTrue(cells.Contains("a2"));
        }

        /// <summary>
        /// Tests that an InvalidNameException is thrown when cell contents are updated with a variety
        /// of invalid cell names.
        /// </summary>
        [TestMethod]
        public void TestInvalidNameExceptionSetContent()
        {
            Spreadsheet ss = new Spreadsheet();

            Assert.ThrowsException<InvalidNameException>(() =>
            {
                ss.SetContentsOfCell("3", "three");
            });
            Assert.ThrowsException<InvalidNameException>(() =>
            {
                ss.SetContentsOfCell("3x", "2.3");
            });
            Assert.ThrowsException<InvalidNameException>(() =>
            {
                ss.SetContentsOfCell("x2><", "=" + new Formula("6").ToString());
            });
            Assert.ThrowsException<InvalidNameException>(() =>
            {
                ss.SetContentsOfCell("x8x", "=" + new Formula("6").ToString());
            });
        }

        /// <summary>
        /// Tests that an InvalidNameException is thrown when GetCellContents is called with an invalid
        /// cell name.
        /// </summary>
        [TestMethod]
        public void TestInvalidNameExceptionGetContent()
        {
            Spreadsheet ss = new Spreadsheet();

            Assert.ThrowsException<InvalidNameException>(() =>
            {
                ss.GetCellContents("3");
            });
            Assert.ThrowsException<InvalidNameException>(() =>
            {
                ss.GetCellContents("3x");
            });
            Assert.ThrowsException<InvalidNameException>(() =>
            {
                ss.GetCellContents("x2><");
            });
            Assert.ThrowsException<InvalidNameException>(() =>
            {
                ss.GetCellContents("x8x");
            });
        }

        /// <summary>
        /// Tests that a CircularException is thrown when it should be. 
        /// </summary>
        [TestMethod]
        public void TestCircularException()
        {
            Spreadsheet ss = new Spreadsheet();

            Assert.ThrowsException<CircularException>(() =>
            {
                ss.SetContentsOfCell("a1", "=" + new Formula("a2 + 2").ToString());
                ss.SetContentsOfCell("a2", "=" + new Formula("a1 + 2").ToString());
            });

            Assert.ThrowsException<CircularException>(() =>
            {
                ss.SetContentsOfCell("a1", "=" + new Formula("a2 + 2").ToString());
                ss.SetContentsOfCell("a2", "=" + new Formula("a3 + b4").ToString());
                ss.SetContentsOfCell("a3", "=" + new Formula("a4 + b7").ToString());
                ss.SetContentsOfCell("ab4", "=" + new Formula("a1 + b4").ToString());
            });
        }

        // ***** A5 Tests *****
        // These tests were purpose-built to test the new changes and additions made in the Assignment 5 version of the Spreadsheet.

        /// <summary>
        /// Tests that the 3-parameter constructor can build a Spreadsheet object without issue.
        /// </summary>
        [TestMethod]
        public void TestThreeParamConstructor()
        {
            Spreadsheet ss = new Spreadsheet(IsValid, Normalize, "default");

            Assert.IsNotNull(ss);
            Assert.AreEqual(IsValid, ss.IsValid);
            Assert.AreEqual(Normalize, ss.Normalize);
            Assert.AreEqual("default", ss.Version);
        }

        /// <summary>
        /// Tests that an IsValid function passed to the 3-parameter constructor limits variable names as expected.
        /// </summary>
        [TestMethod]
        public void TestIsValid()
        {
            Spreadsheet ss = new Spreadsheet(IsValid, Normalize, "default");
            Spreadsheet ssDefault = new Spreadsheet();

            ssDefault.SetContentsOfCell("a11", "this should be valid!");
            Assert.ThrowsException<InvalidNameException>(() => { ss.SetContentsOfCell("a11", "2 char names only!!"); });
        }

        /// <summary>
        /// Tests that a Normalize function passed to the 3-parameter constructor works as expected.
        /// </summary>
        [TestMethod]
        public void TestNormalize()
        {
            Spreadsheet ss = new Spreadsheet(IsValid, Normalize, "default");
            Spreadsheet ssDefault = new Spreadsheet();

            ssDefault.SetContentsOfCell("a1", "lower case");
            ss.SetContentsOfCell("a1", "normalized to upper case");

            Assert.AreEqual("lower case", ssDefault.GetCellContents("a1"));
            Assert.AreEqual("normalized to upper case", ss.GetCellContents("a1"));
            Assert.AreEqual("", ssDefault.GetCellContents("A1"));
            Assert.AreEqual("normalized to upper case", ss.GetCellContents("A1"));
        }

        /// <summary>
        /// Sample IsValid function to be passed for testing 3-parameter constructor. Only allows variable names that are exactly 
        /// 2 characters long.
        /// </summary>
        /// <param name="v"> Variable name to be validated </param>
        /// <returns> bool signifying whether the variable is considered valid </returns>
        private bool IsValid(string v)
        {
            if (v.Length == 2)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sample Normalize function to be passed for testing 3-parameter constructor. Simply converts all variable names to upper
        /// case.
        /// </summary>
        /// <param name="v"> Variable name to be normalized </param>
        /// <returns> Normalized form of the variable name </returns>
        private string Normalize(string v)
        {
            return v.ToUpper();
        }

        /// <summary>
        /// Tests that cell values end up being what we would expect when adding string contents to cells.
        /// </summary>
        [TestMethod]
        public void TestValueText()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "Hello World!");
            ss.SetContentsOfCell("A1", " ");
            ss.SetContentsOfCell("a2", "8x + 2");

            Assert.AreEqual(ss.GetCellValue("a1"), "Hello World!");
            Assert.AreEqual(ss.GetCellValue("A1"), " ");
            Assert.AreEqual(ss.GetCellValue("a2"), "8x + 2");
            Assert.AreEqual(ss.GetCellValue("b1"), "");
        }

        /// <summary>
        /// Tests that a cell's value is updated after it is changed.
        /// </summary>
        [TestMethod]
        public void TestValueAfterChange()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "Hello World!");
            ss.SetContentsOfCell("a1", "Goodbye World.");

            Assert.AreEqual(ss.GetCellValue("a1"), "Goodbye World.");
        }

        /// <summary>
        /// Tests that cell values end up being what we would expect when adding numerical contents to cells. Tests
        /// an integer, a double, 0, a negative number, and a number in scientific notation.
        /// </summary>
        [TestMethod]
        public void TestValueDouble()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "12");
            ss.SetContentsOfCell("a2", "3.3");
            ss.SetContentsOfCell("b1", "0");
            ss.SetContentsOfCell("c4", "-1.1");
            ss.SetContentsOfCell("d5", "3e1");

            Assert.AreEqual(ss.GetCellValue("a1"), 12.0);
            Assert.AreEqual(ss.GetCellValue("a2"), 3.3);
            Assert.AreEqual(ss.GetCellValue("b1"), 0.0);
            Assert.AreEqual(ss.GetCellValue("c4"), -1.1);
            Assert.AreEqual(ss.GetCellValue("d5"), 30.0);
        }

        /// <summary>
        /// Tests that cell values end up being what we would expect when adding Formulas as contents to cells. No 
        /// dependencies or FormulaErrors are tested yet.
        /// </summary>
        [TestMethod]
        public void TestValueFormulaSimple() 
        {
            Spreadsheet ss = new Spreadsheet();
            Formula f1 = new Formula("5 + 5");

            ss.SetContentsOfCell("a1", "=" + f1.ToString());
            ss.SetContentsOfCell("a2", "=2.5 * (3 - 2)");
            ss.SetContentsOfCell("b1", "=7.4");

            Assert.AreEqual(ss.GetCellValue("a1"), 10.0);
            Assert.AreEqual(ss.GetCellValue("a2"), 2.5);
            Assert.AreEqual(ss.GetCellValue("b1"), 7.4);
        }

        /// <summary>
        /// Tests that if a formula involving division by zero is added to a cell, that cell's value is a FormulaError.
        /// </summary>
        [TestMethod]
        public void TestValueFormulaErrorDivisionByZero()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "=4 / 0");
            ss.SetContentsOfCell("a2", "=7 / (3 - 3)");

            Assert.IsTrue(ss.GetCellValue("a1") is FormulaError);
            Assert.IsTrue(ss.GetCellValue("a2") is FormulaError);
        }

        /// <summary>
        /// Tests that if a formula which is dependent on a cell containing a non-double value added to a cell, that cell's 
        /// value is a FormulaError.
        /// </summary>
        [TestMethod]
        public void TestValueFormulaErrorDependency()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a5", "yo");
            ss.SetContentsOfCell("a6", "=4/0");

            //Dependent on empty cell
            ss.SetContentsOfCell("a1", "=a4 + 2");
            Assert.IsTrue(ss.GetCellValue("a1") is FormulaError);

            //Dependent on undefined variable
            ss.SetContentsOfCell("a2", "=a5 + 7");
            Assert.IsTrue(ss.GetCellValue("a2") is FormulaError);

            //Dependent on division by 0
            ss.SetContentsOfCell("a3", "=1 + a6");
            Assert.IsTrue(ss.GetCellValue("a3") is FormulaError);
        }

        /// <summary>
        /// Tests that cell values end up being what we would expect when adding Formulas as contents to cells. These 
        /// formulas are dependent on each other.
        /// </summary>
        [TestMethod]
        public void TestValueFormulaWithDependencies()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "=1 + 1");
            ss.SetContentsOfCell("a2", "=2/(a1 + 6)");
            ss.SetContentsOfCell("a3", "=a2 * a1 * 2");
            ss.SetContentsOfCell("a4", "=a3 + (36.0 / (6 + 6))");
            ss.SetContentsOfCell("a5", "=a4 + a2");

            Assert.AreEqual(ss.GetCellValue("a1"), 2.0);
            Assert.AreEqual(ss.GetCellValue("a2"), 0.25);
            Assert.AreEqual(ss.GetCellValue("a3"), 1.0);
            Assert.AreEqual(ss.GetCellValue("a4"), 4.0);
            Assert.AreEqual(ss.GetCellValue("a5"), 4.25);
        }

        /// <summary>
        /// Tests that changing the contents of one cell changes the value of all cells dependent on it.
        /// </summary>
        [TestMethod]
        public void TestValueChangesBecauseOfDependents()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "=1 + 1");
            ss.SetContentsOfCell("a2", "=2/(a1 + 6)");
            ss.SetContentsOfCell("a3", "=a2 * a1 * 2");
            ss.SetContentsOfCell("a4", "=a3 + (36.0 / (6 + 6))");
            ss.SetContentsOfCell("a5", "=a4 + a2");

            Assert.AreEqual(ss.GetCellValue("a1"), 2.0);
            Assert.AreEqual(ss.GetCellValue("a2"), 0.25);
            Assert.AreEqual(ss.GetCellValue("a3"), 1.0);
            Assert.AreEqual(ss.GetCellValue("a4"), 4.0);
            Assert.AreEqual(ss.GetCellValue("a5"), 4.25);

            ss.SetContentsOfCell("a1", "=2+2");
            Assert.AreEqual(ss.GetCellValue("a1"), 4.0);
            Assert.AreEqual(ss.GetCellValue("a2"), 0.2);
            Assert.AreEqual(ss.GetCellValue("a3"), 1.6);
            Assert.AreEqual(ss.GetCellValue("a4"), 4.6);
            Assert.AreEqual(ss.GetCellValue("a5"), 4.8);
        }

        /// <summary>
        /// Tests that a spreadsheet can be built from an existing XML file when the file path is passed to the constructor.
        /// </summary>
        [TestMethod]
        public void TestFourParamConstructor()
        {
            WriteDocument();
            Spreadsheet ss = new Spreadsheet("save.txt", (x) => true, (x) => x, "default");

            Assert.IsNotNull(ss);
            Assert.AreEqual(ss.GetCellValue("A1"), "hello");
            Assert.AreEqual(ss.GetCellValue("A2"), 44.0);
            Assert.AreEqual(ss.GetCellValue("A3"), 8.0);

            Assert.AreEqual(ss.Version, "default");
            Assert.AreEqual(ss.GetNamesOfAllNonemptyCells().Count(), 3);
        }

        /// <summary>
        /// Tests that trying to build a spreadsheet from a nonexistent file name throws a SpreadsheetReadWriteException.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetReadWriteExceptionFileNotFound()
        {
            Assert.ThrowsException<SpreadsheetReadWriteException>(() => 
            {
                Spreadsheet ss = new Spreadsheet("svae.txt", (x) => true, (x) => x, "default");
            });
        }

        /// <summary>
        /// Tests that if the version of the file doesn't match the version we pass, a SpreadsheetReadWriteException is thrown.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetReadWriteExceptionMismatchedVersions()
        {
            Assert.ThrowsException<SpreadsheetReadWriteException>(() =>
            {
                Spreadsheet ss = new Spreadsheet("save.txt", (x) => true, (x) => x, "not default");
            });

        }

        /// <summary>
        /// Tests the save method. Now that we know that building from an existing file works, we can test the save
        /// method by saving an existing spreadsheet, then making sure a different spreadsheet can be built from this
        /// file without issue.
        /// </summary>
        [TestMethod]
        public void TestSave()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "hello");
            ss.SetContentsOfCell("a2", "44.4");
            ss.SetContentsOfCell("a3", "=90 - 60");

            ss.Save("new.txt");

            Spreadsheet ssNew = new Spreadsheet("new.txt", (x) => true, (x) => x, "default");

            Assert.AreEqual(ssNew.GetCellValue("a1"), "hello");
            Assert.AreEqual(ssNew.GetCellValue("a2"), 44.4);
            Assert.AreEqual(ssNew.GetCellValue("a3"), 30.0);

            Assert.AreEqual(ssNew.Version, "default");
            Assert.AreEqual(ssNew.GetNamesOfAllNonemptyCells().Count(), 3);
        }

        /// <summary>
        /// Tests the GetSavedVersion method on multiple saved files.
        /// </summary>
        [TestMethod]
        public void TestGetSavedVersion()
        {
            WriteDocument();

            Spreadsheet ss = new Spreadsheet();

            Assert.AreEqual(ss.GetSavedVersion("save.txt"), "default");

            Spreadsheet ss2 = new Spreadsheet((x) => true, (x) => x, "v1.0");
            ss2.Save("versionTest.txt");

            Assert.AreEqual(ss.GetSavedVersion("versionTest.txt"), "v1.0");
        }

        /// <summary>
        /// Tests that the GetSavedVersion method throws a SpreadsheetReadWriteException if the file is not found.
        /// </summary>
        [TestMethod]
        public void TestGetSavedVersionExceptionFileNotFound()
        {
            Spreadsheet ss = new Spreadsheet("save.txt", (x) => true, (x) => x, "default");

            Assert.ThrowsException<SpreadsheetReadWriteException>(() =>
            {
                ss.GetSavedVersion("svae.txt");
            });
        }

        /// <summary>
        /// Tests that a spreadsheet that has just been created returns false for Changed, whether or not it was built
        /// from an XML file.
        /// </summary>
        [TestMethod]
        public void TestChangedNewSpreadsheet()
        {
            WriteDocument();
            Spreadsheet ss = new Spreadsheet();
            Spreadsheet ssFile = new Spreadsheet("save.txt", (x) => true, (x) => x, "default");

            Assert.IsFalse(ss.Changed);
            Assert.IsFalse(ssFile.Changed);
        }

        /// <summary>
        /// Tests that a spreadsheet that has been modified returns true for Changed.
        /// </summary>
        [TestMethod]
        public void TestChangedAfterAdd()
        {
            WriteDocument();
            Spreadsheet ss = new Spreadsheet();
            Spreadsheet ssFile = new Spreadsheet("save.txt", (x) => true, (x) => x, "default");

            ss.SetContentsOfCell("a1", "ch ch ch ch changes");
            ssFile.SetContentsOfCell("A1", "yo");

            Assert.IsTrue(ss.Changed);
            Assert.IsTrue(ssFile.Changed);

        }

        /// <summary>
        /// Tests that a spreadsheet that has been modified returns false for Changed after being saved.
        /// </summary>
        [TestMethod]
        public void TestChangedAfterSave()
        {
            Spreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("a1", "2.3");
            ss.Save("this.txt");

            Assert.IsFalse(ss.Changed);
        }

        /// <summary>
        /// Tests that a spreadsheet that has had a cell return to empty returns true for Changed.
        /// </summary>
        [TestMethod]
        public void TestChangedAfterRemove()
        {
            WriteDocument();
            Spreadsheet ss = new Spreadsheet("save.txt", (x) => true, (x) => x, "default");

            ss.SetContentsOfCell("A1", "");

            Assert.IsTrue(ss.Changed);
        }

        /// <summary>
        /// Private helper method which writes a spreadsheet XML file for testing purposes.
        /// </summary>
        private void WriteDocument()
        {
            using (XmlWriter writer = XmlWriter.Create("save.txt"))
            { 
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A2");
                writer.WriteElementString("contents", "44.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A3");
                writer.WriteElementString("contents", "=4+4");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            return;
        }
    }
}