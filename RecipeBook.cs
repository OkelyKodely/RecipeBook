using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

public class RecipeBook
{
    private int selectedIndex = -1;

    private Form mainForm = new Form();

    private Panel leftPanel = new Panel();

    private Panel rightPanel = new Panel();

    private ComboBox recipeList = new ComboBox();

    private TextBox tb1 = new TextBox();

    private TextBox tb = new TextBox();

    private LList recipes = new LList();

    private Label theContent = new Label();

    private Serializer serializer = new Serializer();

    private Button upButton = new Button();

    private Button downButton = new Button();

    private Form splashForm = new Form();

    private Label selectRecipe = new Label();

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

        recipeList.MaxDropDownItems = 5;

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
        upButton.Text = "Up";
        upButton.SetBounds(604, 170, 44, 24);
        upButton.Click += new EventHandler(MoveTextDown);

        rightPanel.Controls.Add(upButton);

        downButton.Text = "Down";
        downButton.SetBounds(650, 170, 44, 24);
        downButton.Click += new EventHandler(MoveTextUp);

        rightPanel.Controls.Add(downButton);

        Button editButton = new Button();
        editButton.SetBounds(600, 300, 80, 40);
        editButton.Text = "Edit";
        editButton.Click += new EventHandler(EditCurrentRecipe);

        rightPanel.Controls.Add(editButton);


        Button printDocumentButton = new Button();
        printDocumentButton.SetBounds(600, 20, 60, 30);
        printDocumentButton.Text = "Print";
        printDocumentButton.Click += new EventHandler(PrintDoc);

        rightPanel.Controls.Add(printDocumentButton);


        Button publishButton = new Button();
        publishButton.SetBounds(0, 300, 160, 30);
        publishButton.Text = "Publish PDF Recipe Book";
        publishButton.Click += new EventHandler(PrintDocAll);

        leftPanel.Controls.Add(publishButton);

        
        mainForm.Hide();

