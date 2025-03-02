


using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using VentilatorAlignerAC.UserControls;

namespace VentilatorAlignerAC;

public partial class CalculationView : ContentPage
{
    private double rotationOut;

    public double RotationOut
    {
        get { return rotationOut; }
        set {
            rotationOut = value;  Indicator.Rotation = RotationOut;
           
        }
    }

  


    string MtValue;
    string V0Value;
    string V1Value; 
    string V2Value;
    string V3Value;

    bool canCalculate = false;
    
    
    public CalculationView()
	{
        
        //MtValue = Convert.ToDouble( UserInputMt.UserInputEntryText);

        UserInputMt = new UserInput();
        
        
        
        

        InitializeComponent();

        
        

    }

    private void Vypocet_Btn_Click(object sender, EventArgs e)
    {
        MtValue = UserInputMt.input;
        V0Value = UserInputV0.input;
        V1Value = UserInputV1.input;
        V2Value = UserInputV2.input;
        V3Value = UserInputV3.input;

        string[] vlaueArray = { MtValue, V0Value, V1Value, V2Value, V3Value };

        foreach (var v in vlaueArray) 
        {
            if (string.IsNullOrEmpty(v)) { canCalculate = false; break; }
            else { canCalculate = true; }
        }

        if (canCalculate)
        {
            CalculationSheetModel calculationSheet = new CalculationSheetModel(Convert.ToDouble(MtValue), Convert.ToDouble(V0Value), Convert.ToDouble(V1Value), Convert.ToDouble(V2Value), Convert.ToDouble(V3Value));

            DCOHmotnost.OutputValueText = calculationSheet.hmotnostZavazia.ToString();
            DCOUhol.OutputValueText = calculationSheet.uholPoziciaVyvazku_a.ToString();

            RotationOut = -calculationSheet.uholPoziciaVyvazku_a;
        }
        else { DisplayAlert("Input Error", "Nie je moûnÈ vypoËÌtaù v˝sledok, jeden alebo viac vstupov ch˝ba","OK"); }
       canCalculate = false;


    }
    private void Reset_Btn_Click(object sender, EventArgs e)
    {

        


    }

   
}


