using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

public class RecipeBook
{
    private Form mainForm = new Form();
    private Panel leftPanel = new Panel();
    private Panel rightPanel = new Panel();
    private ListBox recipeList = new ListBox();
    private Hashtable hashtable = new Hashtable();
    private Label theContent = new Label();
    private int selectedIndex = -1;

    public RecipeBook()
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
        selectedIndex = 1;
        recipeList.SelectedIndex = 1;
        rightPanel.Controls.Clear();
        rightPanel.Controls.Add(DisplaySelectedText());

        AddContentViewBg();
        AddButtons();
    }

    private void AddContentViewBg()
    {
        Size size = new Size(600, 600);
        rightPanel.BackgroundImage = Image.FromFile(@"cherries.jpg");

        Bitmap bmp = new Bitmap(rightPanel.BackgroundImage, size);
        rightPanel.BackgroundImage = bmp;
    }

    private void AddButtons()
    {
        Button upButton = new Button();
        upButton.Text = "Up";
        upButton.SetBounds(600, 100, 100, 50);
        rightPanel.Controls.Add(upButton);
        upButton.Click += new EventHandler(MoveTextUp);

        Button downButton = new Button();
        downButton.Text = "Down";
        downButton.SetBounds(600, 510, 100, 50);
        rightPanel.Controls.Add(downButton);
        downButton.Click += new EventHandler(MoveTextDown);
    }

    private void MoveTextUp(object sender, EventArgs e)
    {
        Label content=theContent;
        Console.WriteLine(content.Text);
        content.SetBounds(0, content.Top - 20, 600, 6000);
        rightPanel.Hide();
        rightPanel.Show();
    }

    private void MoveTextDown(object sender, EventArgs e)
    {
        Label content = theContent;
        Console.WriteLine(content.Text);
        content.SetBounds(0, content.Top + 20, 600, 6000);
        rightPanel.Hide();
        rightPanel.Show();
    }

    private void AddList()
    {
        hashtable["candy 1"] = "One";
        hashtable["candy2"] = "The thread 'vshost.LoadReference' (0x12c4) has exited with code 0 (0x0).\n'WindowsFormsApplication2.vshost.exe' (Managed (v4.0.30319)): Loaded 'C:\\Users\\Daniel Cho\\Documents\\Visual Studio 2012\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\bin\\Debug\\WindowsFormsApplication2.exe', Symbols loaded.\nThe thread 'vshost.RunParkingWindow' (0x192c) has exited with code 0 (0x0).\nThe thread '<No Name>' (0x1b0c) has exited with code 0 (0x0).\nThe program '[700] WindowsFormsApplication2.vshost.exe: Program Trace' has exited with code 0 (0x0).\nThe program '[700] WindowsFormsApplication2.vshost.exe: Managed (v4.0.30319)' has exited with code 0 (0x0).";
        hashtable["candy  3"] = "Thirteen";

        Button neew = new Button();
        neew.Text = "Make Recipe";
        neew.SetBounds(0, 0, 250, 100);
        leftPanel.Controls.Add(neew);
        neew.Click += new EventHandler(NewRecipe);

        recipeList.SetBounds(0, 100, 250, 500);
        recipeList.Items.Add("candy 1");
        recipeList.Items.Add("candy2");
        recipeList.Items.Add("candy  3");
        leftPanel.Controls.Add(recipeList);
        recipeList.Click += new EventHandler(DisplayRecipe);
    }

    private void NewRecipe(object sender, EventArgs e)
    {

        rightPanel.Controls.Clear();

        TextBox tb = new TextBox();
        tb.Multiline = true;
        tb.SetBounds(0, 0, 600, 400);

        Button saveOrUpdate = new Button();
        saveOrUpdate.Text = "Make Recipe";
        saveOrUpdate.SetBounds(300, 420, 200, 40);

        rightPanel.Controls.Add(saveOrUpdate);
        rightPanel.Controls.Add(tb);

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
            Label lbl = new Label();
            lbl.ForeColor = Color.AntiqueWhite;
            lbl.BackColor = Color.Transparent;
            lbl.SetBounds(0, 0, 600, 6000);
            lbl.Text = recipeList.Items[selectedIndex] + "\n\n" + hashtable[recipeList.Items[selectedIndex]];
            lbl.Font = new Font("arial", 20);

            theContent = lbl;

            return theContent;
        }
        else
        {
            Label lbl = new Label();
            lbl.BackColor = Color.Transparent;

            theContent = lbl;

            return theContent;
        }
    }

    public static void Main()
    {
        RecipeBook recipeBook = new RecipeBook();
        Application.Run(recipeBook.mainForm);
    }
}