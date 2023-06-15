/// <summary>
/// Author:    Draeden Jensen and John Haraden
/// Date:      02-17-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500, Draeden Jensen, and John Haraden - This work may not 
///            be copied for use in Academic Coursework.
///
/// We, Draeden Jensen and John Haraden, certify that this code was written from scratch and
/// we did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in the README file.
///
/// File Contents:
/// Implementation of a MainPage class which allows for a graphical implementation of a spreadsheet model
/// using the MAUI software.
/// </summary>

using Microsoft.Maui.Controls;
using SpreadsheetUtilities;
using SS;
using System.Diagnostics;
using System.Text;

namespace GUI
{
    /// <summary>
    /// Simple nested class which inherits from Entry, but adds new fields and and methods that implements the Name, Content, and Value
    /// fields that each spreadsheet cell has.
    /// </summary>
    internal class CellEntry : Entry
    {
        /// <summary>
        /// Corresponds to this entry's coordinates in the spreadsheet grid.
        /// </summary>
        public (int, int) Position { get; set; }

        /// <summary>
        /// Name, Content, and Value properties correspond to these fields for this entry's cell in the spreadsheet.
        /// </summary>
        public string Name { get; set; }
        public string Content { get; set; }
        public string Value { get; private set; }

        /// <summary>
        /// Constructor calls the base constructor then sets all properties to empty strings.
        /// </summary>
        public CellEntry() : base()
        {
            Name = "";
            Content = "";
            Value = "";
        }

        /// <summary>
        /// Added method sets the text of the entry to this entry's content property. To be called when a cell becomes focused.
        /// </summary>
        public void DisplayContent()
        {
            this.Text = Content;
        }

        /// <summary>
        /// Added method sets the text of the entry to this entry's value property. To be called when a cell becomes unfocused.
        /// </summary>
        public void DisplayValue()
        {
            this.Text = Value;
        }

        /// <summary>
        /// Added method updates the content and value of the cell. If the content is a formula, the content and value are calculated accordingly.
        /// </summary>
        /// <param name="content"> New content of the cell </param>
        /// <param name="value">New value of the cell </param>
        public void UpdateContentAndValue(object content, object value)
        {
            if (content is Formula)
            {
                Content = "=" + content.ToString();
            }
            else
            {
                Content = content.ToString();
            }

            if (value is FormulaError)
            {
                Value = "Error";
            }
            else
            {
                Value = value.ToString();
            }

            DisplayValue();
        }
    }

    /// <summary>
    /// MainPage class which inherits from ContentPage. This implementation of our MainPage contains all fields and methods 
    /// needed to properly display a graphical representation of our spreadsheet.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private readonly char[] letterLabels;
        private readonly int[] numberLabels;

        private Dictionary<string, CellEntry> entriesDictionary;
        private CellEntry[,] entriesArray;

        private Spreadsheet sheet;
        private (int, int) focused;

        private readonly int length = 26;
        private readonly int height = 50;

        private bool colorized;

