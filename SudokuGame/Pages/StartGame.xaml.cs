namespace SudokuGame.Pages;

public partial class StartGame : ContentPage
{
    private readonly string[] difficulties = { "EASY", "MEDIUM", "HARD" };
    private readonly string[] colors = { "#00FF66", "#FFD700", "#FF4444" };
    private readonly string[] images =
    {
        "easy_emoji.png",
        "medium_emoji.png",
        "hard_emoji.png"
    };
    public StartGame()
	{
		InitializeComponent();
        UpdateDifficulty(0);
    }
    private void OnDifficultyChanged(object sender, ValueChangedEventArgs e)
    {
        int level = (int)Math.Round(e.NewValue);
        UpdateDifficulty(level);
    }

    private void UpdateDifficulty(int level)
    {
        DifficultyLabel.Text = difficulties[level];
        DifficultyLabel.TextColor = Color.FromArgb(colors[level]);
        DifficultyImage.Source = images[level];
    }

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        string selectedDifficulty = DifficultyLabel.Text;

        switch (selectedDifficulty)
        {
            case "EASY":
                await Navigation.PushAsync(new EasyPart());
                break;
            case "MEDIUM":
                await Navigation.PushAsync(new MediumPart());
                break;
            case "HARD":
                await Navigation.PushAsync(new HardPart());
                break;

        }
    }
}