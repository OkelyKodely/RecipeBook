using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class RecipeBook
{
    private int selectedIndex = 0;

    private Form mainForm = new Form();
    
    private Panel leftPanel = new Panel();
    
    private Panel rightPanel = new Panel();
    
    private ListBox recipeList = new ListBox();
    
    private TextBox tb1 = new TextBox();
    
    private TextBox tb = new TextBox();
    
    private LList recipes = new LList();
    
    private Label theContent = new Label();
    
    private ComboBox printList = new ComboBox();
    
    private Serializer serializer = new Serializer();

    private Button upButton = new Button();

    private Button downButton = new Button();

    private RadioButton deleteButton = new RadioButton();

    private RadioButton printButton = new RadioButton();
    
    private Form splashForm = new Form();
    
    private PrintManager printManager = null;
    
    private Label selectRecipe = new Label();

    public RecipeBook()
    {
        printManager = new PrintManager(printButton, printList, theContent.Text);

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
        upButton.Text = "Up";
        upButton.SetBounds(604, 70, 44, 24);
        upButton.Click += new EventHandler(MoveTextDown);

        rightPanel.Controls.Add(upButton);

        downButton.Text = "Down";
        downButton.SetBounds(650, 70, 44, 24);
        downButton.Click += new EventHandler(MoveTextUp);

        rightPanel.Controls.Add(downButton);
        
        deleteButton.SetBounds(664, 0, 80, 40);
        deleteButton.Text = "Erase?";
        deleteButton.ForeColor = Color.Black;
        deleteButton.BackColor = Color.Transparent;
        deleteButton.Click += new EventHandler(DeleteCurrentRecipe);

        rightPanel.Controls.Add(deleteButton);
        
        printButton.SetBounds(600, 0, 80, 40);
        printButton.Text = "Print? or ";
        printButton.ForeColor = Color.Black;
        printButton.BackColor = Color.Transparent;
        
        rightPanel.Controls.Add(printButton);


        
        rightPanel.Controls.Add(printList);
        

        
        mainForm.Hide();
        
        mainForm.Show();
    }

    private void DeleteCurrentRecipe(object sender, EventArgs e)
    {
        recipes.Remove(recipes[recipeList.SelectedIndex]);
        recipeList.Items.Remove(recipeList.Items[recipeList.SelectedIndex]);
        
        UpdateDatabase(null, null);

        NewRecipe(null, null);
        
        mainForm.Hide();
        mainForm.Show();
    }

    private void MoveTextUp(object sender, EventArgs e)
    {
        theContent.SetBounds(0, theContent.Top- 70, 600, 6000);

        rightPanel.Hide();
        
        rightPanel.Show();
    }

    private void MoveTextDown(object sender, EventArgs e)
    {
        theContent.SetBounds(0, theContent.Top+ 70, 600, 6000);
        
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
        
        leftPanel.Controls.Add(selectRecipe);

        recipeList.SetBounds(0, 100, 250, 500);

        for (int i = 0; i < recipes.Count; i++)
        {
            recipeList.Items.Add(recipes[i].key);
        }

        leftPanel.Controls.Add(recipeList);
        
        recipeList.Click += new EventHandler(DisplayRecipe);
        recipeList.ScrollAlwaysVisible = true;

        recipeList.SetBounds(0, 100, 200, 500);
        leftPanel.AutoScroll = true;
    }

    private void OpenRecipesFile(object sender, EventArgs e)
    {
        recipes = serializer.Deserialize();
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
        saveOrUpdate.Click += new EventHandler(UpdateDatabase);


        
        rightPanel.Controls.Add(saveOrUpdate);
        
        rightPanel.Controls.Add(tb1);
        
        rightPanel.Controls.Add(tb);
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

            
            if (selectedIndex > -1 && selectedIndex < recipeList.Items.Count)
            {
                dynamicTextBox.Text = recipeList.Items[selectedIndex] + "\n\n" + recipes[selectedIndex].value;
            }

            
            theContent = dynamicTextBox;

            return dynamicTextBox;

        }
        else
        {
            Label nothingTextBox = new Label();
            
            theContent = nothingTextBox;
            
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