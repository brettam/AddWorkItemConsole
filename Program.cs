// See https://aka.ms/new-console-template for more information


Console.WriteLine("WorkItem Creator - Console Application - Practice");
System.Console.WriteLine("Would you like to add a work item to your azure directory?");
System.Console.WriteLine("Enter (y) for Yes, (n) for No");
string? response = Console.ReadLine();
bool addWI = response == "y" || response == "Y";
if (!addWI)
{
    Environment.Exit(1);
}

System.Console.WriteLine("Work Item Type = User Story");
System.Console.WriteLine("Please Enter your User Story Title:");
string? title = Console.ReadLine();

System.Console.WriteLine("Enter the item description:");
string? description = Console.ReadLine();
if (String.IsNullOrEmpty(description))
{
    System.Console.WriteLine("A description must be entered. Please enter a description or leave blank to exit the program");
    description = Console.ReadLine();

    if (String.IsNullOrEmpty(description))
    {
        Environment.Exit(1);
    }
}

System.Console.WriteLine("If applicable, please add drawing reference for where issue occurs");
string? reference = Console.ReadLine();

if (String.IsNullOrEmpty(reference))
{
    reference = "n/a";
}

if (String.IsNullOrEmpty(title))
{
    title = description.ToUpper();
}

string finalItem = ReviewStory(title, description, reference);

SubmitItem(finalItem);

string ReviewStory(string title, string description, string reference)
{
    char c = description[0];
    if (char.IsLetter(c) && c != char.ToUpper(c))
    {
        description = char.ToUpper(c) + description.Substring(1);
    }
    System.Console.WriteLine("Review your User Story:");
    System.Console.WriteLine($"Title: {title}");
    System.Console.WriteLine($"Description: {description} - ref: {reference}");

    System.Console.WriteLine("Submit? (Y) for Yes, (N) for No or Exit, (E) for Edit");
    string? x = Console.ReadLine();

    if (!ValidateSubmission(x, out string suboredit))
    {
        Environment.Exit(1);
    }

    string checkAgain = "";
    string[] finalItem;

    if (suboredit == "edit")
    {
        finalItem = EditItem(title, description, reference);
        title = finalItem[0];
        description = finalItem[1];
        reference = finalItem[2];
        checkAgain = ReviewStory(title, description, reference);
    }

    return $"{title.ToUpper()}: {description} - ref: {reference}";
}

bool ValidateSubmission(string? x, out string suboredit)
{
    if (String.IsNullOrEmpty(x))
    {
        System.Console.WriteLine("No option entered. Submit? (y) for Yes, (n) for Exit App, (e) for Edit Title, Description, or Reference");
        System.Console.WriteLine("If no option entered, program will exit");
        x = Console.ReadLine();

        if (String.IsNullOrEmpty(x))
        {
            suboredit = "";
            return false;
        }

        if (!ValidateSubmission(x, out suboredit))
        {
            suboredit = "";
            return false;
        }
    }

    if (x == "y" || x == "Y")
    {
        suboredit = "submit";
        return true;
    }
    if (x == "n" || x == "N")
    {
        suboredit = "";
        return false;
    }
    if (x == "e" || x == "E")
    {
        suboredit = "edit";
        return true;
    }

    suboredit = "";
    return false;
}


string[] EditItem(string title, string description, string reference)
{
    System.Console.WriteLine("What would you like to edit?");
    System.Console.WriteLine("(T) for Title, (D) for Description, (R) for Reference");
    string? response = Console.ReadLine();

    if (!ValidateEdit(response, out string editType))
    {
        Environment.Exit(1);
    }

    switch (editType)
    {
        case "title":
            title = EditInput(title);
            break;

        case "description":
            description = EditInput(description);
            break;

        case "reference":
            reference = EditInput(reference);
            break;
    }

    string[] final = { title, description, reference };
    return final;
}

bool ValidateEdit(string? x, out string editType)
{
    if (String.IsNullOrEmpty(x))
    {
        System.Console.WriteLine("Please select an option for editing:");
        System.Console.WriteLine("(T) for Title, (D) for Description, (R) for Reference");
        System.Console.WriteLine("Or any other character to exit the application");
        x = Console.ReadLine();

        if (String.IsNullOrEmpty(x))
        {
            editType = "";
            return false;
        }

        if (!ValidateEdit(x, out editType))
        {
            editType = "";
            return false;
        }

        return true;
    }

    if (x == "t" || x == "T")
    {
        editType = "title";
        return true;
    }
    if (x == "d" || x == "D")
    {
        editType = "description";
        return true;
    }
    if (x == "r" || x == "R")
    {
        editType = "reference";
        return true;
    }

    editType = "";
    return false;
}

string EditInput(string input)
{
    System.Console.WriteLine($"Please enter what you'd like to replace ({input})");
    string? output = Console.ReadLine();

    if (String.IsNullOrEmpty(output))
    {
        System.Console.WriteLine($"Please enter what you'd like to replace ({input})");
        System.Console.WriteLine("Or leave line empty to exit program.");
        output = Console.ReadLine();

        if (String.IsNullOrEmpty(output))
        {
            Environment.Exit(1);
        }
    }
    return output;
}

void SubmitItem(string finalItem)
{
    System.Console.WriteLine(finalItem);
}
