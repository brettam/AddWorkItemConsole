using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

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

System.Console.WriteLine("Please assign a priority value (1, 2, 3 or 4)");
System.Console.WriteLine("Any entry outside of '1' '2' '3' or '4' will result in a Priority level of '3'");
string? priority = Console.ReadLine();

if (String.IsNullOrEmpty(priority))
{
    priority = "3";
}

if (String.IsNullOrEmpty(reference))
{
    reference = "n/a";
}

if (String.IsNullOrEmpty(title))
{
    title = description;
}

await ReviewStory(title, description, reference, priority);


//// Review the Input and Get it Ready For Submission
///
async Task ReviewStory(string title, string description, string reference, string priority)
{
    char c = description[0];
    if (char.IsLetter(c) && c != char.ToUpper(c))
    {
        description = char.ToUpper(c) + description.Substring(1);
    }
    System.Console.WriteLine("Review your User Story:");
    System.Console.WriteLine($"TITLE: {title}");
    System.Console.WriteLine($"DESCRIPTION: {description} - ref: {reference}");
    System.Console.WriteLine($"PRIORITY: {priority}");

    System.Console.WriteLine("Submit? (Y) for Yes, (N) for No or Exit, (E) for Edit");
    string? x = Console.ReadLine();

    if (!ValidateSubmission(x, out string suboredit))
    {
        Environment.Exit(1);
    }
    string[] finalItem;

    if (suboredit == "edit")
    {
        finalItem = EditItem(title, description, reference, priority);
        title = finalItem[0];
        description = finalItem[1];
        reference = finalItem[2];
        priority = finalItem[3];
        await ReviewStory(title, description, reference, priority);
    }

    description = $"{description} - Ref: {reference}.";

    WorkItem workItem = new WorkItem
    {
        Title = title,
        Description = description,
        AssignedTo = "toddmorrell@hotmail.com",
        Priority = priority
    };

    await SubmitWorkItem(workItem);
}

////// Validating the Submission
bool ValidateSubmission(string? x, out string suboredit)
{
    if (String.IsNullOrEmpty(x))
    {
        System.Console.WriteLine("No option entered. Submit? (y) for Yes, (n) for Exit App, (e) for Edit Title, Description, Reference, or Priority");
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

    if (x.ToUpper() == "Y")
    {
        suboredit = "submit";
        return true;
    }
    if (x.ToUpper() == "N")
    {
        suboredit = "";
        return false;
    }
    if (x.ToUpper() == "E")
    {
        suboredit = "edit";
        return true;
    }

    suboredit = "";
    return false;
}

////// Editing a Submission Item "title, description, reference"
string[] EditItem(string title, string description, string reference, string priority)
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

        case "priority":
            priority = EditInput(priority);
            break;
    }

    string[] final = { title, description, reference, priority };
    return final;
}

////// Validate the input that was edited
bool ValidateEdit(string? x, out string editType)
{
    if (String.IsNullOrEmpty(x))
    {
        System.Console.WriteLine("Please select an option for editing:");
        System.Console.WriteLine("(T) for Title, (D) for Description, (R) for Reference, (P) for Priority");
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

    if (x.ToUpper() == "T")
    {
        editType = "title";
        return true;
    }
    if (x.ToUpper() == "D")
    {
        editType = "description";
        return true;
    }
    if (x.ToUpper() == "R")
    {
        editType = "reference";
        return true;
    }
    if (x.ToUpper() == "P")
    {
        editType = "priority";
        return true;
    }

    editType = "";
    return false;
}

/////// Validate and Replcae the string that was edited
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


///////// Submit the Work Item
async Task SubmitWorkItem(WorkItem workItem)
{
    string uri = "https://localhost:7088/create_work_item";

    using (HttpClient client = new HttpClient())
    {
        try
        {
            var response = await client.PostAsync(
            uri, new StringContent(JsonConvert.SerializeObject(workItem), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("Work Item has been created!");
            }
            else
            {
                System.Console.WriteLine("Submission has failed.");
            }
        }
        catch (System.Exception Ex)
        {
            System.Console.WriteLine(Ex);
        }



    }
    Console.ReadLine();
}


public class WorkItem
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? AssignedTo { get; set; }
    public string? Priority { get; set; }
}
