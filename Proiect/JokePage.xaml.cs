
using Microsoft.Maui.Controls;
using SQLite;
using System.Collections.Generic;
using Proiect.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

public class JokePage : ContentPage
{
    private ListView jokeListView;

    public JokePage()
    {
        jokeListView = new ListView();
        jokeListView.ItemTemplate = new DataTemplate(typeof(JokeCell));

        
        List<Joke> jokes = RetrieveJokesFromDatabase();

       
        jokeListView.ItemsSource = jokes;

        Content = jokeListView;
    }


    private void RemoveJokeFromDatabase(Joke joke)
    {

        const string name = "Jokes.db";
        var path = Path.Combine(FileSystem.AppDataDirectory, name);

        using (SQLiteConnection connection = new SQLiteConnection(path))
        {
            connection.Delete(joke);
        }
    }

    public void DeleteJoke(Joke joke)
    {
        // Remove the joke from the database
        RemoveJokeFromDatabase(joke);

        // Refresh the list by retrieving the updated jokes and setting them as the ItemsSource
        List<Joke> updatedJokes = RetrieveJokesFromDatabase();
        jokeListView.ItemsSource = updatedJokes;


           
         var currentPage = new JokePage();

        // Update the BindingContext to trigger a refresh
        currentPage.BindingContext = null;
        currentPage.BindingContext = this;
    }

    private List<Joke> RetrieveJokesFromDatabase()
    {
       
        const string name = "Jokes.db";
        var path = Path.Combine(FileSystem.AppDataDirectory, name);

        using (SQLiteConnection connection = new SQLiteConnection(path))
        {
            // Assuming you have a "jokes" table in your database
            List<Joke> jokes = connection.Table<Joke>().ToList();
            return jokes;
        }
    }
}

public class JokeCell : ViewCell
{

    private Label setupLabel;
    private Label punchlineLabel;

    public JokeCell()
    {
        StackLayout mainLayout = new StackLayout();
        mainLayout.Orientation = StackOrientation.Vertical;
        mainLayout.Spacing = 10; // Add spacing between the jokeLayout and deleteButton

        StackLayout jokeLayout = new StackLayout();
        jokeLayout.Orientation = StackOrientation.Vertical;
        jokeLayout.Spacing = 5; // Add spacing between the setupLabel and punchlineLabel

        Label setupLabel = new Label();
        setupLabel.SetBinding(Label.TextProperty, new Binding("setup"));
        setupLabel.Margin = new Thickness(0, 0, 0, 5); // Add margin at the bottom of the setupLabel

        Label punchlineLabel = new Label();
        punchlineLabel.SetBinding(Label.TextProperty, new Binding("punchline"));

        var deleteGestureRecognizer = new TapGestureRecognizer();
        deleteGestureRecognizer.NumberOfTapsRequired = 2; // Detect double-tap
        deleteGestureRecognizer.Tapped += DeleteButton_Clicked;

        mainLayout.Children.Add(jokeLayout);
        

        jokeLayout.Children.Add(setupLabel);
        jokeLayout.Children.Add(punchlineLabel);
        jokeLayout.GestureRecognizers.Add(deleteGestureRecognizer);

        View = mainLayout;


    }

    public void DeleteJoke(Joke joke)
    {
        var jokePage = Parent as JokePage;
        jokePage?.DeleteJoke(joke);
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var joke = BindingContext as Joke;
        if (joke != null)
        {
            
            RemoveJokeFromDatabase(joke);
            RefreshJokeList();
        }
    }

    private void RefreshJokeList()
    {
        var jokePage = Parent as ContentPage;
        var jokeListView = jokePage?.Content as ListView;

        if (jokeListView != null)
        {
            // Retrieve jokes from the database
            List<Joke> jokes = RetrieveJokesFromDatabase();

            // Set the jokes as the ItemsSource of the ListView
            jokeListView.ItemsSource = jokes.ToList();
        }

    }

    private List<Joke> RetrieveJokesFromDatabase()
    {

        const string name = "Jokes.db";
        var path = Path.Combine(FileSystem.AppDataDirectory, name);

        using (SQLiteConnection connection = new SQLiteConnection(path))
        {
            // Assuming you have a "jokes" table in your database
            List<Joke> jokes = connection.Table<Joke>().ToList();
            return jokes;
        }
    }

    private void RemoveJokeFromDatabase(Joke joke)
    {

        const string name = "Jokes.db";
        var path = Path.Combine(FileSystem.AppDataDirectory, name);

        using (SQLiteConnection connection = new SQLiteConnection(path))
        {
            connection.Delete(joke);
        }
    }

}