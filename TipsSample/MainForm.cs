using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AI.Fuzzy.Library;

namespace AI.Fuzzy.Samples.TipsSample
{
    public partial class MainForm : Form
    {
        MamdaniFuzzySystem _fsTips = null;

        public MainForm()
        {
            InitializeComponent();
        }

        MamdaniFuzzySystem CreateSystem()
        {
            //
            // Create empty fuzzy system
            //
            MamdaniFuzzySystem fsTips = new MamdaniFuzzySystem();

            //
            // Create input variables for the system
            //
            FuzzyVariable fvService = new FuzzyVariable("service", 0.0, 10.0);
            fvService.Terms.Add(new FuzzyTerm("poor", new TriangularMembershipFunction(-5.0, 0.0, 5.0)));
            fvService.Terms.Add(new FuzzyTerm("good", new TriangularMembershipFunction(0.0, 5.0, 10.0)));
            fvService.Terms.Add(new FuzzyTerm("excellent", new TriangularMembershipFunction(5.0, 10.0, 15.0)));
            fsTips.Input.Add(fvService);

            FuzzyVariable fvFood = new FuzzyVariable("food", 0.0, 10.0);
            fvFood.Terms.Add(new FuzzyTerm("rancid", new TrapezoidMembershipFunction(0.0, 0.0, 1.0, 3.0)));
            fvFood.Terms.Add(new FuzzyTerm("delicious", new TrapezoidMembershipFunction(7.0, 9.0, 10.0, 10.0)));
            fsTips.Input.Add(fvFood);

            //
            // Create output variables for the system
            //
            FuzzyVariable fvTips = new FuzzyVariable("tips", 0.0, 30.0);
            fvTips.Terms.Add(new FuzzyTerm("cheap", new TriangularMembershipFunction(0.0, 5.0, 10.0)));
            fvTips.Terms.Add(new FuzzyTerm("average", new TriangularMembershipFunction(10.0, 15.0, 20.0)));
            fvTips.Terms.Add(new FuzzyTerm("generous", new TriangularMembershipFunction(20.0, 25.0, 30.0)));
            fsTips.Output.Add(fvTips);

            //
            // Create three fuzzy rules
            //
            try
            {
                MamdaniFuzzyRule rule1 = fsTips.ParseRule("if (service is poor )  or (food is rancid) then tips is cheap");
                MamdaniFuzzyRule rule2 = fsTips.ParseRule("if ((service is good)) then tips is average");
                MamdaniFuzzyRule rule3 = fsTips.ParseRule("if (service is excellent) or (food is delicious) then (tips is generous)");

                fsTips.Rules.Add(rule1);
                fsTips.Rules.Add(rule2);
                fsTips.Rules.Add(rule3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Parsing exception: {0}", ex.Message));
                return null;
            }

            return fsTips;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //
            // Create new fuzzy system
            //
            if (_fsTips == null)
            {
                _fsTips = CreateSystem();
                if (_fsTips == null)
                {
                    return;
                }
            }

            //
            // Get variables from the system (for convinience only)
            //
            FuzzyVariable fvService = _fsTips.InputByName("service");
            FuzzyVariable fvFood = _fsTips.InputByName("food");
            FuzzyVariable fvTips = _fsTips.OutputByName("tips");

            //
            // Associate input values with input variables
            //
            Dictionary<FuzzyVariable, double> inputValues = new Dictionary<FuzzyVariable, double>();
            inputValues.Add(fvService, (double)nudInputService.Value);
            inputValues.Add(fvFood, (double)nudInputFood.Value);

            //
            // Calculate result: one output value for each output variable
            //
            Dictionary<FuzzyVariable, double> result = _fsTips.Calculate(inputValues);

            //
            // Get output value for the 'tips' variable
            //
            tbTips.Text = result[fvTips].ToString("f1");
        }
    }
}
