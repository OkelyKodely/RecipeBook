using System.Windows.Forms;

public class Displayer
{
    private RecipeBook rb;

    public Displayer(RecipeBook recipeBook)
    {
        rb = recipeBook;
    }

    public void SetIndexOfSelection(int i)
    {
        rb.SetSelectionIndex(i);
    }

    public void AddButtons()
    {
        rb.RightPanelClear();
        rb.AddButtons();
    }

    public void ClearRightPanel()
    {
        rb.RightPanel().Controls.Clear();
    }

    public void DisplaySearchResults(Panel panelToDisplay)
    {
        rb.RightPanel().Controls.Add(panelToDisplay);
    }

    public void DisplaySelectionOfRecipe()
    {
        rb.DisplaySelectionOfRecipe();
    }
}