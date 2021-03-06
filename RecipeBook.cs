﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Drawing.Printing;
using Workaholic.RTFEditor;

public class RecipeBook
{


    private AddSearch searchManager;

    private Displayer displayer;
    
    private Button neew = new Button();

    private Form mainForm = new Form();

    private Panel leftPanel = new Panel();

    private Panel rightPanel = new Panel();

    private ComboBox recipeList = new ComboBox();

    private TextBox tb1 = new TextBox();

    private RTFEditor tb = new RTFEditor();

    private ComboBox tb2 = new ComboBox();

    private TextBox author = new TextBox();

    private LList recipes = new LList();

    private Label theContent = new Label();

    private Serializer serializer = new Serializer();

    private Button upButton = new Button();

    private Button downButton = new Button();

    private Form splashForm = new Form();

    private Label selectRecipe = new Label();

    private string stringToPrint = "";

    private int selectedIndex = -1;

    private string category = "";



    public RecipeBook()
    {
        displayer = new Displayer(this);

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

    public void SetSelectionIndex(int i)
    {
        selectedIndex = i;
    }

    public void DisplaySelectionOfRecipe()
    {
        rightPanel.Controls.Add(DisplaySelectedText());
    }

    public void ShowMyBook()
    {
        mainForm.SetBounds(0, 0, 1000, 600);

        mainForm.Controls.Add(leftPanel);
        mainForm.Controls.Add(rightPanel);

        mainForm.Text = "RecipeBook";

        mainForm.FormBorderStyle = FormBorderStyle.FixedSingle;
        mainForm.MaximizeBox = false;

        leftPanel.SetBounds(0, 0, 250, 600);
        rightPanel.SetBounds(250, 0, 750, 600);

        AddList();

        rightPanel.Controls.Clear();
        rightPanel.Controls.Add(DisplaySelectedText());

        AddContentViewBg();

        AddSearch();

        AddButtons();

        tb.MenuVisible = false;
    }

    private void AddSearch()
    {
        searchManager = new AddSearch(recipes, displayer);

        searchManager.Setup();
        
        searchManager.SetEvents();
        
        leftPanel.Controls.Add(searchManager.Search());
    }

    public Panel RightPanel()
    {
        return rightPanel;
    }

    public Panel LeftPanel()
    {
        return leftPanel;
    }

    private void AddContentViewBg()
    {
        Size size = new Size(600, 600);
        
        rightPanel.BackgroundImage = Image.FromFile(@"bg.png");
        
        Bitmap bmp = new Bitmap(rightPanel.BackgroundImage, size);
        
        rightPanel.BackgroundImage = bmp;
    }

    public void LeftPanelClear()
    {
        leftPanel.Controls.Clear();
    }

    public void RightPanelClear()
    {
        rightPanel.Controls.Clear();
    }

    public void AddButtons()
    {
        upButton.Text = "Up";
        upButton.SetBounds(604, 170, 44, 24);
        upButton.ForeColor = Color.Red;
        upButton.BackColor = Color.Wheat;
        upButton.MouseDown += new MouseEventHandler(MoveTextDown);
        RightPanel().Controls.Add(upButton);

        downButton.Text = "Down";
        downButton.SetBounds(650, 170, 44, 24);
        downButton.ForeColor = Color.Red;
        downButton.BackColor = Color.Wheat;
        downButton.MouseDown += new MouseEventHandler(MoveTextUp);
        RightPanel().Controls.Add(downButton);

        Button editButton = new Button();
        editButton.SetBounds(600, 300, 80, 40);
        editButton.Text = "Open";
        editButton.ForeColor = Color.SlateGray;
        editButton.BackColor = Color.White;
        editButton.Click += new EventHandler(EditCurrentRecipe);
        RightPanel().Controls.Add(editButton);

        Button printDocumentButton = new Button();
        printDocumentButton.SetBounds(600, 20, 120, 30);
        printDocumentButton.Text = "Print THIS.";
        printDocumentButton.Click += new EventHandler(PrintDoc);
        RightPanel().Controls.Add(printDocumentButton);

        Button publishButton = new Button();
        publishButton.SetBounds(50, 510, 130, 20);
        publishButton.Text = "Print Everything";
        publishButton.ForeColor = Color.Blue;
        publishButton.Click += new EventHandler(PrintDocAll);
        LeftPanel().Controls.Add(publishButton);

        /* Update the screen shot ***/
        mainForm.Update();
    }

    public void PrintDoc(object sender, EventArgs e)
    {
        stringToPrint = recipes[selectedIndex].value;

        PrintDocument pd = new PrintDocument();

        pd.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

        PrintDialog MyPrintDialog = new PrintDialog();

        if (MyPrintDialog.ShowDialog() == DialogResult.OK)
        {
            pd.Print();
        }
    }

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

        e.Graphics.MeasureString(stringToPrint, font,
            e.MarginBounds.Size, StringFormat.GenericTypographic,
            out charactersOnPage, out linesPerPage);

        e.Graphics.DrawString(stringToPrint, font, Brushes.Black,
            e.MarginBounds, StringFormat.GenericTypographic);

        stringToPrint = stringToPrint.Substring(charactersOnPage);

        e.HasMorePages = (stringToPrint.Length > 0);
    }
    
