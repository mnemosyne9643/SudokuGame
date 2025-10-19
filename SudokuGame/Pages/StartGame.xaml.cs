namespace SudokuGame.Pages;

public partial class StartGame : ContentPage
{
    GamePlay easy_game;
    GamePlay medium_game;
    GamePlay hard_game;

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
        DifficultySlider.Value = level;

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
                // generate one
                if (easy_game is null) easy_game = new GamePlay();

                await Navigation.PushAsync(easy_game);
                easy_game.GenerateBoard(Random.Shared.Next(24, 27));
                break;
            case "MEDIUM":
                // generate one
                if (medium_game is null) medium_game = new GamePlay();

                await Navigation.PushAsync(medium_game);
                medium_game.GenerateBoard(Random.Shared.Next(30, 36));
                break;
            case "HARD":
                // generate one
                if (hard_game is null) hard_game = new GamePlay();

                await Navigation.PushAsync(hard_game);
                hard_game.GenerateBoard(Random.Shared.Next(42, 50));
                break;
        }
    }
}