using System;
using System.Windows.Forms;
using System.Drawing;

public class AddSearch
{
    private const int PANEL_SEARCH_X = 0;
    private const int PANEL_SEARCH_Y = 0;
    private const int PANEL_SEARCH_WIDTH = 210;
    private const int PANEL_SEARCH_HEIGHT = 70;
    
    private const int PANEL_RETRIEVE_X = 50;
    private const int PANEL_RETRIEVE_Y = 10;
    private const int PANEL_RETRIEVE_WIDTH = 550;
    private const int PANEL_RETRIEVE_HEIGHT = 550;

    private Panel panelForSearch;
    private Panel panelToRetrieve;

    private TreeView searchView;

    private LList recipes;

    private TextBox searchBox = new TextBox();

    private Button searchMe = new Button();

    private Displayer displayer;

    public AddSearch(LList therecipes, Displayer thedisplayer)
    {
        searchView = new TreeView();
        recipes = therecipes;
        displayer = thedisplayer;
        SetPanels();
    }

    private void SetPanels()
    {
        panelForSearch = new Panel();
        panelForSearch.SetBounds(AddSearch.PANEL_SEARCH_X, AddSearch.PANEL_SEARCH_Y, AddSearch.PANEL_SEARCH_WIDTH, AddSearch.PANEL_SEARCH_HEIGHT);

        panelToRetrieve = new Panel();
        panelToRetrieve.SetBounds(AddSearch.PANEL_RETRIEVE_X, AddSearch.PANEL_RETRIEVE_Y, AddSearch.PANEL_RETRIEVE_WIDTH, AddSearch.PANEL_RETRIEVE_HEIGHT);

        panelForSearch.BackColor = Color.Transparent;
        panelToRetrieve.BackColor = Color.Transparent;
    }

    public void Setup()
    {
        searchBox.SetBounds(0, 40, 120, 40);

        searchMe.SetBounds(130, 40, 80, 40);

        searchBox.BackColor = Color.Cyan;
        searchBox.ForeColor = Color.White;

        searchMe.BackColor = Color.Violet;
        searchMe.ForeColor = Color.Brown;

        searchBox.Text = "";

        searchMe.Text = "Lookup";

        panelForSearch.Controls.Add(searchBox);

        panelForSearch.Controls.Add(searchMe);
    }

    public void SetEvents()
    {
        searchBox.KeyPress += new KeyPressEventHandler(KeyPressASearch);
        
        searchMe.Click += new EventHandler(PerformASearch);
        
        searchView.NodeMouseClick += new TreeNodeMouseClickEventHandler(OpenNode);
    }

    public Panel Search()
    {
        return panelForSearch;
    }

    public Panel Retrieve()
    {
        return panelToRetrieve;
    }

    private void KeyPressASearch(object sender, KeyPressEventArgs e)
    {
        if (searchBox.Text.Length > 0 && e.KeyChar == (char)13)
        {
            PerformASearch(sender, e);
        }
    }