    private void EditCurrentRecipe(object sender, EventArgs e)
    {
        NewRecipe(sender, null);

        tb1.Text = recipes[selectedIndex].key;

        tb.DocumentText = recipes[selectedIndex].value;

        tb.DocumentRtf = recipes[selectedIndex].src;

    }

    private void DeleteCurrentRecipe(object sender, EventArgs e)
    {
        recipes.RemoveAt(selectedIndex);
        recipeList.Items.Remove(recipeList.Items[selectedIndex]);

        UpdateDatabase(null, null);

        selectedIndex = -1;
        recipeList.SelectedIndex = -1;

        DisplayRecipe(null, null);

        mainForm.Update();
    }

    private void MoveTextUp(object sender, EventArgs e)
    {
        theContent.SetBounds(0, theContent.Top - 10, 600, 6000);

        rightPanel.Update();
    }

    private void MoveTextDown(object sender, EventArgs e)
    {
        theContent.SetBounds(0, theContent.Top + 10, 600, 6000);

        rightPanel.Update();
    }

    private void AddList()
    {
        OpenRecipesFile(null, null);

        neew.Text = "Make Recipe";
        neew.SetBounds(0, 0, 86, 20);
        neew.Click += new EventHandler(NewRecipe);
        
        LeftPanel().Controls.Add(neew);

        selectRecipe.SetBounds(0, 80, 80, 20);
        selectRecipe.Text = "Select Recipe";
        selectRecipe.Font = new Font("Arial", 8, FontStyle.Underline);
        selectRecipe.Click += new EventHandler(DisplayRecipe);

        LeftPanel().Controls.Add(selectRecipe);

        recipeList.SetBounds(0, 100, 250, 500);

        for (int i = 0; i < recipes.Count; i++)
        {
            recipeList.Items.Add(recipes[i].key);
        }

        LeftPanel().Controls.Add(recipeList);

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
            recipes[i].src = recipes[i].src.Replace("\n", Environment.NewLine);
        }
        tb2.Items.Clear();
        for (int i = 0; i < recipes.Count; i++)
        {
            if (i + 1 >= recipes.Count && i == 0)
            {
                tb2.Items.Add(recipes[i].cat);
            }
            else if(i == 0)
            {
                tb2.Items.Add(recipes[i].cat);
            }
            else
            {
                bool same = false;
                for (int j = 0; j < i; j++)
                {
                    if (recipes[j].cat.Equals(recipes[i].cat))
                    {
                        same = true;
                    }
                }
                if (!same)
                {
                    tb2.Items.Add(recipes[i].cat);
                }
            }
        }
        tb2.SelectedIndexChanged += new EventHandler(NewCategory);
    }

    private void NewCategory(object sender, EventArgs e)
    {
        category = tb2.Items[tb2.SelectedIndex].ToString();

        newcaty = false;
    }

    private void NewRecipe(object sender, EventArgs e)
    {
        RightPanel().Controls.Clear();
        Label title = new Label();
        title.Text = "Title of recipe";
        title.BackColor = Color.Transparent;
        title.SetBounds(0, 0, 180, 25);
        RightPanel().Controls.Add(title);

        tb1.SetBounds(0, 25, 600, 40);

        Label cont = new Label();
        cont.Text = "Recipe";
        cont.BackColor = Color.Transparent;
        cont.SetBounds(0, 65, 80, 25);

        RightPanel().Controls.Add(cont);

        tb.FilePanelVisible = false;
        
        tb.SetBounds(0, 90, 600, 350);
        tb.DocumentText = "";
        tb.DocumentRtf = "";
        tb1.Text = "";

        if ((sender).Equals(neew))
        {
            author.Text = "";
        }

        Label cat = new Label();
        cat.Font = new Font("Arial", 10, FontStyle.Underline);
        cat.Text = "Category";
        cat.SetBounds(0, 45, 70, 20);
        tb2.SetBounds(80, 45, 100, 20);
        cat.BackColor = Color.Transparent;
        cat.Click += new EventHandler(FormCategory);

        Label authorr = new Label();
        authorr.Text = "Author";
        authorr.SetBounds(190, 45, 70, 20);
        author.SetBounds(260, 45, 120, 20);
        authorr.BackColor = Color.Transparent;

        Button saveOrUpdate = new Button();
        saveOrUpdate.SetBounds(300, 450, 50, 20);
        saveOrUpdate.Text = "SAVE";
        if (sender != null)
        {
            if (((Button)sender).Text.Equals("Open"))
            {
                saveOrUpdate.Text = "EDIT";
                saveOrUpdate.Click += new EventHandler(EditDatabase);
            }
            else
            {
                saveOrUpdate.Click += new EventHandler(UpdateDatabase);
            }
        }

        RightPanel().Controls.Add(authorr);

        RightPanel().Controls.Add(cat);

        RightPanel().Controls.Add(author);

        RightPanel().Controls.Add(tb2);

        RightPanel().Controls.Add(saveOrUpdate);

        RightPanel().Controls.Add(tb1);

        RightPanel().Controls.Add(tb);
    }

    Form form = new Form();
    TextBox neewcatt = new TextBox();
    bool newcaty = false;
    String newcateegorree = "";
    private void FormCategory(object sender, EventArgs e)
    {
        form = new Form();
        form.MaximizeBox = false;
        form.FormBorderStyle = FormBorderStyle.FixedSingle; 
        form.MinimizeBox = false;
        form.SetBounds(0, 0, 240, 100);
        form.Show();

        Label newcat = new Label();
        newcat.Text = "New Cat";
        newcat.SetBounds(0, 0, 60, 20);
        neewcatt.Text = "";
        neewcatt.SetBounds(70, 0, 70, 20);
        Button add = new Button();
        add.Text = "Add";
        add.SetBounds(0, 30, 50, 30);
        add.Click += new EventHandler(FormTheCategory);
        form.Controls.Add(newcat);
        form.Controls.Add(neewcatt);
        form.Controls.Add(add);
    }

    private void FormTheCategory(object sender, EventArgs e)
    {
        newcaty = true;
        newcateegorree = neewcatt.Text;
        form.Close();
    }

    private void EditDatabase(object sender, EventArgs e)
    {
        recipeList.Items.Clear();

        if (sender != null)
        {
            recipes[selectedIndex].key = tb1.Text;
            recipes[selectedIndex].value = tb.DocumentText;
            recipes[selectedIndex].src = tb.DocumentRtf;
            recipes[selectedIndex].author = author.Text;
            recipes[selectedIndex].cat = category;
            if (newcaty)
            {
                tb2.Items.Add(newcateegorree);
                recipes[selectedIndex].cat = newcateegorree;
            }
            else
            {
                newcaty = false;
            }
        }

        for (int i = 0; i < recipes.Count; i++)
        {
            recipeList.Items.Add(recipes[i].key);
        }

        serializer.Serialize(recipes);

        mainForm.Update();

        recipeList.SelectedIndex = selectedIndex;

        DisplayRecipe(sender, null);
    }

    private void UpdateDatabase(object sender, EventArgs e)
    {
        recipeList.Items.Clear();

        if (sender != null)
        {
            if (!newcaty)
            {
                recipes.Add(new Item(tb1.Text, tb.DocumentText, tb.DocumentRtf, author.Text, category));
                newcaty = false;
            }
            else
            {
                tb2.Items.Add(newcateegorree);
                recipes.Add(new Item(tb1.Text, tb.DocumentText, tb.DocumentRtf, author.Text, newcateegorree));
            }
        }

        for (int i = 0; i < recipes.Count; i++)
        {
            recipeList.Items.Add(recipes[i].key);
        }

        serializer.Serialize(recipes);

        mainForm.Update();

        selectedIndex = 0;

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

            dynamicTextBox.BackColor = Color.White;

            dynamicTextBox.BorderStyle = BorderStyle.FixedSingle;

            dynamicTextBox.ForeColor = Color.Black;

            dynamicTextBox.Font = new Font("Times New Roman", 15);

            dynamicTextBox.SetBounds(50, 0, 500, 6000);


            if (selectedIndex > -1 && selectedIndex < recipeList.Items.Count && selectedIndex < recipes.Count)
            {
                dynamicTextBox.Text = "Title: " + recipeList.Items[selectedIndex] + "\n\nCategory: " + recipes[selectedIndex].cat + "\n\nAuthor: " + recipes[selectedIndex].author + "\n\nRecipe: " + recipes[selectedIndex].value;
                category = recipes[selectedIndex].cat;
                for (int i = 0; i < tb2.Items.Count; i++)
                {
                    if (tb2.Items[i].ToString().Equals(recipes[selectedIndex].cat))
                    {
                        tb2.SelectedIndex = i;
                    }
                }

                author.Text = recipes[selectedIndex].author;
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

            dynamicTextBox.ForeColor = Color.Red;

            dynamicTextBox.Font = new Font("Times New Roman", 24, FontStyle.Italic);

            dynamicTextBox.SetBounds(0, 0, 600, 6000);

            dynamicTextBox.Text = "select Your Recipes from left";

            theContent = dynamicTextBox;

            Panel selectRecipePanel = new Panel();

            selectRecipePanel.SetBounds(0, 50, 600, 500);

            Size size = new Size(600, 500);

            selectRecipePanel.BackgroundImage = Image.FromFile(@"splash.jpg");

            Bitmap bmp = new Bitmap(selectRecipePanel.BackgroundImage, size);

            selectRecipePanel.BackgroundImage = bmp;

            rightPanel.Controls.Add(selectRecipePanel);

            return theContent;
        }
    }

    [STAThreadAttribute]
    public static void Main()
    {
        RecipeBook recipeBook = new RecipeBook();

        recipeBook.splashForm.Show();

        new System.Threading.ManualResetEvent(false).WaitOne(1250);

        recipeBook.splashForm.Close();

        recipeBook.ShowMyBook();
        recipeBook.mainForm.Show();

        Application.Run(recipeBook.mainForm);
    }
}