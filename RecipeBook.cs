using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using WindowsFormsApplication2;

public class RecipeBook
{
    private Form mainForm = new Form();
    private Panel leftPanel = new Panel();
    private Panel rightPanel = new Panel();
    private ListBox recipeList = new ListBox();
    private TextBox tb1 = new TextBox();
    private TextBox tb = new TextBox();
    private LList hashtable = new LList();
    private Label theContent = new Label();
    private ComboBox printList = new ComboBox();
    private int selectedIndex = 0;

    public RecipeBook()
    {
        splashForm.SetBounds(0, 0, 400, 400);
        splashForm.ControlBox = false;
        splashForm.MaximizeBox = false;
        splashForm.MinimizeBox = false;
        splashForm.ShowIcon = false;
        splashForm.StartPosition = FormStartPosition.CenterScreen;
        splashForm.FormBorderStyle = FormBorderStyle.None;
        splashForm.MinimumSize = new Size(400, 400);
        splashForm.MaximumSize = new Size(400, 400);
        
        Panel splashPanel = new Panel();
        splashPanel.SetBounds(0, 0, 400, 400);
        splashPanel.BackColor = Color.Transparent;

        Size size = new Size(400, 400);
        splashPanel.BackgroundImage = Image.FromFile("splash.jpg");

        Bitmap bmp = new Bitmap(splashPanel.BackgroundImage, size);
        splashPanel.BackgroundImage = bmp;

        splashForm.Controls.Add(splashPanel);
    }

    public void ShowBook()
    {
        mainForm.SetBounds(0, 0, 1000, 600);
        mainForm.Controls.Add(leftPanel);
        mainForm.Controls.Add(rightPanel);

        mainForm.Text = "RecipeBook";

        mainForm.FormBorderStyle = FormBorderStyle.FixedSingle;
        mainForm.MaximizeBox = false;

        leftPanel.SetBounds(0, 0, 250, 600);
        rightPanel.SetBounds(250, 0, 750, 600);

        mainForm.Icon = new Icon("block.ico");

        AddList();
        rightPanel.Controls.Clear();
        rightPanel.Controls.Add(DisplaySelectedText());

        AddContentViewBg();
        AddButtons();
    }

    private void AddContentViewBg()
    {
        Size size = new Size(600, 600);
        rightPanel.BackgroundImage = Image.FromFile(@"bg.png");

        Bitmap bmp = new Bitmap(rightPanel.BackgroundImage, size);
        rightPanel.BackgroundImage = bmp;
    }

    private void AddButtons()
    {
        Button upButton = new Button();
        upButton.Text = "Up";
        upButton.SetBounds(600, 100, 100, 50);
        rightPanel.Controls.Add(upButton);
        upButton.Click += new EventHandler(MoveTextDown);

        Button downButton = new Button();
        downButton.Text = "Down";
        downButton.SetBounds(600, 150, 100, 50);
        rightPanel.Controls.Add(downButton);
        downButton.Click += new EventHandler(MoveTextUp);

        RadioButton deleteButton = new RadioButton();
        deleteButton.SetBounds(660, 0, 80, 40);
        deleteButton.Text = "REMOVE";
        deleteButton.ForeColor = Color.Black;
        deleteButton.BackColor = Color.Transparent;
        rightPanel.Controls.Add(deleteButton);
        deleteButton.Click += new EventHandler(DeleteCurrentRecipe);

        RadioButton printButton = new RadioButton();
        printButton.SetBounds(600, 0, 80, 40);
        printButton.Text = "PRINT";
        printButton.ForeColor = Color.Black;
        printButton.BackColor = Color.Transparent;
        rightPanel.Controls.Add(printButton);
        printButton.Click += new EventHandler(PrintRecipe);

        printList.SetBounds(600, 40, 130, 20);
        foreach (String printer in PrinterSettings.InstalledPrinters)
        {
            printList.Items.Add(printer.ToString());
        }
        rightPanel.Controls.Add(printList);
        mainForm.Hide();
        mainForm.Show();
    }

    private void PrintRecipe(object sender, EventArgs e)
    {
        //Create a PrintDocument object
        PrintDocument pd = new PrintDocument();

        //Set PrinterName as the selected printer in the printers list
        pd. PrinterSettings.PrinterName =
        printList.SelectedItem.ToString();

        //Add PrintPage event handler
        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

        //Print the document
        pd.Print();
    }

    public void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
        //Get the Graphics object
        Graphics g = ev.Graphics;

        //Create a font Arial with size 16
        Font font = new Font("Arial", 16);

        //Create a solid brush with black color
        SolidBrush brush = new SolidBrush(Color.Black);

