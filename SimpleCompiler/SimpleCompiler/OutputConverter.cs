using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;

namespace VisualStudent
{
    class OutputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FlowDocument doc = new FlowDocument();
            doc.PageWidth = 200000;
            doc.PagePadding = new System.Windows.Thickness(0,0,0,0);
            doc.LineHeight = 3;
            

            string s = value as string;
            if (s != null)
            {
                using (StringReader reader = new StringReader(s))
                {
                    string newLine;
                    while ((newLine = reader.ReadLine()) != null)
                    {
                        if (String.IsNullOrWhiteSpace(newLine) || String.IsNullOrEmpty(newLine))
                            continue;
                        newLine=newLine.Trim();
                        Paragraph paragraph = null;
                        paragraph = new Paragraph(new Run(newLine));    

                        doc.Blocks.Add(paragraph);
                    }
                }
            }

            return doc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
