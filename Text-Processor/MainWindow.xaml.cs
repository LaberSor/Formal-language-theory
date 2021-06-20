using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Microsoft.Win32;

namespace Text_Processor
{
    
    public partial class MainWindow : Window
    {

        /*################
        Блок обработки стандартных функций текстового процессора
        #################*/

        string FileName = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextBox1.Selection.Text = "";
        }

        private void ClearTextBuffer()
        {
            TextBox1.IsUndoEnabled = false;
            TextBox1.IsUndoEnabled = true;
        }

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {

            TextRange textRange1 = new TextRange(
            TextBox1.Document.ContentStart,
            TextBox1.Document.ContentEnd);

            bool checkFlag = true;
            string check = "";

            if (FileName != "")
                check = File.ReadAllText(FileName);
            else
                checkFlag = false;

            if (check == textRange1.Text && checkFlag)
            {
                TextBox1.Document.Blocks.Clear();
                ClearTextBuffer();
                FileName = "";
            }
            else
            {
                string sMessageBoxText = "Сохранить текущий файл?";
                string sCaption = "Текущий файл не сохранен";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        SaveFile(sender, e);
                        TextBox1.Document.Blocks.Clear();
                        ClearTextBuffer();
                        FileName = "";
                        break;

                    case MessageBoxResult.No:
                        TextBox1.Document.Blocks.Clear();
                        ClearTextBuffer();
                        FileName = "";
                        break;

                    case MessageBoxResult.Cancel:
                        /* ... */
                        break;
                }
            }
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {

            TextRange textRange1 = new TextRange(
            TextBox1.Document.ContentStart,
            TextBox1.Document.ContentEnd);

            bool checkFlag = true;
            string check = "";

            if (FileName != "")
                check = File.ReadAllText(FileName);
            else
                checkFlag = false;

            if (check == textRange1.Text && checkFlag)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|All files (*.*)|*.*";

                if (ofd.ShowDialog() == true)
                {
                    TextRange doc = new TextRange(TextBox1.Document.ContentStart, TextBox1.Document.ContentEnd);
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                            doc.Load(fs, DataFormats.Rtf);
                        else if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                            doc.Load(fs, DataFormats.Text);
                        else
                            doc.Load(fs, DataFormats.Xaml);
                    }
                    FileName = ofd.FileName;
                }
                ClearTextBuffer();
            }
            else
            {
                string sMessageBoxText = "Сохранить текущий файл?";
                string sCaption = "Текущий файл не сохранен";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        SaveFile(sender, e);

                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|All files (*.*)|*.*";

                        if (ofd.ShowDialog() == true)
                        {
                            TextRange doc = new TextRange(TextBox1.Document.ContentStart, TextBox1.Document.ContentEnd);
                            using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                            {
                                if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                                    doc.Load(fs, DataFormats.Rtf);
                                else if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                                    doc.Load(fs, DataFormats.Text);
                                else
                                    doc.Load(fs, DataFormats.Xaml);
                            }
                            FileName = ofd.FileName;
                        }
                        ClearTextBuffer();
                        break;

                    case MessageBoxResult.No:
                        ofd = new OpenFileDialog();
                        ofd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|All files (*.*)|*.*";

                        if (ofd.ShowDialog() == true)
                        {
                            TextRange doc = new TextRange(TextBox1.Document.ContentStart, TextBox1.Document.ContentEnd);
                            using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                            {
                                if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                                    doc.Load(fs, DataFormats.Rtf);
                                else if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                                    doc.Load(fs, DataFormats.Text);
                                else
                                    doc.Load(fs, DataFormats.Xaml);
                            }
                            FileName = ofd.FileName;
                        }
                        ClearTextBuffer();
                        break;

                    case MessageBoxResult.Cancel:
                        /* ... */
                        break;
                }
            }
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (FileName != "")
            {
                TextRange doc = new TextRange(TextBox1.Document.ContentStart, TextBox1.Document.ContentEnd);
                using (FileStream fs = File.Create(FileName))
                {
                    if (System.IO.Path.GetExtension(FileName).ToLower() == ".rtf")
                        doc.Save(fs, DataFormats.Rtf);
                    else if (System.IO.Path.GetExtension(FileName).ToLower() == ".txt")
                        doc.Save(fs, DataFormats.Text);
                    else
                        doc.Save(fs, DataFormats.Xaml);
                }
            }
            else
            {
                SaveAsFile(sender, e);
            }
        }

        private void SaveAsFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(TextBox1.Document.ContentStart, TextBox1.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (System.IO.Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                        doc.Save(fs, DataFormats.Rtf);
                    else if (System.IO.Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                        doc.Save(fs, DataFormats.Text);
                    else
                        doc.Save(fs, DataFormats.Xaml);
                }

                FileName = sfd.FileName;

            }

        }

        private void Close(object sender, RoutedEventArgs e)
        {
            TextRange textRange1 = new TextRange(
            TextBox1.Document.ContentStart,
            TextBox1.Document.ContentEnd);

            bool checkFlag = true;
            string check = "";

            if (FileName != "")
                check = File.ReadAllText(FileName);
            else
                checkFlag = false;

            if (check == textRange1.Text && checkFlag)
            {
                TextBox1.Document.Blocks.Clear();
                FileName = "";
            }
            else
            {
                string sMessageBoxText = "Сохранить текущий файл?";
                string sCaption = "Текущий файл не сохранен";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        SaveFile(sender, e);
                        Close();
                        break;

                    case MessageBoxResult.No:
                        Close();
                        break;

                    case MessageBoxResult.Cancel:
                        /* ... */
                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void helpFile(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("../Text-Processor/help/help.html");
        }

        private void about(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("О программе:");
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void deleteAll(object sender, RoutedEventArgs e)
        {
            TextBox1.SelectAll();
            TextBox1.Selection.Text = "";
        }

        private void TextBox1_EventHandler(object sender, TextChangedEventArgs args)
        {
            TextRange MyText = new TextRange(TextBox1.Document.ContentStart, TextBox1.Document.ContentEnd);

            string[] splittedLines = MyText.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (LineTextBox1 != null)
            {
                LineTextBox1.SelectAll();
                LineTextBox1.Selection.Text = "";
                for (int i = 1; i < splittedLines.Length; i++)
                {
                    LineTextBox1.AppendText(i.ToString() + "\n");
                }
            }
        }

        private void TextBox2_EventHandler(object sender, TextChangedEventArgs args)
        {
            string[] rtfStrings = TextBox2.Selection.Text.Split(new char[] { '\n' });

            //for (int i = 1; i <= rtfStrings.Length; i++)
            //{
              //  LineTextBox2.Selection.Text += i.ToString() + "\n";
            //}
        }

        private void LineTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /*################
        Блок функций по ЛР
        #################*/

        private void findRegEx(object sender, RoutedEventArgs e)
        {
            TextBox2.SelectAll();
            TextBox2.Selection.Text = "\n";

            TextRange textRange1 = new TextRange(
            TextBox1.Document.ContentStart,
            TextBox1.Document.ContentEnd);

            char[] separators = new char[] { ' ', '\r', '\n' };

            string[] tmp = null;
            tmp = textRange1.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            System.Text.RegularExpressions.Regex regexp = new System.Text.RegularExpressions.Regex(@"((https?|ftp):\/\/)?(www\.)?[a-zA-Z0-9\-\.\~\\_]+.\b(\.){1}(ru|com|org|net|mil|edu|COM|ORG|NET|MIL|EDU){1}(\/[a-zA-Z0-9\-\.]+)*");
            foreach (string S in tmp)
            {
                if (regexp.IsMatch(S) == true)
                {
                    TextBox2.AppendText(S + '\n');
                }
            }
        }

        private void stateMachine(object sender, RoutedEventArgs e)
        {
            TextBox2.SelectAll();
            TextBox2.Selection.Text = "\n";

            StateMachine sm = new StateMachine();

            TextRange textRange1 = new TextRange(
            TextBox1.Document.ContentStart,
            TextBox1.Document.ContentEnd);

            string str = textRange1.Text;
            char[] text = str.ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n' || text[i] == '\r')
                {

                }
                if (sm.SymbolChecking(text[i]) == false && (text[i] != '\n' || text[i] != '\r'))
                {
                    sm = new StateMachine();
                    i--;
                }
                if (sm.getState().ToString() == "q23")
                {
                    TextBox2.AppendText(sm.states + "\n");
                    //stateMachine.states = "q1->";
                    TextBox2.AppendText(sm.getStr() + "\n" + "TRUE");
                    sm = new StateMachine();
                }
            }
        }

        private void grammarCheck(object sender, EventArgs e)
        {

            TextBox2.SelectAll();
            TextBox2.Selection.Text = "\n";

            TextRange textRange1 = new TextRange(
            TextBox1.Document.ContentStart,
            TextBox1.Document.ContentEnd);

            string str = textRange1.Text;
            var symbolizer = new Symbolizer(str);
            var symbol = symbolizer.GetSymbol();

            while (symbol.Type != SymbolType.Eof)
            {
                TextBox2.AppendText(((int)symbol.Type).ToString() + " " + symbol.Value + "\n");
                symbol = symbolizer.GetSymbol();
            }
        }

        /*################
        КУРСОВАЯ РАБОТА
        #################*/
        private void basicCheck(object sender, EventArgs e)
        {
            TextBox2.SelectAll();
            TextBox2.Selection.Text = "\n";

            TextRange textRange1 = new TextRange(
            TextBox1.Document.ContentStart,
            TextBox1.Document.ContentEnd);
            string str = textRange1.Text;
            str = str.Remove(str.Length - 2);

            string temp_str = String.Empty;
            var basicscaner = new BasicScaner(str);
            var symbol = basicscaner.mainSyntaxis();
            int i = 0;
            while (i < symbol.Length)
            {
                if (symbol[i] == '|')
                {
                    TextBox2.AppendText(temp_str + "\n");
                    temp_str = String.Empty;
                }
                else
                {
                    temp_str += symbol[i];
                }
                i++;
            }
        }

    }
}
