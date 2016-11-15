using System;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

public class PrintManager
{
    private System.Windows.Forms.ComboBox printList;
    
    private String theContent = "";

    public PrintManager(RadioButton printButton, ComboBox printList, string theContent)
    {
        this.printList = printList;
    
        this.theContent = theContent;

        printButton.Click += new EventHandler(PrintRecipe);

        printList.SetBounds(600, 40, 130, 20);
        
        foreach (String printer in PrinterSettings.InstalledPrinters)
        {
            printList.Items.Add(printer.ToString());
        }

    }

    private void PrintRecipe(object sender, EventArgs e)
    {
        PrintDocument pd = new PrintDocument();

        if (printList.SelectedIndex > -1)
        {
            pd.PrinterSettings.PrinterName = printList.SelectedItem.ToString();
        }
        else
        {
            printList.SelectedIndex = -1;
        }

        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

        pd.Print();
    }

    public void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
        Graphics g = ev.Graphics;

        Font font = new Font("Arial", 16);

        SolidBrush brush = new SolidBrush(Color.Black);

        g.DrawString(theContent, font, brush, new Rectangle(20, 20, 200, 100));
    }
}