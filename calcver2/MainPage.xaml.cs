using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Shapes;

namespace calcver2;

public partial class MainPage : ContentPage
{
    Entry inputEntry;
    Label resultLabel;

    string currentInput = "";
    double firstNumber = 0;
    string selectedOperator = "";
    bool isNewInput = false;

    public MainPage()
    {
        // InitializeComponent(); // אין צורך ב־XAML

        // Create UI in code
        inputEntry = new Entry
        {
            FontSize = 50,
            HorizontalTextAlignment = TextAlignment.End,
            IsReadOnly = true,
            Placeholder = "0"
        };

        resultLabel = new Label
        {
            FontSize = 40,
            HorizontalTextAlignment = TextAlignment.End,
            TextColor = Colors.Gray
        };

        // Create top display
        var displayStack = new VerticalStackLayout
        {
            Padding = 10,
            Children = { inputEntry, resultLabel }
        };

        var displayBorder = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0, 0, 25, 25) },
            BackgroundColor = Colors.LightBlue,
            Content = displayStack
        };

        // Create button grid
        var buttonGrid = new Grid
        {
            RowSpacing = 10,
            ColumnSpacing = 10
        };

        for (int i = 0; i < 5; i++)
            buttonGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        for (int i = 0; i < 4; i++)
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        // Define buttons
        AddButton(buttonGrid, "AC", 0, 0, OnClearClicked);
        AddButton(buttonGrid, "÷", 0, 1, OnOperatorClicked);
        AddButton(buttonGrid, "×", 0, 2, OnOperatorClicked);
        AddButton(buttonGrid, "←", 0, 3, OnBackspaceClicked);

        AddButton(buttonGrid, "7", 1, 0, OnNumberClicked);
        AddButton(buttonGrid, "8", 1, 1, OnNumberClicked);
        AddButton(buttonGrid, "9", 1, 2, OnNumberClicked);
        AddButton(buttonGrid, "−", 1, 3, OnOperatorClicked);

        AddButton(buttonGrid, "4", 2, 0, OnNumberClicked);
        AddButton(buttonGrid, "5", 2, 1, OnNumberClicked);
        AddButton(buttonGrid, "6", 2, 2, OnNumberClicked);
        AddButton(buttonGrid, "+", 2, 3, OnOperatorClicked);

        AddButton(buttonGrid, "1", 3, 0, OnNumberClicked);
        AddButton(buttonGrid, "2", 3, 1, OnNumberClicked);
        AddButton(buttonGrid, "3", 3, 2, OnNumberClicked);
        AddButton(buttonGrid, "=", 3, 3, OnEqualsClicked);

        AddButton(buttonGrid, "0", 4, 0, OnNumberClicked, columnSpan: 2);
        AddButton(buttonGrid, ".", 4, 2, OnNumberClicked);

        // Put everything in layout
        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            Children = { displayBorder, buttonGrid }
        };

        Content = new ScrollView { Content = layout };
    }

    private void AddButton(Grid grid, string text, int row, int column, EventHandler onClick, int columnSpan = 1)
    {
        var button = new Button
        {
            Text = text,
            FontSize = 32,
            FontAttributes = FontAttributes.Bold,
            WidthRequest = 80,
            HeightRequest = 80,
            CornerRadius = 40,
            BackgroundColor = Colors.LightSkyBlue,
            TextColor = Colors.Black
        };

        button.Clicked += onClick;
        Grid.SetRow(button, row);
        Grid.SetColumn(button, column);
        if (columnSpan > 1)
            Grid.SetColumnSpan(button, columnSpan);
        grid.Children.Add(button);
    }

    private void OnNumberClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string value = button.Text;

        if (isNewInput)
        {
            currentInput = "";
            isNewInput = false;
        }

        if (value == "." && currentInput.Contains("."))
            return;

        currentInput += value;
        inputEntry.Text = currentInput;
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string op = button.Text;

        if (double.TryParse(currentInput, out double number))
        {
            firstNumber = number;
            selectedOperator = op;
            isNewInput = true;
            resultLabel.Text = $"{currentInput} {selectedOperator}";
        }
    }

    private void OnEqualsClicked(object sender, EventArgs e)
    {
        if (double.TryParse(currentInput, out double secondNumber))
        {
            double result = 0;

            switch (selectedOperator)
            {
                case "+": result = firstNumber + secondNumber; break;
                case "−": result = firstNumber - secondNumber; break;
                case "×": result = firstNumber * secondNumber; break;
                case "÷":
                    if (secondNumber != 0)
                        result = firstNumber / secondNumber;
                    else
                    {
                        resultLabel.Text = "Error: Cannot divide by zero";
                        inputEntry.Text = "";
                        return;
                    }
                    break;
            }

            inputEntry.Text = result.ToString();
            resultLabel.Text = $"{firstNumber} {selectedOperator} {secondNumber} =";
            currentInput = result.ToString();
            isNewInput = true;
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        currentInput = "";
        firstNumber = 0;
        selectedOperator = "";
        isNewInput = false;
        inputEntry.Text = "";
        resultLabel.Text = "";
    }

    private void OnBackspaceClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(currentInput))
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            inputEntry.Text = currentInput;
        }
    }
}

