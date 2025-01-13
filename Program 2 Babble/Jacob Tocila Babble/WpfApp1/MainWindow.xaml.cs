////////////////////////////////////////////////////////////////////
///
///   Jacob Tocila CS 212 Babble Program September 28, 2023
///
////////////////////////////////////////////////////////////////////


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics.Eventing.Reader;

namespace BabbleSample
{
    /// Babble framework
    /// Starter code for CS212 Babble assignment
    public partial class MainWindow : Window
    {
        private string input;               // input file
        private string[] words;             // input file broken into array of words
        private int wordCount = 200;        // number of words to babble
        public MainWindow()
        {
            InitializeComponent();
        }

        Dictionary<string, ArrayList> makeHashtable()
        {
            Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();
            // This part was fun, coding the hash table felt like a recursive project since it just kept getting longer and longer but with slightly different variables
            if (orderComboBox.SelectedIndex == 1)
            {
                for (int i = 0; i < words.Length - 1; i++)
                {
                    string oneWord = words[i];
                    if (!hashTable.ContainsKey(oneWord))
                        hashTable.Add(oneWord, new ArrayList());
                    hashTable[oneWord].Add(words[i + 1]);
                }
            }
            else if (orderComboBox.SelectedIndex == 2)
            {
                for (int i = 1; i < words.Length - 1; i++)
                {
                    string twoWords = words[i - 1] + " " + words[i];
                    if (!hashTable.ContainsKey(twoWords))
                        hashTable.Add(twoWords, new ArrayList());
                    hashTable[twoWords].Add(words[i + 1]);
                }
            }
            else if (orderComboBox.SelectedIndex == 3)
            {
                for (int i = 2; i < words.Length - 1; i++)
                {
                    string threeWords = words[i - 2] + " " + words[i - 1] + " " + words[i];
                    if (!hashTable.ContainsKey(threeWords))
                        hashTable.Add(threeWords, new ArrayList());
                    hashTable[threeWords].Add(words[i + 1]);
                }
            }
            else if (orderComboBox.SelectedIndex == 4)
            {
                for (int i = 3; i < words.Length - 1; i++)
                {
                    string fourWords = words[i - 3] + " " + words[i - 2] + " " + words[i - 1] + " " + words[i];
                    if (!hashTable.ContainsKey(fourWords))
                        hashTable.Add(fourWords, new ArrayList());
                    hashTable[fourWords].Add(words[i + 1]);
                }
            }
            else if (orderComboBox.SelectedIndex == 5)
            {
                for (int i = 4; i < words.Length - 1; i++)
                {
                    string fourWords = words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1] + " " + words[i];
                    if (!hashTable.ContainsKey(fourWords))
                        hashTable.Add(fourWords, new ArrayList());
                    hashTable[fourWords].Add(words[i + 1]);
                }
            }
            return hashTable;
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "Sample"; // Default file name
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            if ((bool)ofd.ShowDialog())
            {
                textBlock1.Text = "Loading file " + ofd.FileName + "\n";
                input = System.IO.File.ReadAllText(ofd.FileName);  // read file
                words = Regex.Split(input, @"\s+");       // split into array of words
            }
        }

        private void analyzeInput(int order)
        {
            if (order > 0)
            {
                MessageBox.Show("Analyzing at order: " + order);
                
            }
        }

        private void babbleButton_Click(object sender, RoutedEventArgs e)
        {
            textBlock1.Text = "";
            /*
            Random random = new Random();
            string key = Hashtable.Keys.ElementAt(0);
            int length = key.Length;
            int randomNumber = random.Next(0, length);
            string element = Hashtable.Keys.ElementAt(0).;

            
            for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                textBlock1.Text += " " + words[i];
            for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            {
                textBlock1.Text += " " + SecondWordInHash;
                if (i < wordCount) {
                    SecondWordInHash = words[i];
                }
                else
                {
                    SecondWordInHash = words[0];
                }
            }
            */
            if (orderComboBox.SelectedIndex == 1) {
                for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                    textBlock1.Text += " " + words[i];
            } 

            // old garbage code
            /*else if (orderComboBox.SelectedIndex > 0)
            {
                foreach (KeyValuePair<string, ArrayList> entry in makeHashtable())
                {
                    
                    int randomNumber = new Random().Next(0, orderComboBox.SelectedIndex);
                    textBlock1.Text += words[0] + entry.Element(0, randomNumber);
                }
              
            }*/
            
            // Struggled with this part... program did not entirely work as expected when implementing this part
            // to gain full credit. Even with online help I don't think this entirely works as intended... from here
            else if (orderComboBox.SelectedIndex > 0)
            {
                Random random = new Random();

                foreach (KeyValuePair<string, ArrayList> entry in makeHashtable())
                {
                    int listCount = entry.Value.Count;

                    if (listCount > 0)
                    {
                        int randomNumber = random.Next(0, listCount);
                        string randomElement = entry.Value[randomNumber].ToString();

                        textBlock1.Text += randomElement + " ";
                    }
                }
            }
            // to here

            else
            {
                // I need the output to use 200 words, but if I change the parameter, it doesn't alwys work if the .txt file doesn't
                // have 200 words to work with. Not entirely sure what to do here or how to resolve...
                for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                    textBlock1.Text += " " + words[i];
            }

        }
    
        private void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            analyzeInput(orderComboBox.SelectedIndex);
        }
    }
}
