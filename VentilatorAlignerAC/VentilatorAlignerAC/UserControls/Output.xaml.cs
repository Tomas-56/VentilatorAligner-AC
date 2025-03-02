namespace VentilatorAlignerAC.UserControls;

public partial class Output : ContentView
{
    private string outputDescriptionText;

    public string OutputDescriptionText
    {
        get { return outputDescriptionText; }
        set { outputDescriptionText = value; OutputDescription.Text = outputDescriptionText; }
    }

    private string outputValuetext;

    public string OutputValueText
    {
        get { return outputValuetext; }
        set { outputValuetext = value; OutputValue.Text = outputValuetext; }
    }

    private string unitText;

    public string UnitText
    {
        get { return unitText; }
        set { unitText = value; Unit.Text = unitText; }
    }
    public Output()
	{



		InitializeComponent();
	}
}