    private void PerformASearch(object sender, EventArgs e)
    {
        if (searchBox.Text.Length > 0)
        {
            displayer.ClearRightPanel();

            displayer.DisplaySearchResults(Retrieve());

            panelToRetrieve.Controls.Clear();

            Label searchResults = new Label();
            searchResults.Text = "Search Results For: " + searchBox.Text;
            searchResults.ForeColor = Color.Black;
            searchResults.BackColor = Color.CornflowerBlue;
            searchResults.Font = new Font("Arial", 22, FontStyle.Underline);
            searchResults.SetBounds(0, 0, 600, 40);

            searchView.Nodes.Clear();

            searchView.SetBounds(50, 90, 500, 400);
            searchView.BackColor = Color.Chocolate;
            searchView.ForeColor = Color.Cyan;

            panelToRetrieve.Controls.Add(searchResults);
            panelToRetrieve.Controls.Add(searchView);

            for (int i = 0; i < recipes.Count; i++)
            {
                if (recipes[i].key.Contains(searchBox.Text))
                {
                    new System.Threading.ManualResetEvent(false).WaitOne(250);

                    TreeNode key = new TreeNode(recipes[i].key);
                    key.BackColor = Color.Yellow;

                    TreeNode value = new TreeNode(recipes[i].value);

                    TreeNode category = new TreeNode(recipes[i].cat);

                    TreeNode author = new TreeNode(recipes[i].author);

                    TreeNode[] treeNodeArray = new TreeNode[] { key, value, category, author };

                    TreeNode treeNode = new TreeNode(key.Text, treeNodeArray);

                    searchView.Nodes.Add(treeNode);
                }
                if (recipes[i].value.Contains(searchBox.Text))
                {
                    new System.Threading.ManualResetEvent(false).WaitOne(250);

                    TreeNode key = new TreeNode(recipes[i].key);

                    string val = recipes[i].value;
                    int inn = val.IndexOf(searchBox.Text);
                    string theval1 = val.Substring(inn, val.Length - inn);
                    string theval2 = val.Substring(0, inn);
                    if (theval1.Length < 50 || theval2.Length < 50)
                    {
                        val = recipes[i].value;
                    }
                    else
                    {
                        string t1 = theval2.Substring(theval2.Length - 50, 50);
                        string t2 = theval1.Substring(0, 50);
                        val = t1 + t2;
                        string tval = "{<{" + val.Substring(val.IndexOf(searchBox.Text), searchBox.Text.Length) + "}>}";
                        string ts = val.Substring(0, val.IndexOf(searchBox.Text)) + tval + val.Substring(val.IndexOf(searchBox.Text) + searchBox.Text.Length, val.Length - val.IndexOf(searchBox.Text) - searchBox.Text.Length);
                        val = ts;
                    }
                    TreeNode value = new TreeNode(val);
                    value.BackColor = Color.Yellow;

                    TreeNode category = new TreeNode(recipes[i].cat);

                    TreeNode author = new TreeNode(recipes[i].author);

                    TreeNode[] treeNodeArray = new TreeNode[] { key, value, category, author };

                    TreeNode treeNode = new TreeNode(key.Text, treeNodeArray);

                    searchView.Nodes.Add(treeNode);
                }
                if (recipes[i].cat.Contains(searchBox.Text))
                {
                    new System.Threading.ManualResetEvent(false).WaitOne(250);

                    TreeNode key = new TreeNode(recipes[i].key);

                    TreeNode value = new TreeNode(recipes[i].value);

                    TreeNode category = new TreeNode(recipes[i].cat);
                    category.BackColor = Color.Yellow;

                    TreeNode author = new TreeNode(recipes[i].author);

                    TreeNode[] treeNodeArray = new TreeNode[] { key, value, category, author };

                    TreeNode treeNode = new TreeNode(key.Text, treeNodeArray);

                    searchView.Nodes.Add(treeNode);
                }
                if (recipes[i].author.Contains(searchBox.Text))
                {
                    new System.Threading.ManualResetEvent(false).WaitOne(250);

                    TreeNode key = new TreeNode(recipes[i].key);

                    TreeNode value = new TreeNode(recipes[i].value);

                    TreeNode category = new TreeNode(recipes[i].cat);

                    TreeNode author = new TreeNode(recipes[i].author);
                    author.BackColor = Color.Yellow;

                    TreeNode[] treeNodeArray = new TreeNode[] { key, value, category, author };

                    TreeNode treeNode = new TreeNode(key.Text, treeNodeArray);

                    searchView.Nodes.Add(treeNode);
                }

                searchView.ExpandAll();
                searchView.Scrollable = true;
            }
        }
    }

    private void OpenNode(object sender, TreeNodeMouseClickEventArgs e)
    {
        for (int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i].key.Equals(e.Node.Text))
            {
                displayer.SetIndexOfSelection(i);
                displayer.AddButtons();
                displayer.DisplaySelectionOfRecipe();
                return;
            }
        }
    }

}

