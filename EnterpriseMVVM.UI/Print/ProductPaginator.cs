using EnterpriseMVVM.Model;
using Mohsenmou.MVVM.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace EnterpriseMVVM.UI.Print
{
    public class ProductPaginator : ModelPaginator<Product>
    {
        public ProductPaginator(
            ObservableCollection<Product> items,
            Typeface typeface,
            double fontSize,
            double margin,
            Size pageSize) : base(items, typeface, fontSize, margin, pageSize)
        {

        }
        public override DocumentPage GetPage(int pageNumber)
        {
            // Create a test string for the purposes of measurement.
            FormattedText text = GetFormattedText("A");
            // Size columns relative to the width of one "A" letter.
            // It's a shortcut that works in this example.
            double col1_X = Margin;
            double col2_X = col1_X + text.Width * 10;
            double col3_X = col2_X + text.Width * 30;
            double col4_X = col3_X + text.Width * 20;

            // Calculate the range of rows that fits on this page.
            int minRow = pageNumber * RowsPerPage;
            int maxRow = minRow + RowsPerPage;

            // Create the visual for the page.
            DrawingVisual visual = new DrawingVisual();

            // Initially, set the position to the top-left corner of the printable area.
            Point point = new Point(Margin, Margin);

            // Print the column values.
            using (DrawingContext dc = visual.RenderOpen())
            {
                // Draw the column headers.
                Typeface columnHeaderTypeface = new Typeface(Typeface.FontFamily,
                                                             FontStyles.Normal,
                                                             FontWeights.Bold,
                                                             FontStretches.Normal);
                point.X = col1_X;
                text = GetFormattedText("No.", columnHeaderTypeface);
                dc.DrawText(text, point);

                text = GetFormattedText("Name", columnHeaderTypeface);
                point.X = col2_X;
                dc.DrawText(text, point);

                text = GetFormattedText("Unit Price", columnHeaderTypeface);
                point.X = col3_X;
                dc.DrawText(text, point);

                text = GetFormattedText("Category", columnHeaderTypeface);
                point.X = col4_X;
                dc.DrawText(text, point);

                // Draw the line underneath.
                dc.DrawLine(new Pen(Brushes.Black, 2),
                    new Point(Margin, Margin + text.Height),
                    new Point(PageSize.Width - Margin, Margin + text.Height));

                point.Y += text.Height;

                // Draw the column values.
                for (int i = minRow; i < maxRow; i++)
                {
                    // Check for the end of the last (half-filled) page.
                    if (i > (Items.Count - 1)) break;

                    point.X = col1_X;
                    text = GetFormattedText((i + 1).ToString());
                    dc.DrawText(text, point);

                    // Add second column.                    
                    text = GetFormattedText(Items[i].Name);
                    point.X = col2_X;
                    dc.DrawText(text, point);

                    // Add third column.                    
                    text = GetFormattedText(Items[i].UnitPrice.ToString("#,##"));
                    point.X = col3_X;
                    dc.DrawText(text, point);

                    // Add forth column.  
                    text = GetFormattedText(Items[i].Category.Name);
                    point.X = col4_X;
                    dc.DrawText(text, point);

                    point.Y += text.Height;
                }
            }
            return new DocumentPage(visual,
                                    PageSize,
                                    new Rect(PageSize),
                                    new Rect(PageSize));
        }
    }
}