        //Draw theContent.text;
        g.DrawString(theContent.Text,
        font, brush,
        new Rectangle(20, 20, 200, 100));
    }

    private void DeleteCurrentRecipe(object sender, EventArgs e)
    {
        hashtable.Remove(hashtable[recipeList.SelectedIndex]);
        recipeList.Items.Remove(recipeList.Items[recipeList.SelectedIndex]);
        Serialize(null, null);

        NewRecipe(null, null);
        mainForm.Hide();
        mainForm.Show();
    }

    private void MoveTextUp(object sender, EventArgs e)
    {
        Label content=theContent;
        Console.WriteLine(content.Text);
        content.SetBounds(0, content.Top - 70, 600, 6000);
        rightPanel.Hide();
        rightPanel.Show();
    }

    private void MoveTextDown(object sender, EventArgs e)
    {
        Label content = theContent;
        Console.WriteLine(content.Text);
        content.SetBounds(0, content.Top + 70, 600, 6000);
        rightPanel.Hide();
        rightPanel.Show();
    }

    private void AddList()
    {
        Deserialize(null, null);

        Button neew = new Button();
        neew.Text = "Make Recipe";
        neew.SetBounds(0, 0, 86, 20);
        leftPanel.Controls.Add(neew);
        neew.Click += new EventHandler(NewRecipe);

        recipeList.SetBounds(0, 100, 250, 500);
        for (int i = 0; i < hashtable.Count; i++)
        {
            recipeList.Items.Add(hashtable[i].key);
        }
        leftPanel.Controls.Add(recipeList);
        recipeList.Click += new EventHandler(DisplayRecipe);
        recipeList.ScrollAlwaysVisible = true;

        recipeList.SetBounds(0, 100, 200, 500);
        leftPanel.AutoScroll = true;
    }

    private void Deserialize(object sender, EventArgs e)
    {
        hashtable = null;
        string path = "Test.txt";

        XmlSerializer serializer = new XmlSerializer(typeof(LList));

        StreamReader reader = new StreamReader(path);
        hashtable = (LList)serializer.Deserialize(reader);
        reader.Close();
    }

    private void NewRecipe(object sender, EventArgs e)
    {
        rightPanel.Controls.Clear();

        Label title = new Label();
        title.Text = "Title of recipe";
        title.BackColor = Color.Transparent;
        title.SetBounds(0, 0, 180, 25);
        rightPanel.Controls.Add(title);

        tb1.SetBounds(0, 25, 600, 40);

        Label cont = new Label();
        cont.Text = "Recipe";
        cont.BackColor = Color.Transparent;
        cont.SetBounds(0, 65, 80, 25);
        rightPanel.Controls.Add(cont);

        tb.Multiline = true;
        tb.SetBounds(0, 90, 600, 300);
        tb.Text = "";
        tb1.Text = "";

        tb1.ForeColor = Color.White;
        tb.ForeColor = Color.White;
        tb1.BackColor = Color.Black;
        tb.BackColor = Color.Black;

        Button saveOrUpdate = new Button();
        saveOrUpdate.Text = "SAVE";
        saveOrUpdate.SetBounds(300, 420, 50, 20);
        saveOrUpdate.Click += new EventHandler(Serialize);

        rightPanel.Controls.Add(saveOrUpdate);
        rightPanel.Controls.Add(tb1);
        rightPanel.Controls.Add(tb);
    }

    private void Serialize(object sender, EventArgs e)
    {
        const string fileName = "Test.txt";
        recipeList.Items.Clear();
        if (sender != null)
        {
            hashtable.Add(new Item(tb1.Text, tb.Text));
        }
        for (int i = 0; i < hashtable.Count; i++)
        {
            recipeList.Items.Add(hashtable[i].key);
        }
        SerializeCollection(fileName, hashtable);
        mainForm.Hide();
        mainForm.Show();
    }

    public void SerializeCollection<T>(string fullFileName, IEnumerable<T> items)
    {
        var writer = new XmlSerializer(items.GetType());
        var file = new StreamWriter(fullFileName);
        writer.Serialize(file, items);
        file.Close();
        writer = null;
    }

    private void DisplayRecipe(object sender, EventArgs e)
    {
        selectedIndex = recipeList.SelectedIndex;

        rightPanel.Controls.Clear();
        AddButtons();
        rightPanel.Controls.Add(DisplaySelectedText());
    }

    private Label DisplaySelectedText()
    {
        if (selectedIndex != -1)
        {
            Label dynamicTextBox = new Label();
            dynamicTextBox.BackColor = Color.Transparent;
            dynamicTextBox.ForeColor = Color.Black;
            dynamicTextBox.Font = new Font("Georgia", 16);
            dynamicTextBox.SetBounds(0, 0, 600, 6000);
            
            if(selectedIndex > -1 && selectedIndex < recipeList.Items.Count)
                dynamicTextBox.Text = recipeList.Items[selectedIndex] + "\n\n" + hashtable[selectedIndex].value;
            
            theContent = dynamicTextBox;

            return dynamicTextBox;
        }
        else
        {
            Label lbla = new Label();

            theContent = lbla;

            return theContent;
        }
    }

    private Form splashForm = new Form();

    public static void Main()
    {
        RecipeBook recipeBook = new RecipeBook();
        recipeBook.splashForm.Show();
        new System.Threading.ManualResetEvent(false).WaitOne(1250);
        recipeBook.splashForm.Close();
        recipeBook.ShowBook();
        recipeBook.mainForm.Show();
        Application.Run(recipeBook.mainForm);
    }

    public XmlWriter WriterFileStream { get; set; }
}