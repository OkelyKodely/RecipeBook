using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing.Printing;

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
    
    private ComboBox printList = new ComboBox();
    
    private Serializer serializer = new Serializer();

    private Button upButton = new Button();

    private Button downButton = new Button();

    private Button printButton = new Button();

    private Button printAllButton = new Button();

    private Button printPdfButton = new Button();

    private Form splashForm = new Form();
    
    private PrintManager printManager = null;

    private PrintManager printAllManager = null;

    private PrintManager printPdfManager = null;    
    
    private Label selectRecipe = new Label();

    public RecipeBook()
    {
        printManager = new PrintManager(printButton, printList, theContent.Text);

        ComboBox pl1 = new ComboBox();
        ComboBox pl2 = new ComboBox();
        string allContent = "";
        OpenRecipesFile(null, null);
        
        for (int i = 0; i < recipes.Count; i++)
        {
            allContent += "Recipe Book___________\n\n" + recipes[i].key + "\n\n" + recipes[i].value + "\n\n\n";
        }
        printAllManager = new PrintManager(printAllButton, pl1, allContent);
        printPdfManager = new PrintManager(printPdfButton, pl2, allContent);
        pl2.Items.Clear();
        pl2.Items.Add("Microsoft Print To PDF");
        pl1.SetBounds(0, 250, 200, 20);
        pl2.SetBounds(0, 400, 200, 20);

        leftPanel.Controls.Add(pl1);
        leftPanel.Controls.Add(pl2);

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
        printAllButton.SetBounds(0, 300, 80, 40);
        printAllButton.Text = "Print All";
        printAllButton.ForeColor = Color.Black;
        printAllButton.BackColor = Color.Transparent;

        leftPanel.Controls.Add(printAllButton);

        printPdfButton.SetBounds(0, 450, 80, 40);
        printPdfButton.Text = "Print Pdf";
        printPdfButton.ForeColor = Color.Black;
        printPdfButton.BackColor = Color.Transparent;

        leftPanel.Controls.Add(printPdfButton);

        
        upButton.Text = "Up";
        upButton.SetBounds(604, 170, 44, 24);
        upButton.Click += new EventHandler(MoveTextDown);

        rightPanel.Controls.Add(upButton);

        downButton.Text = "Down";
        downButton.SetBounds(650, 170, 44, 24);
        downButton.Click += new EventHandler(MoveTextUp);

        rightPanel.Controls.Add(downButton);
        
        printButton.SetBounds(600, 100, 80, 40);
        printButton.Text = "Print";
        printButton.ForeColor = Color.Black;
        printButton.BackColor = Color.Transparent;
        
        rightPanel.Controls.Add(printButton);

        Button editButton = new Button();
        editButton.SetBounds(600, 300, 80, 40);
        editButton.Text = "Edit";
        editButton.Click += new EventHandler(EditCurrentRecipe);

        rightPanel.Controls.Add(editButton);


        
        rightPanel.Controls.Add(printList);
        

        
        mainForm.Hide();
        
        mainForm.Show();
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
        ///recipeList.ScrollAlwaysVisible = true;

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

        selectedIndex = recipes.Count - 1;

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

            
            if (selectedIndex > -1 && selectedIndex < recipeList.Items.Count)
            {
                dynamicTextBox.Text = recipeList.Items[selectedIndex] + "\n\n" + recipes[selectedIndex].value;
            }


            Button delBtn = new Button();
            delBtn.Text = "Delete";
            delBtn.SetBounds(600, 370, 60, 20);
            delBtn.Click += new EventHandler(DeleteCurrentRecipe);
            rightPanel.Controls.Add(delBtn);

            theContent = dynamicTextBox;

            return dynamicTextBox;

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
        RecipeBook recipeBook = new RecipeBook();
        
        recipeBook.splashForm.Show();
        
        new System.Threading.ManualResetEvent(false).WaitOne(1250);
        
        recipeBook.splashForm.Close();

        recipeBook.ShowBook();
        
        recipeBook.mainForm.Show();
        
        Application.Run(recipeBook.mainForm);
    }
}