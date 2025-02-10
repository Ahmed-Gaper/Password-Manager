public static class ConsoleHelper
{
       public static void PrintCenteredBanner(string text)
       {
            int horizontalPadding = 4; 
            int verticalPadding = 1;   

            int boxWidth = text.Length + horizontalPadding * 2;
            int boxHeight = verticalPadding * 2 + 1; 

            ConsoleColor originalForeground = Console.ForegroundColor;
            ConsoleColor originalBackground = Console.BackgroundColor;

            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < boxHeight; i++)
            {
                if (i == verticalPadding)
                {
                    Console.Write(new string(' ', horizontalPadding));
                    Console.Write(text);
                    Console.Write(new string(' ', horizontalPadding));
                }
                else
                {
                    Console.Write(new string(' ', boxWidth));
                }
                Console.WriteLine();
            }

    // Reset the console colors.
    Console.ForegroundColor = originalForeground;
    Console.BackgroundColor = originalBackground;
       }

        public static void PrintSidebarTitle(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            string border = "┌" + new string('─', title.Length + 2) + "┐";
            string textLine = $"│ {title} │";
            string bottomBorder = "└" + new string('─', title.Length + 2) + "┘";

            Console.WriteLine(border);
            Console.WriteLine(textLine);
            Console.WriteLine(bottomBorder);

            Console.ResetColor();
            Console.WriteLine();
        }
        public static int TakeValidChoice(string option1,string option2)
        {
            bool wrongChoice=false; 

                   while(true)
                   {
                       
                        if(!wrongChoice)
                        {
 
                        Console.WriteLine("\nWhat would you like to do?");
                        Console.WriteLine($"[1] {option1}");
                        Console.WriteLine($"[2] {option2}");
                
                        }
                        string choice = Console.ReadLine();
                        Console.WriteLine();
                
                        switch (choice)
                        {
                            case "1":
                    Console.Clear();
                                return 1;
                                break;
                            case "2":
                    Console.Clear();
                                return 2;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                    wrongChoice=true;
                                break;

                        }
                   }

        }

    public static void PrintErrorMessage(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {error}");
        Console.ResetColor();
        Console.WriteLine("Press any key to continue....");
        Console.ReadLine();

    }

}