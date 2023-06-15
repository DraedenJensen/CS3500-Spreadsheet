/// <summary>
/// Author:    Draeden Jensen
/// Partner:   None
/// Date:      02-17-2023
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
/// Implementation of a spreadsheet class which contains an infinite number of named cells, whose contents
/// can be updated and retrieved.
/// </summary>


using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace SS
{
    /// <summary>
    /// Simple nested internal class which represents cell objects. Every cell has a content fields and a
    /// value field, which are not necessarily the same. The only methods that exist are getters and setters
    /// for these variables.
    /// </summary>
    internal class Cell
    {
        private Object content;
        private Object value;
        public Cell(Object content)
        {
            this.content = content;
            value = "";
        }
        public Object getContent() { return content; }
        public void setContent(Object content) { this.content = content; }
        public Object getValue() { return value; }
        public void setValue(Object value) { this.value = value; }
    }

    /// <inheritdoc/>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> cells;
        private DependencyGraph dg;

        private string version;

        /// <inheritdoc/>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new();
            dg = new();

            Changed = false;

            this.version = version;
        }
        
        /// <summary>
        /// Default constructor which creates a new Spreadsheet object. Uses default values of isValid, 
        /// normalize, and version.
        /// </summary>
        public Spreadsheet() : this((x) => true, (x) => x, "default") { }

        /// <summary>
        /// Constructs a spreadsheet by recording its a path to a file to build a 
        /// spreadsheet from, its variable validity test,
        /// its normalization method, and its version information.  
        /// </summary>
        /// 
        /// <remarks>
        ///   The variable validity test is used throughout to determine whether a string that consists of 
        ///   one or more letters followed by one or more digits is a valid cell name.  The variable
        ///   equality test should be used throughout to determine whether two variables are equal.
        /// </remarks>
        /// 
        /// <param name="path">      defines a path to a file to build a spreadsheet from</param>
        /// <param name="isValid">   defines what valid variables look like for the application</param>
        /// <param name="normalize"> defines a normalization procedure to be applied to all valid variable strings</param>
        /// <param name="version">   defines the version of the spreadsheet (should it be saved)</param>
        public Spreadsheet(string path, Func<string, bool> isValid, Func<string, string> normalize, string version) : this(isValid, normalize, version)
        {
            try {
                using (XmlReader reader = XmlReader.Create(path))
                {
                    string cellName = "";

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    if (!version.Equals(reader["version"]))
                                    {
                                        throw new SpreadsheetReadWriteException("Incorrect version");
                                    }
                                    break;

                                case "cell":
                                    break;

                                case "name":
                                    reader.Read();
                                    cellName = reader.Value;
                                    break;

                                case "contents":
                                    reader.Read();
                                    SetContentsOfCell(cellName, reader.Value);
                                    break;
                            }
                        }
                    }
                }

                Changed = false;
            }
            catch (FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File not found");
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Missing file");
            }
            catch (XmlException)
            {
                throw new SpreadsheetReadWriteException("Incorrect file type");
            }
        }

        /// <inheritdoc/>
        public override object GetCellContents(string name)
        {
            name = Validate(name);

            if (cells.ContainsKey(name))
            {
                return cells[name].getContent();
            }
            else
            {
                return "";
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> names = new();

            foreach (string name in cells.Keys)
            {
                names.Add(name);
            }

            return names;
        }

        /// <inheritdoc/>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Validate(name);

            if (content.Length > 0)
            {
                if (Double.TryParse(content, out double value))
                {
                    return SetCellContents(name, value);
                }
                else if (content[0] == '=')
                {
                    content = content.Substring(1).ToLower();

                    content = Regex.Replace(content, @"\s*", "");

                    if (Regex.IsMatch(content, @"^sum\([a-zA-Z][0-9]+,[a-zA-Z][0-9]+\)$"))
                    {
                        return SumOperations(name, content, 0);
                    }
                    if (Regex.IsMatch(content, @"^avg\([a-zA-Z][0-9]+,[a-zA-Z][0-9]+\)$"))
                    {
                        return SumOperations(name, content, 1);
                    }

                    // This will throw a FormulaFormatException if formula is invalid
                    Formula f = new(content, Normalize, IsValid);
                    return SetCellContents(name, f);
                }
            }
            
            return SetCellContents(name, content);
        }

        /// <summary>
        /// Helper method that allows the calculation of sum and averages, given the bounds of a range of cells.
        /// </summary>
        protected IList<string> SumOperations (string name, string content, int type)
        {
            int index = content.IndexOf(',');

            StringBuilder formula = new StringBuilder();
            formula.Append('(');

            string name1 = content[4..index];
            string name2 = content[(index + 1)..(content.Length - 1)];

            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            int letter1 = alphabet.IndexOf(name1[0]);
            int letter2 = alphabet.IndexOf(name2[0]);

            if (letter2 < letter1)
            {
                int temp = letter2;
                letter2 = letter1;
                letter1 = temp;
            }

            int num1 = int.Parse(name1.Substring(1));
            int num2 = int.Parse(name2.Substring(1));

            if (num2 < num1)
            {
                int temp = num2;
                num2 = num1;
                num1 = temp;
            }
            
            for (int i = letter1; i <= letter2; i++)
            {
                for (int j = num1; j <= num2; j++)
                {
                    formula.Append(alphabet[i]);
                    formula.Append(j);

                    if (j < num2)
                    {
                        formula.Append('+');
                    }
                }

                if (i < letter2)
                {
                    formula.Append('+');
                }
            }

            formula.Append(')');

            if (type == 1)
            {
                int numOfElements = (num2 - num1 + 1) * (letter2 - letter1 + 1);
                formula.Append('/');
                formula.Append(numOfElements);
            }

            Formula result = new Formula(formula.ToString(), Normalize, IsValid);
            return SetCellContents(name, result);
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, double number)
        {
            return CellContentHelper(name, number, new());
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, string text)
        {
            return CellContentHelper(name, text, new());
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            return CellContentHelper(name, formula, formula.GetVariables().ToHashSet());
        }

        /// <summary>
        /// Private helper method for executing common code between the three SetCellContents methods.
        /// </summary>
        /// <param name="name"> The cell name</param>
        /// <param name="contents"> The contents to be added to the cell</param>
        /// <param name="newDees"> The list of dependees to replace, empty unless contents is a Formula</param>
        /// <returns></returns>
        private List<string> CellContentHelper(string name, Object contents, HashSet<string> newDees)
        {
            dg.ReplaceDependees(name, newDees);
            List<string> dependents = GetCellsToRecalculate(name).ToList();

            if (cells.ContainsKey(name))
            {
                if (contents.Equals(""))
                {
                    cells.Remove(name);
                    Changed = true;
                }
                else
                {
                    cells[name].setContent(contents);
                    Changed = true;
                }
            }
            else
            {
                if (!contents.Equals(""))
                {
                    cells.Add(name, new(contents));
                    Changed = true;
                }
            }
            
            foreach (string cellName in dependents) {
                try
                {
                    Recalculate(cellName, cells[cellName]);
                }
                catch(KeyNotFoundException) { }
            }
            return dependents;
        }
         
        /// <summary>
        /// Private helper method which recalculates the value of a given cell
        /// </summary>
        /// <param name="name"> name of cell to recalculate</param>
        private void Recalculate(string name, Cell cell)
        {
            if (cell.getContent() is Formula)
            {
                Formula f = (Formula) cell.getContent();
                cells[name].setValue(f.Evaluate((x) => (double) GetCellValue(x)));
            } else
            {
                cells[name].setValue(cell.getContent());
            }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dg.GetDependents(name);
        }

        /// <inheritdoc/>
        public override bool Changed { get; protected set; }

        /// <inheritdoc/>
        public override string GetSavedVersion(String filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.Name.Equals("spreadsheet"))
                        {
                            return reader["version"];
                        }
                    }
                }
                throw new SpreadsheetReadWriteException("No version information found");
            }
            catch (FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File not found");
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Missing file");
            }
            catch (XmlException)
            {
                throw new SpreadsheetReadWriteException("Missing file");
            }
        }

        /// <inheritdoc/>
        public override void Save(String filename)
        {
            try
            {
                XmlWriterSettings settings = new();
                settings.Indent = true;
                settings.IndentChars = "  ";

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");

                    writer.WriteAttributeString("version", version);

                    foreach (string s in cells.Keys)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", s);

                        Object contents = cells[s].getContent();
                        if (contents is Formula)
                        {
                            writer.WriteElementString("contents", "=" + contents.ToString());
                        }
                        else
                        {
                            writer.WriteElementString("contents", contents.ToString());
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                Changed = false;
            }
            catch (FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File not found");
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Missing file");
            }
            catch (XmlException)
            {
                throw new SpreadsheetReadWriteException("Missing file");
            }
        }

        /// <inheritdoc/>
        public override object GetCellValue(String name)
        {
            name = Validate(name);

            try
            {
                return cells[name].getValue();
            }
            catch (KeyNotFoundException)
            {
                return "";
            }
        }

        /// <summary>
        /// Private helper method used throughout the class to normalize and validate variables names. Every time a cell
        /// name is used, a method calls this method which uses the normalize function to standardize the variable name,
        /// then ensures that that name is valid. If it matches the regex syntax for a variable and is validated by the 
        /// isValid function, then the normalized name is returned.
        /// </summary>
        /// <param name="name"> Name of the cell this method is validating</param>
        /// <exception cref="InvalidNameException"></exception>
        private string Validate(string name)
        {
            name = Normalize(name);

            if (!Regex.IsMatch(name, "^[a-zA-Z]+[0-9]+$") || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            return name;
        }
    }
}