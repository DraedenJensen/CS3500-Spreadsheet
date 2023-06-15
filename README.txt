```
Author:     Draeden Jensen and John Haraden
Partner:    None
Start Date: 17-Jan-2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  DraedenJEnsen
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-DraedenJensen
Commit Date:03-02-2023 12:00 PM
Solution:   Spreadsheet
Copyright:  CS 3500, Draeden Jensen, and John HAraden - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is currently capable of evaluating formulas in the form of arithmetic expressions containing
positive integers, variables, and operators. This functionality has been expanded to include a more generally applicable
Formula class which performs double operations. The program also implements a dependency graph which tracks which variables
are dependent on others, allowing relevant formulas to be computed in the necessary order. These functionalities have been
brought together into the complete Spreadsheet class, which implements a Spreadsheet object consisting of cells that can
contain test, numbers, or formulas. The spreadsheet includes the functionality of evaluating formulas, and using the 
dependency graph to update cell contents when one cell is changed. Finally, all of this functionality is graphically
represented in a functional spreadsheet GUI.

# Time Expenditures:
    
    Hours Estimated/Worked         Assignment                       Note
         12    /   14      Assignment 1 - Formula Evaluator     Spent a few hours figuring out GitHub and getting the repo 
                                                                set up.
         
         12    /   09      Assignment 2 - Dependency Graph      Everything went smoothly; very few unexpected bugs and 
                                                                minimal GitHub trouble.
         
         13    /   11      Assignment 3 - Formula               Took me longer than expected initially; I was struggling to
                                                                figure out what exactly the assignment specifications were 
                                                                saying.

         10    /   08      Assignment 4 - Spreadsheet           Everything went smoothly. At first the project seemed
                                                                overwhelming, but it didn't take long to recognize what the
                                                                professor had said about a lot of the solution already being
                                                                implemented.

        14     /   13      Assignment 5 - Spreadsheet           This one took a bit longer. I didn't run into any major issues,
                                                                it was just that there was a lot to understand, a lot of small
                                                                changed to implement, and a lot of new XML stuff to learn about.
                                                                It was a big assignment, but I feel good about how it went
                                                                overall.
      
        20     /   24      Assignment 6 - GUI                   It was very intensive, but... we got it done.

# My Software Practices
 
- DRY: I feel that I have successfully implemented code that has minimal repetition. One specific way I've found myself doing this
    is by setting up private helper methods whenever possible. If I notice that any chunk of code is being repeated in multiple
    methods, then my response is always to extract that method into a separate helper method that can then be called multiple times.
    Examples of this are the single CellContentHelper method which executes all the code for changing cell contentst that is common
    between the three types of cell contents, and the small Validate method which ensures that variables match the correct syntax. 
    I understand the importance of avoiding repetition in code, because repeating code only makes it less readable and more likely
    to contain bugs somewhere.

- Regression testing: Assignment 5 was my first real opportunity to implement much regression testing, but I feel that I did it well.
    I carefully went back through my Assignment 4 tests and refactored them to work for Assignment 5. Doing this was very helpful --
    rather than beginning a new test suite from scratch, it really improved my confidence in my program to feel like I was starting
    testing Assignment 5 with a headstart. All the tests I'd already written for Assignment 4 were still there and still testing my 
    code effectively, just changed slightly for the new specifications. This provided a great basis to build the remaining Assignment 5
    tests off of.

- Good naming and commenting: Honestly this was something I did not do well in my early coding career. Commenting seemed like an
    unnecessary hassle, so I refrained from doing any more that was necessary; and I never prioritized clear names for my variables
    and methods. However, working on the full Spreadsheet assignment has shown me the importance of well-documented code. This project is
    more complex than anything I've ever worked on, with more interlocking parts and systems than I'm used to. With so much going on, good
    documentation is vital for understanding what all this code is doing. I've started to really prioritize it. Every time I write a 
    method, my first instinct is now to write a short summary giving a high-level description of what it does. As I go back to my old code
    from earlier assignments, I'm finding these old comments invaluable. Similarly, I'm giving longer and more descriptive names to my
    methods, and that's really helping me put together a full understanding of how all the little parts combine to make a functional
    spreadsheet.
