using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;

namespace TranslateWords
{
    public partial class translatewords : System.Web.UI.Page
    {
        public class SearchParameters
        {
            public string EnglishWords { get; set; }
            public string FrenchWords { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FindAndReplace();
        }

        private void FindAndReplace()
        {
            //Read and store the text file 
            string file = @"C:\Documents\find_words.txt";

            //Memory processed 
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            long memoryUsage = currentProcess.WorkingSet64;

            // Read all the content in one string
            string str = File.ReadAllText(file);
            string[] words = str.Split(' ');

            int count = 0;
            var listofwords = "";

            //loading csv data into the data table from the file using File.OpenRead() method
            var csvTable = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(@"D:\CSVFolder\french_dictionary.csv")), true))
            {
                csvTable.Load(csvReader);
            }
            string eng_words = csvTable.Columns[0].ToString();
            string french_words = csvTable.Columns[1].ToString();

            List<SearchParameters> searchParameters = new List<SearchParameters>();

            for (int i = 0; i < csvTable.Rows.Count; i++)
            {
                searchParameters.Add(new SearchParameters
                {
                    EnglishWords = csvTable.Rows[i][0].ToString(),
                    FrenchWords = csvTable.Rows[i][1].ToString()
                });
            }


            //calculating start time
            DateTime startingTime = DateTime.UtcNow;
            for (int i = 0; i < words[i].Length; i++)
            {
                //finding words if it has the replacement in dictionary csv file
                if (words[i].Contains(eng_words))
                {
                    //replace with french words
                    words[i].Replace(words[i], french_words);

                    //save the processed file
                    File.WriteAllText("C:\\Documents\find_words.txt", file);

                    listofwords += words[i] + ", ";
                    var length = words.Length;
                    count++;
                }
            }

            //calculating end time and difference
            DateTime endTime = DateTime.UtcNow;
            TimeSpan timeDifference = endTime - startingTime;

            double seconds = timeDifference.TotalSeconds;

            TextWriter txt = new StreamWriter("C:\\demo\\find_words.txt");
            txt.Close();

            //displaying list of words, count, time and memory processed
            Console.WriteLine(listofwords);
            Console.WriteLine(count);
            Console.WriteLine(seconds);
            Console.WriteLine(memoryUsage);
        }
    }
}