        mainForm.Show();
    }

    [DllImport("winspool.drv",
               CharSet = CharSet.Auto,
               SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern Boolean SetDefaultPrinter(String name);

    public void PrintDoc(object sender, EventArgs e)
    {
        stringToPrint = recipes[selectedIndex].value;

        PrintDocument pd = new PrintDocument();

        pd.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

        SetDefaultPrinter("Microsoft Print to PDF");
        
        PrintDialog MyPrintDialog = new PrintDialog();

        if (MyPrintDialog.ShowDialog() == DialogResult.OK)
        {
            pd.Print();
        }
    }

    private string stringToPrint = "";

    public void PrintDocAll(object sender, EventArgs e)
    {
        stringToPrint = "Recipe Book" + Environment.NewLine + "___________" + Environment.NewLine;
        
        for (int i = 0; i < recipes.Count; i++)
        {
            if (i < recipes.Count - 1)
            {
                stringToPrint += recipes[i].key + Environment.NewLine + Environment.NewLine + recipes[i].value + Environment.NewLine + Environment.NewLine;
            }
            else if (i == recipes.Count - 1)
            {
                stringToPrint += recipes[i].key + Environment.NewLine + Environment.NewLine + recipes[i].value;
            }
        }

        PrintDocument pd = new PrintDocument();

        pd.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

        SetDefaultPrinter("Microsoft Print to PDF");

        PrintDialog MyPrintDialog = new PrintDialog();

        if (MyPrintDialog.ShowDialog() == DialogResult.OK)
        {
            pd.Print();
        }
    }

    private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        Font font = new Font("Arial", 12);
        int charactersOnPage = 0;
        int linesPerPage = 0;
        // Sets the value of charactersOnPage to the number of characters 
        // of stringToPrint that will fit within the bounds of the page.
        e.Graphics.MeasureString(stringToPrint, font,
            e.MarginBounds.Size, StringFormat.GenericTypographic,
            out charactersOnPage, out linesPerPage);

        // Draws the string within the bounds of the page
        e.Graphics.DrawString(stringToPrint, font, Brushes.Black,
            e.MarginBounds, StringFormat.GenericTypographic);

        // Remove the portion of the string that has been printed.
        stringToPrint = stringToPrint.Substring(charactersOnPage);

        // Check to see if more pages are to be printed.
        e.HasMorePages = (stringToPrint.Length > 0);
    }
    
    private void EditCurrentRecipe(object sender, EventArgs e)
    {

        NewRecipe(sender, null);

        tb1.Text = recipes[selectedIndex].key;

        tb.Text = recipes[selectedIndex].value;

    }

    private void DeleteCurrentRecipe(object sender, EventArgs e)
    {
        recipes.RemoveAt(selectedIndex);
        recipeList.Items.Remove(recipeList.Items[selectedIndex]);

        UpdateDatabase(null, null);

        selectedIndex = -1;
        recipeList.SelectedIndex = -1;

        DisplayRecipe(null, null);

        mainForm.Hide();
        mainForm.Show();
    }

    private void MoveTextUp(object sender, EventArgs e)
    {
        theContent.SetBounds(0, theContent.Top - 70, 600, 6000);

        rightPanel.Hide();

        rightPanel.Show();
    }

    private void MoveTextDown(object sender, EventArgs e)
    {
        theContent.SetBounds(0, theContent.Top + 70, 600, 6000);

        rightPanel.Hide();

        rightPanel.Show();
    }

    private void AddList()
    {
        OpenRecipesFile(null, null);

        Button neew = new Button();
        neew.Text = "Make Recipe";
        neew.SetBounds(0, 0, 86, 20);
        neew.Click += new EventHandler(NewRecipe);

        leftPanel.Controls.Add(neew);

        selectRecipe.SetBounds(0, 70, 80, 20);
        selectRecipe.Text = "Select Recipe";
        selectRecipe.Click += new EventHandler(DisplayRecipe);

        leftPanel.Controls.Add(selectRecipe);

        recipeList.SetBounds(0, 100, 250, 500);

        for (int i = 0; i < recipes.Count; i++)
        {
            recipeList.Items.Add(recipes[i].key);
        }

        leftPanel.Controls.Add(recipeList);

        recipeList.SelectedIndexChanged += new EventHandler(DisplayRecipe);

        recipeList.SetBounds(0, 100, 200, 500);
        leftPanel.AutoScroll = true;
    }

    private void OpenRecipesFile(object sender, EventArgs e)
    {
        recipes = serializer.Deserialize();
        for (int i = 0; i < recipes.Count; i++)
        {
            recipes[i].value = recipes[i].value.Replace("\n", Environment.NewLine);
        }
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

        tb1.ForeColor = Color.Gray;
        tb.ForeColor = Color.Gray;
        tb1.BackColor = Color.Azure;
        tb.BackColor = Color.Aquamarine;

        Button saveOrUpdate = new Button();
        saveOrUpdate.SetBounds(300, 420, 50, 20);
        saveOrUpdate.Text = "SAVE";
        if (sender != null)
        {
            if (((Button)sender).Text.Equals("Edit"))
            {
                saveOrUpdate.Text = "EDIT";
                saveOrUpdate.Click += new EventHandler(EditDatabase);
            }
            else
            {
                saveOrUpdate.Click += new EventHandler(UpdateDatabase);
            }
        }

        rightPanel.Controls.Add(saveOrUpdate);

        rightPanel.Controls.Add(tb1);

        rightPanel.Controls.Add(tb);
    }

    private void EditDatabase(object sender, EventArgs e)
    {
        recipeList.Items.Clear();

        if (sender != null)
        {
            recipes[selectedIndex].key = tb1.Text;
            recipes[selectedIndex].value = tb.Text;
        }

        for (int i = 0; i < recipes.Count; i++)
        {
            recipeList.Items.Add(recipes[i].key);
        }

        serializer.Serialize(recipes);

        mainForm.Hide();

        mainForm.Show();

        recipeList.SelectedIndex = selectedIndex;

        DisplayRecipe(sender, null);
    }

    private void UpdateDatabase(object sender, EventArgs e)
    {
        recipeList.Items.Clear();

        if (sender != null)
        {
            recipes.Add(new Item(tb1.Text, tb.Text));
        }

        for (int i = 0; i < recipes.Count; i++)
        {
            recipeList.Items.Add(recipes[i].key);
        }

        serializer.Serialize(recipes);

        mainForm.Hide();

        mainForm.Show();

        selectedIndex = 0;// recipes.Count - 1;

        recipeList.SelectedIndex = selectedIndex;

        DisplayRecipe(sender, null);
    }

    private void DisplayRecipe(object sender, EventArgs e)
    {

        selectedIndex = recipeList.SelectedIndex;


        if (sender == selectRecipe)
        {
            selectedIndex = -1;
            recipeList.SelectedIndex = -1;
        }


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


            if (selectedIndex > -1 && selectedIndex < recipeList.Items.Count && selectedIndex < recipes.Count)
            {
                dynamicTextBox.Text = recipeList.Items[selectedIndex] + "\n\n" + recipes[selectedIndex].value;
            }

            Button delBtn = new Button();
            delBtn.Text = "Delete";
            delBtn.SetBounds(600, 370, 60, 20);
            delBtn.Click += new EventHandler(DeleteCurrentRecipe);
            rightPanel.Controls.Add(delBtn);

            theContent = dynamicTextBox;

            return theContent;

        }
        else
        {
            Label dynamicTextBox = new Label();

            dynamicTextBox.BackColor = Color.Transparent;

            dynamicTextBox.ForeColor = Color.Black;

            dynamicTextBox.Font = new Font("Georgia", 36);

            dynamicTextBox.SetBounds(0, 0, 600, 6000);

            dynamicTextBox.Text = "Select Recipe From Left Side";

            theContent = dynamicTextBox;

            Panel selectRecipePanel = new Panel();

            selectRecipePanel.SetBounds(100, 100, 400, 400);

            Size size = new Size(400, 400);

            selectRecipePanel.BackgroundImage = Image.FromFile(@"splash.jpg");

            Bitmap bmp = new Bitmap(selectRecipePanel.BackgroundImage, size);

            selectRecipePanel.BackgroundImage = bmp;

            rightPanel.Controls.Add(selectRecipePanel);

            return theContent;
        }
    }

    public static void Main()
    {
        Application.EnableVisualStyles();

        RecipeBook recipeBook = new RecipeBook();

        recipeBook.splashForm.Show();

        new System.Threading.ManualResetEvent(false).WaitOne(1250);

        recipeBook.splashForm.Close();

        recipeBook.ShowBook();

        recipeBook.mainForm.Show();

        Application.Run(recipeBook.mainForm);
    }
}