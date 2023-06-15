```
Author:     Draeden Jensen and John Haraden
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  DraedenJensen
			DevelopingJohnH
Repo:       https://github.com/uofu-cs3500-spring23/assignment-six---gui---functioning-spreadsheet-caffeinatedcoders
Date:       03-02-2023 11:37 AM 
Project:    GUI
Copyright:  CS 3500, Draeden Jensen, and John Haraden - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

We ran into a weird bug where the spreadsheet will sometimes misbehave or crash after closing the help menu
window. We couldn't find a successful fix for this, and concluded that it was a bug in MAUI.

# Assignment Specific Topics

No additional information is required.

# Consulted Peers:

We didn't consult any peers besides the two of us while completing this project.

# Partnership:

We worked well together, and both of us were able to work much more effectively after teaming up than we did
beforehand. Almost all of the code was created via pair programming. However, John did more work with getting
the basic grid setup figured out. He was also the main contributor to the functional TabbedPage help menu.
Draeden did more work with the sum and average functionality, as well as the colorization functionality.

# Branching:

We typically worked via pair programming, only splitting up for small sections. Therefore we did not make any
separate git branches; we both worked via committing and pushing our progress we made together to the shared
main branch then pulling work to be consistent across machines.

# Design decisions 

There were plenty of points throughout the creation proccess where we made specific design decisions to improve
the usability of our program. Examples:
	- In order to get a functional grid, we decided to include the left label row inside the grid object. 
	  Including the top labels in this manner didn't work, so we just included it as a separate object within
	  the same HorizontalScrollLayout
	- We were having trouble getting the top labels displaying cell name, contents, and values to display
	  properly. We tried using frames and borders, but eventually decided to just have no border element
	  wrapping the labels, and simply separate them with Line objects.
	- We considered several options for data structures for storing each of our CellEntry objects. In the end
	  we decided to contain them in both a two-dimensional array, as well as a dictionary. This double storing 
	  of data was probably not the most efficient thing to do. However, it made it much simpler to access entries
	  wherever we needed to throughout the program. Whenever it was simplest to access entries via numerical
	  coordinates, we used the array. Whenever it was simplest to access entries via their alphanumeric name,
	  we used the dictionary.
	- We originally used DisplayAlerts to represent the help menu, but we switched this to the more robust 
	  TabbedPage object (featuring pictures!!)
	- We ran into a bit of trouble when it came to displaying formula contents/value. We ended up deciding to
	  prepend '=' to the contents whenever they are displayed in order to allow the spreadsheet to remember that
	  this is in fact a formula.

# Overall functionality and additional features

For our additional feature requirement, we decided to add the ability to calculate sums/averages just like Excel.
This is done by typing =sum() or =avg() into a cell. Within the parenthesis, type the names of the cells that form
the bounds of the region you would like to calculate. This works within rows and columns, as well as any rectangular
region of cells. This functionality is case-insensitive and ignores whitespace. If any cell within the range does
not contain a formula or number as its value, the result will return an error. Examples:
	- =sum(a1,a99) will calculate the sum of all cells' values in the A column between 1 and 99.
	- =AVG(A1, B2) will calculate the average of the values of the cells A1, B1, A2, and B2.
	- =avg(a1, a1) will simply return the value of A1.
	- =avg(a1, a3) will return an error if the value of A1, A2, or A3 is empty, a string, or a FormulaError.

We also decided to add a fun option within the help tab that turns the spreadsheet into a beautiful rainbow. This is 
pointless but we like it.

For a more detailed description of how to navigate our spreadsheet program, see the help menu contained within the
program.

# Time tracking

We estimated that this assignment would take around 20 hours. Really we didn't have a solid estimate; we were just
overwhelmed and assumed that 20 hours was whole lot of time.

Well, it took us around 24 hours. We didn't really run into much trouble; this assignment was just a lot. A lot of
learning, a lot of new software, a lot of unexpected bugs, a lot of functionality to implement, a lot of things that
weren't *quite* good enough, and a lot of combing through Microsoft Learn pages. We learned a lot during this assignment.

In terms of breaking down the time spent, it's very hard to do that. There wasn't a lot of time designated for 'design'
and 'debugging' and such; every time we worked together it was a little bit of everything as we moved toward the finished 
product.

Also, almost all of our time was spent working together. There was a bit more individual work in week 1 before we solidly
knew we were going to team up. This time is harder to track, but we estimate we each spent about 4 hours trying to get things
figured out beforehand. After pairing up, we worked almost exclusively via pair programming, so our time estimates are accurate.

# Best Team Practices

One area where our partnership thrived was in our communication. Both of us felt that we worked much more
efficiently as a partnership than we did individually. Talking aloud about the code we were writing gave both
of us a more firm grasp on the code we were writing, and allowed us to get stuck much less often. We were very
open about suggesting approaches to solving problems, theorizing about why our code may be incorrect, and
brainstorming design strategies. Both of us feel that our communication and problem-solving styles are very
compatible, and that is the biggest thing that allows us to be such an effective partnership. Another big factor
is how compatible our schedules are and how willing we each are to devote a significant amount of time to getting
things done. Spending so much time physically side by side allowed us to work very effectively, and the amount
of time we both set aside for getting the assignment done really allowed our sessions to be fruitful by allowing
us to really delve into the problem.

As mentioned, we both feel very good about how well we work together. However, one way our partnership could be
improved is by increased communication when we feel stuck. To be clear, this is Draeden writing this section, and
this is particularly something I feel I could improve. When I feel stuck, my instinct is either to sit silently
pondering and researching solutions independently, or, if I'm really lost, I sometimes even resort to sitting back, 
mentally checking out, and assuming John will come across a magical solution. This does not make for an effective 
partnership. We could both be better at working together and being open about everything we think about and research, 
even when we feel very stuck. Going our separate ways when we get stuck removes the crucial benefit of pair programming
in this situation. 

I, John, also find myself checking out or delving deep into individual research when feeling stuck and find myself 
feeling too embarrassed to reach out for help. It may help if Draeden and I take a short break together to discuss the 
issue before hopping onto Google or stabbing blindly at the problem. I also feel that I could improve my researching 
skills by more fully researching fixes before going back and trying to adjust the code. I think that doing this would 
make a significant difference as I would have a collection of possible solutions to try when approaching the problem. 
Usually, I find something I think can solve the problem and try to implement it as soon as possible instead of compiling
a collection of solutions to draw from. This prevents me from approaching the problem holistically which leads me to 
writing overly complex "solutions" that could be considerably simpler if I were to have thought everything out in the beginning.

# References:

    1. https://learn.microsoft.com/en-us/dotnet/maui/get-started/first-app?view=net-maui-7.0&tabs=vswin&pivots=devices-android microsoft documentation
		Used as a starting point to better understand MAUI.
	2. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/?view=net-maui-7.0
		Used as a reference for layout options.
    3. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/grid?view=net-maui-7.0
		Used as a reference for creating the grid. Specifically used these subsections of this documentation:
			a. https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.compatibility.grid.children?view=net-maui-7.0
			b. https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.compatibility.grid.layoutchildren?view=net-maui-7.0
			c. https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.compatibility.grid.columndefinitionsproperty?view=net-maui-7.0
			d. https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.compatibility.grid.setcolumn?view=net-maui-7.0
	4. https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples 
		Used to learn C# implementation of tuples.
	5. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/entry?view=net-maui-7.0
		Used as a reference for understanding what methods our Entry objects could run.
	6. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/stacklayout?view=net-maui-7.0 
		Used to better understand the StackLayout object.
	7. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/shapes/line?view=net-maui-7.0
		Used to learn about the Line object.
	8. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pop-ups?view=net-maui-7.0
		Used as a reference when creating our various pop-up menus.
	9. https://stackoverflow.com/questions/40781396/c-sharp-save-txt-file-on-desktop
		Used when we were wondering how to save a file to desktop; this taught us about the Environment class.
	10. https://www.tutorialspoint.com/check-if-a-file-exists-in-chash 
		Used to understand how to implement a file exists checker while saving a file.
	11. https://stackoverflow.com/questions/9468950/cut-a-string-with-a-known-start-end-index
		Used to understand C# implementation of the range feature in substrings.
	12. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/tabbedpage?view=net-maui-7.0
		Used as a reference for creating the TabbedPage help menu.
	13. https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/image?view=net-maui-7.0
		Used as a reference for including screenshots in our help menu.
	14. https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
		Used as a reference for improving comments in XAML
		