        /// <summary>
        /// Constructor which initializes everything and sets up the grid. Creates top labels for displaying cell info.
        /// Also creates header labels and the spreadsheet grid, all within a scroll view.
        /// </summary>
        public MainPage()
        {
            // Creates arrays for header labels for the grid.
            InitializeComponent();
            entriesArray = new CellEntry[length, height];
            entriesDictionary = new();
            colorized = false;

            letterLabels = new char[length + 1];
            string alphabet = " ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i <= length; i++)
            {
                letterLabels[i] = alphabet[i];
            }

            numberLabels = new int[height];
            for (int i = 1; i <= height; i++)
            {
                numberLabels[i - 1] = i;
            }

            // Creates top letter labels using the array.
            foreach (char label in letterLabels)
            {
                TopLabels.Add(
                    new Border
                    {
                        Stroke = Color.FromRgb(0, 0, 0),
                        StrokeThickness = 1,
                        HeightRequest = 20,
                        WidthRequest = 70,
                        HorizontalOptions = LayoutOptions.Center,
                        Content =
                            new Label
                            {
                                Text = $"{label}",
                                BackgroundColor = Color.FromRgb(200, 200, 250),
                                HorizontalTextAlignment = TextAlignment.Center
                            }
                    }
                );
            }

            //Creates left number labels using the array.
            foreach (int label in numberLabels)
            {
                LeftLabels.Add(
                    new Border
                    {
                        Stroke = Color.FromRgb(10, 10, 10),
                        StrokeThickness = 1,
                        HeightRequest = 20,
                        WidthRequest = 70,
                        HorizontalOptions = LayoutOptions.Center,
                        Content =
                            new Label
                            {
                                Text = $"{label}",
                                BackgroundColor = Color.FromRgb(200, 200, 250),
                                HorizontalTextAlignment = TextAlignment.Center
                            }
                    }
                );
            }

            // Creates the central grid object using row and column definition helpler methods.
            Grid TableGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(70, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = GridLength.Auto },
                }
            };
            TableGrid.SetColumn(LeftLabels, 0);
            TableGrid.SetRowSpan(LeftLabels, height);
            AddRowDefinitions();

            // Creates the backing spreadsheet object (blank) and focuses the sheet on the first cell.
            sheet = new Spreadsheet((s) => true, (s) => s.ToUpper(), "six");
            focused = (0, 0);
        }

        /// <summary>
        /// Private helper method used during construction of the grid. Adds new row definitions
        /// for each label in LeftLabels
        /// </summary>
        private void AddRowDefinitions()
        {
            for (int i = 0; i < height; i++)
            {
                TableGrid.AddRowDefinition(new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) });
                TableGrid.Add(CreateEmptyCellRow(i), 1, i);
            }
        }

        /// <summary>
        /// Private helper method used during construction of the grid. Creates a horizontal 
        /// stack of empty entries (the "row").
        /// </summary>
        private IView CreateEmptyCellRow(int rowNumber)
        {
            HorizontalStackLayout row = new();

            for (int i = 0; i < length; i++)
            {
                CellEntry entry = new CellEntry
                {
                    BackgroundColor = Color.FromRgb(200, 200, 200),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    FontSize = 10
                };

                entry.Focused += handleClickOrTab;
                entry.Completed += handleEnter;

                row.Add(
                    new Border
                    {
                        Stroke = Color.FromRgb(10, 10, 10),
                        StrokeThickness = 1,
                        HeightRequest = 20,
                        WidthRequest = 70,
                        HorizontalOptions = LayoutOptions.Center,
                        Content = entry,
                    }
                );

                StringBuilder sb = new();
                sb.Append(letterLabels[i + 1]);
                sb.Append(numberLabels[rowNumber]);

                entry.Name = sb.ToString();
                entry.Position = (i, rowNumber);

                entriesArray[i, rowNumber] = entry;
                entriesDictionary.Add(entry.Name, entry);
            }
            return row;
        }

        /// <summary>
        /// Private event handler method which listens for the Completed method from an Entry class,
        /// activated every time the enter key is pressed. Moves the focus down the column, and updates
        /// cell values based on the updated cell's contents.
        /// </summary>
        private void handleEnter(object sender, EventArgs e)
        {
            CellEntry oldEntry = (CellEntry)sender;

            CellEntry newEntry = new();
            try
            {
                newEntry = entriesArray[focused.Item1, ++focused.Item2];
            }
            catch (IndexOutOfRangeException)
            {
                focused.Item2 = 0;
                newEntry = entriesArray[focused.Item1, focused.Item2];
            }

            oldEntry.Unfocus();
            newEntry.Focus();
            HandleCellChange(oldEntry, newEntry);
        }

        /// <summary>
        /// Private event handler method which listens for the Focused method from an Entry class,
        /// activated every time the tab key is pressed or a new entry is clicked. Moves the entry
        /// to the clicked method (or simply to the next cell to the right if tab was pressed), and
        /// updates cell values based on the updated cell's contents.
        /// </summary>
        private void handleClickOrTab(object sender, EventArgs e)
        {
            CellEntry oldEntry = entriesArray[focused.Item1, focused.Item2];
            CellEntry newEntry = (CellEntry)sender;
            focused = newEntry.Position;

            HandleCellChange(oldEntry, newEntry);
        }

        /// <summary>
        /// Private helper method called by both cell changing event handlers. This method is where we
        /// go through an updated cell's dependents and update their values according to the cell's new
        /// contents. 
        /// 
        /// If attempting to change a cell's contents would result in a FormulaFormatException
        /// or a CircularException, we catch this and display an alert notifying the user their operation
        /// was invalid.
        /// </summary>
        /// <param name="oldEntry"> Entry that was just updated, which we are now moving the focus away from. </param>
        /// <param name="newEntry"> New entry that we are now moving the focus towards. </param>
        private void HandleCellChange(CellEntry oldEntry, CellEntry newEntry)
        {
            string cellName = oldEntry.Name;
            string content = oldEntry.Text;

            if (!(content is null))
            {
                try
                {
                    List<string> cellsToUpdate = sheet.SetContentsOfCell(cellName, content).ToList();
                    foreach (string cell in cellsToUpdate)
                    {
                        CellEntry entry = entriesDictionary[cell];
                        entry.UpdateContentAndValue(sheet.GetCellContents(cell), sheet.GetCellValue(cell));
                    }
                }
                catch (FormulaFormatException ex)
                {
                    DisplayAlert("Formula syntax error", ex.Message, "Continue");
                }
                catch (CircularException)
                {
                    DisplayAlert("Circular error", "A cell's value cannot be dependent upon itself", "Continue");
                }
            }

            oldEntry.DisplayValue();
            newEntry.DisplayContent();

            selectedName.Text = newEntry.Name;
            selectedContent.Text = newEntry.Content;
            selectedValue.Text = newEntry.Value;
        }

        /// <summary>
        /// Private event handler method which listens for the 'Save' button to be clicked in the File
        /// drop-down menu. Asks the user for a name, and saves to their desktop. The .sprd extension is
        /// added if the user doesn't provide it. Alerts the user if they are about to overwrite the file,
        /// then saves the backing spreadsheet object to the given file location.
        /// </summary>
        private async void FileMenuSaveAsync(object sender, EventArgs e)
        {
            string? fileName = await DisplayPromptAsync("Save spreadsheet", "Please select path for saving file", placeholder: "ex: spreadsheet.sprd");
            if (fileName is null) { return; }
            fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName;

            if (File.Exists(fileName))
            {
                bool proceed = await DisplayAlert("File Overwrite Warning", $"The file {fileName} already exists. Do you want to overwrite the current file?", "Continue", "Cancel");
                if (!proceed) { return; }
            }

            if (!fileName.Contains('.') || fileName.Substring(fileName.LastIndexOf(".")) != ".sprd")
            {
                fileName += ".sprd";
            }

            if (sheet.Changed)
            {
                sheet.Save(fileName);
            }
        }

        /// <summary>
        /// Private event handler method which listens for the 'Open' button to be clicked in the File
        /// drop-down menu. Alerts the user if their current sheet has not been saved, then replaces
        /// the backing spreadsheet object with a new spreadsheet built from an existing .sprd file.
        /// Every non-empty entry in the old sheet is cleared and every entry that's not empty in the 
        /// new sheet is updated accordingly.
        /// </summary>
        private async void FileMenuOpenAsync(object sender, EventArgs e)
        {
            FileResult? file = await FilePicker.Default.PickAsync();

            if (file is null)
            {
                return;
            }

            string path = file.FullPath.Substring(file.FullPath.LastIndexOf('.'));
            if (!path.Equals(".sprd"))
            {
                await DisplayAlert("File reading error", "Incorrect file type", "Continue");
            }

            if (sheet.Changed)
            {
                bool proceed = await DisplayAlert("Save Warning", "The current sheet has not been saved. Do you want to continue without saving?", "Continue", "Cancel");
                if (!proceed)
                {
                    return;
                }
            }

            try
            {
                ClearCurrentEntries();

                sheet = new Spreadsheet(file.FullPath, (s) => true, (s) => s.ToUpper(), "six");
                HashSet<string> nonempty = (HashSet<string>)sheet.GetNamesOfAllNonemptyCells();

                foreach (string name in nonempty)
                {
                    CellEntry entry = entriesDictionary[name];
                    entry.UpdateContentAndValue(sheet.GetCellContents(name), sheet.GetCellValue(name));
                    entry.Text = entry.Value;
                }
            }
            catch (SpreadsheetReadWriteException ex)
            {
                await DisplayAlert("File reading error", ex.Message, "Continue");
            }
        }

        /// <summary>
        /// Private event handler method which listens for the 'New' button to be clicked in the File
        /// drop-down menu. Alerts the user if their current sheet has not been saved, then replaces
        /// the backing spreadsheet object with a new blank spreadsheet and clears every non-empty entry.
        /// </summary>
        private async void FileMenuNewAsync(object sender, EventArgs e)
        {
            if (sheet.Changed)
            {
                bool proceed = await DisplayAlert("Save Warning", "The current sheet has not been saved. Do you want to continue without saving?", "Continue", "Cancel");
                if (!proceed)
                {
                    return;
                }
            }
            ClearCurrentEntries();
            sheet = new Spreadsheet(s => true, s => s.ToUpper(), "six");
        }

        /// <summary>
        /// Private helper method called by both the 'New' and 'Open' event handlers. Sets every
        /// nonempty entry in the sheet to be empty.
        /// </summary>
        private void ClearCurrentEntries()
        {
            HashSet<string> nonempty = (HashSet<string>)sheet.GetNamesOfAllNonemptyCells();

            foreach (string name in nonempty)
            {
                CellEntry entry = entriesDictionary[name];
                entry.UpdateContentAndValue("", "");
                entry.Text = entry.Value;
            }
        }

        /// <summary>
        /// Private event handler method which listens for the 'Something Fun' button to be clicked in 
        /// the Help drop-down menu. Makes the spreadsheet pretty.
        /// </summary>
        private void Colorize(object sender, EventArgs e)
        {
            if (colorized)
            {
                foreach (CellEntry entry in entriesArray)
                {
                    entry.BackgroundColor = Color.FromRgb(200, 200, 200);
                }
                colorized = false;
            }
            else
            {
                (int, int, int)[] colors = { (250, 5, 5), (250, 128, 5), (250, 250, 5), (5, 250, 5), (5, 130, 250), (107, 5, 250), (230, 5, 250), (250, 5, 148) };

                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        int color = (i + j) % 8;
                        entriesArray[i, j].BackgroundColor = Color.FromRgb(colors[color].Item1, colors[color].Item2, colors[color].Item3);
                    }
                }
                colorized = true;
            }
        }

        /// <summary>
        /// Private event handler method which listens for the 'Tutorial' button to be clicked. 
        /// Displays a tutorial explaining the basic functionality and features of the Spreadsheet.
        /// Uses a tabbed window to allow user to walk through the various pages of the tutorial.
        /// </summary>
        private void HelpMenuTutorial(object sender, EventArgs e)
        {
            Window tutorialWindow = new(new HelpMenuTutorialPage());
            Application.Current.OpenWindow(tutorialWindow);
        }
    }
}