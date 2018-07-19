using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using PluginContracts;
namespace VisualStudent
{
    class MySyntaxHighlight1 : IPlugin
    {
        public string Name => "MySyntaxHighlight1";

        List<string> keywords = new List<string>() { "public", "private", "get", "set" };

        public void Do(RichTextBox richTextBox)
        {
            foreach(var wordRange in GetAllWordRanges(richTextBox.Document))
            {
                if(keywords.Contains(wordRange.Text))
                {
                    wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                    wordRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                }
            }
        }

        public static IEnumerable<TextRange> GetAllWordRanges(FlowDocument document)
        {
            string pattern = @"[^\W\d](\w|[-']{1,2}(?=\w))*";
            TextPointer pointer = document.ContentStart;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern);
                    foreach (Match match in matches)
                    {
                        int startIndex = match.Index;
                        int length = match.Length;
                        TextPointer start = pointer.GetPositionAtOffset(startIndex);
                        TextPointer end = start.GetPositionAtOffset(length);
                        yield return new TextRange(start, end);
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
