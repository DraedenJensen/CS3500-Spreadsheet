using Microsoft.Maui.Controls;

namespace GUI;

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
/// Implementation of a HelpMenuTutorial class which allows for a graphical implementation of a tutorial of the
/// spreadsheet model using the MAUI software.
/// </summary>
public class HelpMenuTutorialPage : TabbedPage
{
    /// <summary>
    /// Creates a tabbed page for the spreadsheet tutorial. This allows users to understand the workings of 
    /// the Spreadsheet GUI and what information they can enter into it.
    /// </summary>
    public HelpMenuTutorialPage()
    {
        // Creates the "Welcome" page for the tutorial
        ContentPage welcomePage = new()
        {
            Title = "Welcome!",
            Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Welcome!",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new Image
                    {
                        Source = "welcome.jpg",
                        MaximumHeightRequest = 350,
                        MaximumWidthRequest = 350,
                    },
                    new Label
                    {
                        Text = "Thank you for using our spreadsheet program! \nThis is a short tutorial on some of the basic functionality.",
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    CreateCloseButton(),
                },
        };

        // Creates the "Layout and Navigation" page for the tutorial
        ContentPage layoutAndNavigationPage = new()
        {
            Title = "Layout And Navigation",
            Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Spreadsheet Layout And Navigation",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new Image
                    {
                        Source = "welcome.jpg",
                        MaximumHeightRequest = 350,
                        MaximumWidthRequest = 350,
                    },
                    new Label
                    {
                        Text = "Click on a cell to select it. \nYou may also use enter and tab to navigate down or to the right, respectively. \n" +
                            "The labels in the top left corner display the cell name, content, and value, respectively.",
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    CreateCloseButton(),
                },
        };

        // Creates the "Cell Content" page for the tutorial
        ContentPage changeCellContentPage = new()
        {
            Title = "Cell Content",
            Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Changing A Cell's Content",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new Image
                    {
                        Source = "cellcontent.jpg",
                        MaximumHeightRequest = 350,
                        MaximumWidthRequest = 350,
                    },
                    new Label
                    {
                        Text = "Each cell can hold a string, decimal numerical value, or a mathematical formula. \n" +
                            "Any entry beginning with the equals symbol (=) will be evaluated as a formula.",
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    CreateCloseButton(),
                },
        };

        // Creates the "Using Formulas" page for the tutorial
        ContentPage formulateFormulaPage = new()
        {
            Title = "Using Formulas",
            Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Formulating A Formula",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new Image
                    {
                        Source = "formulas.jpg",
                        MaximumHeightRequest = 350,
                        MaximumWidthRequest = 350,
                    },
                    new Label
                    {
                        Text = "A formula can contain operators, parenthesis, decimal numerical values, and names of other cells " +
                            "whose value will be used. \nAny Formula referencing an empty cell will result in an error.",
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    CreateCloseButton(),
                },
        };

        // Creates the "Saving" page for the tutorial
        ContentPage savingSpreadsheetPage = new()
        {
            Title = "Saving",
            Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Saving Your Spreadsheet",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new HorizontalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Children= {
                            new Image
                            {
                                Source = "save1.jpg",
                                MaximumHeightRequest = 250,
                                MaximumWidthRequest = 250,
                                Margin = 10,
                            },
                            new Image
                            {
                                Source = "save2.jpg",
                                MaximumHeightRequest = 250,
                                MaximumWidthRequest = 250,
                                Margin = 10,
                            },
                            new Image
                            {
                                Source = "save3.jpg",
                                MaximumHeightRequest = 250,
                                MaximumWidthRequest = 250,
                                Margin = 10,
                            },
                        },
                    },
                    new Label
                    {
                        Text = "The Save option in the File drop-down menu allows you to name your spreadsheet and will save it to your desktop automatically. \n" +
                            "All spreadsheet files used by this program must have the .sprd extension.",
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    CreateCloseButton(),
                },
        };

        // Creates the "Opening Existing" page for the tutorial
        ContentPage openingSpreadsheetPage = new()
        {
            Title = "Opening Existing",
            Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Open An Existing Spreadsheet",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new HorizontalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Children = {
                            new Image
                            {
                                Source = "open1.jpg",
                                MaximumHeightRequest = 250,
                                MaximumWidthRequest = 250,
                                Margin = 10,
                            },
                            new Image
                            {
                                Source = "open2.jpg",
                                MaximumHeightRequest = 250,
                                MaximumWidthRequest = 250,
                                Margin = 10,
                            },
                            new Image
                            {
                                Source = "open3.jpg",
                                MaximumHeightRequest = 250,
                                MaximumWidthRequest = 250,
                                Margin = 10,
                            },
                        },
                    },
                    new Label
                    {
                        Text = "The Open option in the File drop-down menu allows you to open an existing spreadsheet from your computer. \n" +
                            "Only files with the .sprd extension can be successfully opened.",
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    CreateCloseButton(),
                },
        };

        // Creates the "Creating New" page for the tutorial
        ContentPage newSpreadsheetPage = new()
        {
            Title = "Creating New",
            Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Open A New Spreadsheet",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new HorizontalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Children = {
                            new Image
                            {
                                Source = "new1.jpg",
                                MaximumHeightRequest = 250,
                                MaximumWidthRequest = 250,
                                Margin = 10,
                            },
                            new Image
                            {
                                Source = "new2.jpg",
                                MaximumHeightRequest = 250,
                                MaximumWidthRequest = 250,
                                Margin = 10,
                            },
                        },
                    },
                    new Label
                    {
                        Text = "The New option in the File drop-down menu allows you to create a new spreadsheet.",
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    CreateCloseButton(),
                },
        };

        // Creates the "Extra Functionality" page for the tutorial
        ContentPage extraFunctionalityPage = new()
        {
            Title = "Extra Functionality",
            Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Additional Functionality",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new Image
                    {
                        Source = "sumavg.jpg",
                        MaximumHeightRequest = 350,
                        MaximumWidthRequest = 350,
                        Margin = 10,
                    },
                    new Label
                    {
                        Text = "As an additional option, you can perform a sum or average of any region of cells by inputing the corner bounds.\n" +
                            " For example, typing =sum(a1,b4) will calculate the sum of all cells in that range.\n" +
                            "Typing =avg(a1,b4) will calculate the average of all cells in that range.",
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    CreateCloseButton(),
                },
        };
        // Adds each page to the TabbedPage
        Children.Add(welcomePage);
        Children.Add(layoutAndNavigationPage);
        Children.Add(changeCellContentPage);
        Children.Add(formulateFormulaPage);
        Children.Add(savingSpreadsheetPage);
        Children.Add(openingSpreadsheetPage);
        Children.Add(newSpreadsheetPage);
        Children.Add(extraFunctionalityPage);

        SelectedTabColor = Colors.PowderBlue;
        UnselectedTabColor = Color.FromRgb(200, 200, 250);
        BarTextColor = Colors.Black;
    }

    /// <summary>
    /// Creates and returns a button that closes the current window.
    /// </summary>
    private IButton CreateCloseButton()
    {
        Button closeButton = new()
        {
            Text = "Close",
            Command = new Command(
                execute: () =>
                {
                    Application.Current.CloseWindow(Window);
                })

        };
        return closeButton;
    }
}