using System.Text;

namespace WordCounter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Dictionary<string, int> CountWords(string path)
        {
            Dictionary<string, int> output = new();

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(path);

                // Split lines into words and add them to the list
                foreach (string line in lines)
                {
                    string[] lineWords = line.Split(' ');
                    foreach (string word in lineWords)
                    {
                        string keyword = RemovePunctuation(word);
                        if (!string.IsNullOrEmpty(keyword))
                        {
                            if (output.ContainsKey(keyword))
                            {
                                output[keyword] = output[keyword] + 1;
                            }
                            else
                            {
                                output.Add(keyword, 1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }

            return output;
        }

        private string RemovePunctuation(string word)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetter(word[i]))
                {
                    builder.Append(word[i]);
                }
            }
            return builder.ToString();
        }

        private void OpenFileClick(object sender, EventArgs e)
        {

            openFileDialog1.Title = "Open Text File";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Filter for text files
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                lbStat.DataSource = CountWords(filePath).ToList();
            }
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}