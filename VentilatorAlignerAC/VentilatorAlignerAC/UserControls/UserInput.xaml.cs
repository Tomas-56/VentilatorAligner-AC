

using System.ComponentModel;

namespace VentilatorAlignerAC.UserControls;

public partial class UserInput :  ContentView
{
    
    private string textBlockText;

    public string TextBlockText
    {
        get { return textBlockText; }
        set { textBlockText = value; UserInputLable.Text = textBlockText; }
    }




    public string input;

    public UserInput()
	{
        
        
		InitializeComponent();

        
        

    }

    private void UserInputEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        input = UserInputEntry.Text;
    }
}