using System;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

public class PrintManager
{
    private System.Windows.Forms.ComboBox printList;
    
    private Label theContent = null;

    public PrintManager(Button printButton, ComboBox printList, Label theContent)
    {
        this.printList = printList;
    
        this.theContent = theContent;

        printButton.Click += new EventHandler(PrintRecipe);

        foreach (String printer in PrinterSettings.InstalledPrinters)
        {
            printList.Items.Add(printer.ToString());
        }

    }

    private void PrintRecipe(object sender, EventArgs e)
    {
        PrintDocument pd = new PrintDocument();

        pd.PrinterSettings.PrinterName = printList.SelectedItem.ToString();
        
        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

        pd.Print();
    }

    static int count = 0;
    static float yPos = 0;
    static int si = 0;
    static int ei = 59;

    public void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
        yPos = 0;
        Font printFont = new Font("Arial", 16);
        //666//float linesPerPage = 0;
        float leftMargin = ev.MarginBounds.Left;
        float topMargin = ev.MarginBounds.Top;
        string line = null;
        // Calculate the number of lines per page.
        //666///linesPerPage = 40;
        // Print each line of the file.
        Console.WriteLine("" + theContent.Text.Length);
        if (theContent.Text.Length < 1)
            return;
        if (theContent.Text.Length - 1 < ei)
        {
            ei = theContent.Text.Length;
        }
        try
        {
            while (ei < theContent.Text.Length && yPos < 700)//666//count < linesPerPage && (si < theContent.Text.Length && ei < theContent.Text.Length))
            {
                line = theContent.Text.Substring(si, ei);
                si += 45;
                ei += 45;
                if (si > theContent.Text.Length - 1)
                {
                    si += theContent.Text.Length - 1 - 50;
                    ei = theContent.Text.Length - 1;
                }
                else if (ei > theContent.Text.Length - 1)
                {
                    si += theContent.Text.Length - 1 - 50;
                    ei = theContent.Text.Length - 1;
                }
                yPos += 35;
                //yPos = topMargin + (count *
                //printFont.GetHeight(ev.Graphics));
                //ev.Graphics.DrawString(line, printFont, Brushes.Black,
                //leftMargin, yPos, new StringFormat());
                ev.Graphics.DrawString(line, printFont, Brushes.Black, 0, yPos);
                count++;
            }
            if (ei > theContent.Text.Length)
            {
                line = null;
            }
        }
        catch (Exception e)
        {
            line = null;
        }
        // If more lines exist, print another page.
        if (line != null)
            ev.HasMorePages = true;
        else
            ev.HasMorePages = false;
    }
}