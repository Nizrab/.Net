using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Lab1
{
  
    class Program
    {
       //Main Method 
        static void Main(string[] args)
        {
            bool dummy = true;
            IList<String> bigWords = new List<String>();
            while (dummy==true) {
                menu();
                Console.Write("Selection: ");
                //User selection input 
                string selection = Console.ReadLine();
                Console.Clear();
                if (selection.ToString() == "x" || selection.ToString() == "X")
                {
                    Console.WriteLine("Thanks for using my program :)");
                    Environment.Exit(0);
                }
                else { 
                    //Swich statment which redirects user to sepecifyed method 
                    switch (selection)
                    {
                        case "1":
                            bigWords = import();
                            break;
                        case "2":
                            bubbbleSort(bigWords);
                            break;
                        case "3":
                            linqSort(bigWords);
                            break;
                        case "4":
                            diffrent(bigWords);
                            break;
                        case "5":
                            tenWords(bigWords);
                            break;
                        case "6":
                            distinctJ(bigWords);
                            break;
                        case "7":
                           distinctD(bigWords);
                           break;
                        case "8":
                            longFour(bigWords);
                            break;
                        case "9":
                            threeA(bigWords);
                            break;
                        default:
                            Console.WriteLine("Incorrect Selection! \n");
                            dummy = true;
                            break;
                    }
                }
            }
        }
        //Menu method 
        static void menu()
        {
            Console.WriteLine("1 - Import Words from File \n"+
                              "2 - Bubble Sort words \n"+
                              "3 - LINQ / Lambda sort words \n"+
                              "4 - Count the Distinct Words \n"+
                              "5 - Take the first 10 words \n"+
                              "6 - Get the number of words that start with 'j' and display the count \n"+
                              "7 - Get and display of words that end with 'd' and display the count \n" +
                              "8 - Get and display of words that are greater than 4 characters long, and display the count \n" +
                              "9 - Get and display of words that are less than 3 characters long and start with the letter 'a', and display the count \n" +
                              "x – Exit");
        }
        //Import method is uesed to import words from a txt file and store them all in a array 
        static IList<string> import()
        {
            //Array is created to store all of the words 
            IList<string> fileContent = new List<string>();
            fileContent = System.IO.File.ReadAllLines(@"D:\Year3Sem1\.NET\Lab 1 Demo\Words.txt");
            //Counts how meany elemets/words are in the array
            var totalWords = fileContent.Count(); 
            Console.WriteLine("Word Count: " + totalWords);
            return fileContent;
        }
        //Bubble sort method 
        static void bubbbleSort(IList<string> arr)
        {
            //Starts the stopwatch
            Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            IList<String> tempArr = new List<String>(arr);
            int length = tempArr.Count;
            int i, t;
            string temp;
            for (i = 0; i < length; i++)
            {
                for (t = 0; t < length - 1; t++)
                {
                    if (tempArr[t].CompareTo(tempArr[t + 1]) > 0)
                    {
                        temp = tempArr[t];
                        tempArr[t] = tempArr[t + 1];
                        tempArr[t + 1] = temp;
                    }
                }
            }
            Console.WriteLine("Array has been sorted");
            watch.Stop();
            Console.WriteLine($"Time: {watch.ElapsedMilliseconds}Ms");
        }
        //Linq sort method 
        static void linqSort(IList<string> arr)
        {
            IList<String> tempArr = new List<String>(arr);
            Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var linqSort  = from words in tempArr
                        orderby words ascending
                        select words;
            watch.Stop();
            Console.WriteLine($"Time: {watch.ElapsedMilliseconds}Ms");    
        }
        //Destinct Words method 
       static void diffrent(IList<string> arr)
        {
           int counter = arr.Distinct().Count();
           Console.WriteLine("Count: "+ counter);
        }
        //Takes the first 10 words and outputs it 
        static void tenWords(IList<string> arr)
        {
            //Using the .take i get the first 10 elements in the list 
            var firstTen = arr.Take(10);
            foreach (var i in firstTen)
                Console.WriteLine(i);
        }
        //Takes the number of words that starts with j and outputs them
        static void distinctJ(IList<string> arr)
        {
            //Using a linq query i am able to get only the words that begin with j
            var destJ = from word in arr
                            where word.StartsWith("j")
                            select word;
            Console.WriteLine("Count: "+ destJ.Count());
            Console.WriteLine(string.Join("\n", destJ));
        }
        //Takes the number of words that end with D and outputs them
        static void distinctD(IList<string> arr)
        {
            //Using a linq query i am able to get only the words that end with d
            var destD = from word in arr
                            where word.EndsWith("d")
                            select word;
            Console.WriteLine(string.Join("\n", destD));
            Console.WriteLine("Count: " + destD.Count());
        }
        //Get and display of words that are greater than 4 characters long
        static void longFour(IList<string> arr)
        {
            var fourLong = from word in arr
                        where word.Length>4
                        select word;
            foreach (string i in fourLong)
                Console.WriteLine(i);
            Console.WriteLine("Count: " + fourLong.Count());
        }
        //Displays words that are less than 3 characters long and start with the letter a
        static void threeA(IList<string> arr)
        {
            //In this query i use one where statment to get both the words that start with a and are larger than 3 
            var querySort = from word in arr
                            where ((word.StartsWith("a")) && (word.Length < 3))
                            select word;
            Console.WriteLine("Count: "+ querySort.Count());
            Console.WriteLine(string.Join("\n", querySort));
        }
    }
}
