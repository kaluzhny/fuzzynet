using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AI.Fuzzy.Library;

namespace AI.Fuzzy.Samples.CruiseControlSample
{
    public partial class MainForm : Form
    {
        SugenoFuzzySystem _fsCruiseControl = null;

        public MainForm()
        {
            InitializeComponent();
        }

        SugenoFuzzySystem CreateSystem()
        {
            //
            // Create empty Sugeno Fuzzy System
            //
            SugenoFuzzySystem fsCruiseControl = new SugenoFuzzySystem();

            //
            // Create input variables for the system
            //
            FuzzyVariable fvSpeedError = new FuzzyVariable("SpeedError", -20.0, 20.0);
            fvSpeedError.Terms.Add(new FuzzyTerm("slower", new TriangularMembershipFunction(-35.0, -20.0, -5.0)));
            fvSpeedError.Terms.Add(new FuzzyTerm("zero", new TriangularMembershipFunction(-15.0, -0.0, 15.0)));
            fvSpeedError.Terms.Add(new FuzzyTerm("faster", new TriangularMembershipFunction(5.0, 20.0, 35.0)));
            fsCruiseControl.Input.Add(fvSpeedError);

            FuzzyVariable fvSpeedErrorDot = new FuzzyVariable("SpeedErrorDot", -5.0, 5.0);
            fvSpeedErrorDot.Terms.Add(new FuzzyTerm("slower", new TriangularMembershipFunction(-9.0, -5.0, -1.0)));
            fvSpeedErrorDot.Terms.Add(new FuzzyTerm("zero", new TriangularMembershipFunction(-4.0, -0.0, 4.0)));
            fvSpeedErrorDot.Terms.Add(new FuzzyTerm("faster", new TriangularMembershipFunction(1.0, 5.0, 9.0)));
            fsCruiseControl.Input.Add(fvSpeedErrorDot);

            //
            // Create output variables for the system
            //
            SugenoVariable svAccelerate = new SugenoVariable("Accelerate");
            svAccelerate.Functions.Add(fsCruiseControl.CreateSugenoFunction("zero", new double[] { 0.0, 0.0, 0.0 }));
            svAccelerate.Functions.Add(fsCruiseControl.CreateSugenoFunction("faster", new double[] { 0.0, 0.0, 1.0 }));
            svAccelerate.Functions.Add(fsCruiseControl.CreateSugenoFunction("slower", new double[] { 0.0, 0.0, -1.0 }));
            svAccelerate.Functions.Add(fsCruiseControl.CreateSugenoFunction("func", new double[] { -0.04, -0.1, 0.0 }));
            fsCruiseControl.Output.Add(svAccelerate);

            //
            // Create fuzzy rules
            //
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "slower", "slower", "faster");
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "slower", "zero", "faster");
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "slower", "faster", "zero");
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "zero", "slower", "faster");
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "zero", "zero", "func");
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "zero", "faster", "slower");
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "faster", "slower", "zero");
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "faster", "zero", "slower");
            AddSugenoFuzzyRule(fsCruiseControl, fvSpeedError, fvSpeedErrorDot, svAccelerate, "faster", "faster", "slower");

            //
            // Adding the same rules in text form
            //
            ///////////////////////////////////////////////////////////////////
            //SugenoFuzzyRule rule1 = fsCruiseControl.ParseRule("if (SpeedError is slower) and (SpeedErrorDot is slower) then (Accelerate is faster)");
            //SugenoFuzzyRule rule2 = fsCruiseControl.ParseRule("if (SpeedError is slower) and (SpeedErrorDot is zero) then (Accelerate is faster)");
            //SugenoFuzzyRule rule3 = fsCruiseControl.ParseRule("if (SpeedError is slower) and (SpeedErrorDot is faster) then (Accelerate is zero)");
            //SugenoFuzzyRule rule4 = fsCruiseControl.ParseRule("if (SpeedError is zero) and (SpeedErrorDot is slower) then (Accelerate is faster)");
            //SugenoFuzzyRule rule5 = fsCruiseControl.ParseRule("if (SpeedError is zero) and (SpeedErrorDot is zero) then (Accelerate is func)");
            //SugenoFuzzyRule rule6 = fsCruiseControl.ParseRule("if (SpeedError is zero) and (SpeedErrorDot is faster) then (Accelerate is slower)");
            //SugenoFuzzyRule rule7 = fsCruiseControl.ParseRule("if (SpeedError is faster) and (SpeedErrorDot is slower) then (Accelerate is faster)");
            //SugenoFuzzyRule rule8 = fsCruiseControl.ParseRule("if (SpeedError is faster) and (SpeedErrorDot is zero) then (Accelerate is slower)");
            //SugenoFuzzyRule rule9 = fsCruiseControl.ParseRule("if (SpeedError is faster) and (SpeedErrorDot is faster) then (Accelerate is slower)");

            //fsCruiseControl.Rules.Add(rule1);
            //fsCruiseControl.Rules.Add(rule2);
            //fsCruiseControl.Rules.Add(rule3);
            //fsCruiseControl.Rules.Add(rule4);
            //fsCruiseControl.Rules.Add(rule5);
            //fsCruiseControl.Rules.Add(rule6);
            //fsCruiseControl.Rules.Add(rule7);
            //fsCruiseControl.Rules.Add(rule8);
            //fsCruiseControl.Rules.Add(rule9);

            ///////////////////////////////////////////////////////////////////

            return fsCruiseControl;
        }

        void AddSugenoFuzzyRule(
            SugenoFuzzySystem fs,
            FuzzyVariable fv1,
            FuzzyVariable fv2,
            SugenoVariable sv,
            string value1,
            string value2,
            string result)
        {
            SugenoFuzzyRule rule = fs.EmptyRule();
            rule.Condition.Op = OperatorType.And;
            rule.Condition.ConditionsList.Add(rule.CreateCondition(fv1, fv1.GetTermByName(value1)));
            rule.Condition.ConditionsList.Add(rule.CreateCondition(fv2, fv2.GetTermByName(value2)));
            rule.Conclusion.Var = sv;
            rule.Conclusion.Term = sv.GetFuncByName(result);
            fs.Rules.Add(rule);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //
            // Create new fuzzy system
            //
            if (_fsCruiseControl == null)
            {
                _fsCruiseControl = CreateSystem();
                if (_fsCruiseControl == null)
                {
                    return;
                }
            }

            //
            // Get variables from the system (for convinience only)
            //
            FuzzyVariable fvSpeedError = _fsCruiseControl.InputByName("SpeedError");
            FuzzyVariable fvSpeedErrorDot = _fsCruiseControl.InputByName("SpeedErrorDot");
            SugenoVariable svAccelerate = _fsCruiseControl.OutputByName("Accelerate");

            //
            // Fuzzify input values
            //
            Dictionary<FuzzyVariable, double> inputValues = new Dictionary<FuzzyVariable, double>();
            inputValues.Add(fvSpeedError, (double)nudInputSpeedError.Value);
            inputValues.Add(fvSpeedErrorDot, (double)nudInputSpeedErrorDot.Value);

            //
            // Calculate the result
            //
            Dictionary<SugenoVariable, double> result = _fsCruiseControl.Calculate(inputValues);

            tbAccelerate.Text = (result[svAccelerate] * 100.0).ToString("f1");
        }
    }
}
