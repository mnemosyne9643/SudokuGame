namespace SudokuGame.Pages;

public partial class MediumPart : ContentPage
{
	public MediumPart()
	{
		InitializeComponent();
        CreateSudokuGrid();
    }
    private void CreateSudokuGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            SudokuGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            SudokuGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        // Create 81 cells dynamically
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                var border = new Border
                {
                    Stroke = Color.FromArgb("#415A77"),
                    StrokeThickness = 1.5,
                    BackgroundColor = Color.FromArgb("#0D1B2A"),
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
                };

                SudokuGrid.Add(border, col, row);
            }
        }
    }
}
