using System;
using System.Media;

namespace SpeechRecognition
{
    public partial class Form1 : Form
    {
        private bool IsPlayed, IsSoundCheckCreate;
        private SoundPlayer SoundCheker;

        static Label l;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.FilterIndex = 1;
            openFile.ShowDialog();
            if (openFile.FileName != "")
            {
                label1.Text = openFile.FileName;
            }
            else
            {
                label1.Text = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if((label1.Text != "" || label1.Text != null) && !IsSoundCheckCreate)
            {
                SoundCheker = new SoundPlayer(label1.Text);
                l = label2;
                Microsoft.Speech.Recognition.SpeechRecognitionEngine sre = new Microsoft.Speech.Recognition.SpeechRecognitionEngine(new System.Globalization.CultureInfo("ru-ru"));
                sre.SetInputToWaveFile(label1.Text);
                sre.SpeechRecognized += new EventHandler<Microsoft.Speech.Recognition.SpeechRecognizedEventArgs>(sre_SpeechRecognized);
                Microsoft.Speech.Recognition.Choices colors = new Microsoft.Speech.Recognition.Choices();
                string[] WordsMassiv = richTextBox1.Text.Split(' ', '\n', ',', '.', '/', '\\', '!', '\"', '\'', '?', ':');
                List<string> CleanWordMassiv = new List<string>();
                for (int i = 0; i < WordsMassiv.Length; i++)
                {
                    if (WordsMassiv[i] != "")
                    {
                        CleanWordMassiv.Add(WordsMassiv[i]);
                    }
                }
                CleanWordMassiv = CleanWordMassiv.Distinct().ToList();
                Array.Clear(WordsMassiv);
                for (int i = 0; i < CleanWordMassiv.Count; i++)
                {
                    WordsMassiv[i] = CleanWordMassiv[i];
                }
                colors.Add(WordsMassiv);
                Microsoft.Speech.Recognition.GrammarBuilder gb = new Microsoft.Speech.Recognition.GrammarBuilder();
                // gb.Culture = ci;
                gb.Append(colors);
                Microsoft.Speech.Recognition.Grammar g = new Microsoft.Speech.Recognition.Grammar(gb);
                sre.LoadGrammar(g);
                sre.RecognizeAsync(Microsoft.Speech.Recognition.RecognizeMode.Multiple);
                IsSoundCheckCreate = true;
            }
            else
            {
                MessageBox.Show("Пустой путь");
            }
            if (IsPlayed)
            {
                SoundCheker.Stop();
                IsPlayed = false;
            }
            else
            {
                SoundCheker.Play();
                IsPlayed = true;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }
        static void sre_SpeechRecognized(object sender, Microsoft.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.82)
            {
                l.Text += e.Result.Text;
            }
        }
    }
}