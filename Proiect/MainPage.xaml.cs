using Microsoft.VisualBasic;
using Proiect.Models;
using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
//using Windows.Media.Protection.PlayReady;

namespace Proiect;

public partial class MainPage : ContentPage
{
    int count = 0;
    Joke joke = new Joke();
    protected Models.Database Database = new();


    public MainPage()
	{
		InitializeComponent();
        this.BindingContext = this;
        var url = "https://official-joke-api.appspot.com/random_joke";
        var j = _download_serialized_json_data<Joke>(url);

        this.joke.type = j.type;
        this.joke.punchline = j.punchline;
        this.joke.setup = j.setup;
        this.joke.id = j.id;

        Binding jokeBinding = new Binding();
        jokeBinding.Source = this.joke;
        jokeBinding.Path = "setup";
        
        lbl1.SetBinding(Label.TextProperty, jokeBinding);

        Binding jokeBinding2 = new Binding();
        jokeBinding2.Source = this.joke;
        jokeBinding2.Path = "punchline";

        lbl2.SetBinding(Label.TextProperty, jokeBinding2);

        

    }

    private void ViewJokesButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new JokePage());
    }



    async void Button_Clicked_1(object sender, EventArgs e)
    {
        
        this.BindingContext = this;
        var url = "https://official-joke-api.appspot.com/random_joke";
        var j = _download_serialized_json_data<Joke>(url);

        this.joke.type = j.type;
        this.joke.punchline = j.punchline;
        this.joke.setup = j.setup;
        this.joke.id = j.id;

        Binding jokeBinding = new Binding();
        jokeBinding.Source = this.joke;
        jokeBinding.Path = "setup";

        lbl1.SetBinding(Label.TextProperty, jokeBinding);

        Binding jokeBinding2 = new Binding();
        jokeBinding2.Source = this.joke;
        jokeBinding2.Path = "punchline";

        lbl2.SetBinding(Label.TextProperty, jokeBinding2);


        IList<string> list = await Database.GetJokes();
        list.ToString();

    }


    async void Button_Clicked_2(object sender, EventArgs e)
    {
        
        await Database.SaveJokeAsync(this.joke);

    }

    async void Button_Clicked_3(object sender, EventArgs e)
    {

        await Shell.Current.GoToAsync("//jokesByType");

    }




    private static T _download_serialized_json_data<T>(string url) where T : new()
    {
        using (var w = new System.Net.WebClient())
        {
            var json_data = string.Empty;
             
            try
            {
                Console.WriteLine("Started downloading data");
                json_data = w.DownloadString(url);
                Console.WriteLine("Completed downloading");
            }
            catch (Exception)
            { }       
            return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
        }
    }

    
}

